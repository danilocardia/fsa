using System;
using System.Collections.Generic;
using System.Text;
using FSA.TCC.Simulador;
using System.Threading;
using System.Linq;

namespace FSA.TCC
{
    class Program
    {
        const int tamanho_bloco = 200;
        static int carros = 0;

        static Mapa mapa;
        static List<CarroInstante> entradasDeCarros = new List<CarroInstante>();

        class CarroInstante
        {
            public Carro Carro { get; set; }
            public int Instante { get; set; }

            public CarroInstante(Carro c, int instante)
            {
                Carro = c;
                Instante = instante;
            }
        }

        static void PrepararMapa()
        {
            Rua rua_a = new Rua("A", 2 * tamanho_bloco);
            Rua rua_b = new Rua("B", 3 * tamanho_bloco);
            Rua rua_c = new Rua("C", 4 * tamanho_bloco);
            Rua rua_d = new Rua("D", 4 * tamanho_bloco);
            Rua rua_e = new Rua("E", 3 * tamanho_bloco);
            Rua rua_f = new Rua("F", 3 * tamanho_bloco);
            Rua rua_g = new Rua("G", 4 * tamanho_bloco);
            Rua rua_h = new Rua("H", 2 * tamanho_bloco);
            Rua rua_i = new Rua("I", 5 * tamanho_bloco);
            Rua rua_j = new Rua("J", 1 * tamanho_bloco);
            Rua rua_k = new Rua("K", 5 * tamanho_bloco);
            Rua rua_l = new Rua("L", 5 * tamanho_bloco);

            Cruzamento cruzamento_1 = new Cruzamento(rua_a, rua_d, rua_b, rua_c, true);
            Cruzamento cruzamento_2 = new Cruzamento(rua_d, rua_f, rua_e, rua_g, true);
            Cruzamento cruzamento_3 = new Cruzamento(rua_c, rua_l, rua_h, null, false);
            Cruzamento cruzamento_4 = new Cruzamento(rua_h, rua_j, rua_i, null, false);
            Cruzamento cruzamento_5 = new Cruzamento(rua_g, rua_k, rua_j, null, true);

            Controlador controlador_1 = new Controlador(cruzamento_1);
            Controlador controlador_2 = new Controlador(cruzamento_2);
            Controlador controlador_5 = new Controlador(cruzamento_5);

            Caminho caminho_a = new Caminho("A", rua_b, rua_d, rua_e);
            Caminho caminho_b = new Caminho("B", rua_b, rua_c, rua_h, rua_i);
            Caminho caminho_c = new Caminho("C", rua_a, rua_c, rua_h, rua_i);
            Caminho caminho_d = new Caminho("D", rua_k, rua_g, rua_e);

            mapa = new Mapa(cruzamento_1, cruzamento_2, cruzamento_3, cruzamento_4, cruzamento_5);

            mapa.Controladores.Add(controlador_1);
            mapa.Controladores.Add(controlador_2);
            mapa.Controladores.Add(controlador_5);

            mapa.Caminhos.Add(caminho_a);
            mapa.Caminhos.Add(caminho_b);
            mapa.Caminhos.Add(caminho_c);
            mapa.Caminhos.Add(caminho_d);
        }

        static void PrepararCarros()
        {
            foreach (Caminho caminho in mapa.Caminhos)
            {
                for (int i = 0; i < (caminho.Count * 10); i++)
                {
                    Carro carro = new Carro(Guid.NewGuid().ToString(), caminho, 2.3f, 80);

                    Random gen = new Random();
                    entradasDeCarros.Add(new CarroInstante(carro, gen.Next(i, i * 60)));
                }
            }
        }

        static void IniciarCarros()
        {
            List<Carro> carrosParaIniciar = entradasDeCarros
                                                .Where(ci => ci.Instante == TempoDoSistema.Valor)
                                                .Select(ci => ci.Carro)
                                                .ToList();

            foreach (Carro c in carrosParaIniciar)
            {
                c.Iniciar();
            }
        }

        static void MovimentarCarros()
        {
            List<Carro> carrosParaMovimentar = entradasDeCarros
                                                .Where(ci => ci.Instante < TempoDoSistema.Valor && ci.Carro.Status != StatusCarroEnum.CaminhoConcluido)
                                                .Select(ci => ci.Carro)
                                                .ToList();

            foreach (Carro c in carrosParaMovimentar)
            {
                c.Mover();
            }
        }

        static void AtuarControladores()
        {
            foreach (Controlador controlador in mapa.Controladores)
            {
                controlador.Atuar();
            }
        }

        public static void Main(string[] args)
        {
            PrepararMapa();
            PrepararCarros();

            while (entradasDeCarros.Count(ci => ci.Carro.Status != StatusCarroEnum.CaminhoConcluido) > 0)
            {
                IniciarCarros();
                MovimentarCarros();

                ExibirLog();

                AtuarControladores();
                TempoDoSistema.Incrementar();
            }

            GerarResultados();
        }

        static void GerarResultados()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Danilo\Desktop\teste_tcc\result_sim.csv"))
            {
                file.WriteLine("caminho;carro;entrada;semaforo;saida");

                foreach (Carro c in entradasDeCarros.Select(ci => ci.Carro))
                {
                    file.WriteLine("{0};{1};{2};{3};{4}", c.Caminho.Id, c.Id, c.Dados.instanteEntradaSistema, c.Dados.tempoSemaforo, c.Dados.instanteSaidaSistema);
                }
            }
        }

        static void ExibirLog()
        {
            int carrosAguardando = entradasDeCarros.Count(ci => ci.Carro.Status == StatusCarroEnum.AguardandoEntrada);
            int carrosEmMovimento = entradasDeCarros.Count(ci => ci.Carro.Status == StatusCarroEnum.EmMovimento);
            int carrosParadoSemaforo = entradasDeCarros.Count(ci => ci.Carro.Status == StatusCarroEnum.ParadoSemaforo);
            int carrosConcluido = entradasDeCarros.Count(ci => ci.Carro.Status == StatusCarroEnum.CaminhoConcluido);
            
            Console.Clear();
            
            Console.WriteLine("Tempo do sistema: {0}", TempoDoSistema.Valor);
            Console.WriteLine("Qtd. Carros Aguardando: {0}", carrosAguardando);
            Console.WriteLine("Qtd. Carros Movimento : {0}", carrosEmMovimento);
            Console.WriteLine("Qtd. Carros Semaforo : {0}", carrosParadoSemaforo);
            Console.WriteLine("Qtd. Carros Finalizado : {0}", carrosConcluido);

            ExibeCarros(entradasDeCarros.Where(ci => ci.Carro.Status != StatusCarroEnum.AguardandoEntrada).Take(20).Select(ci => ci.Carro).ToArray());
            ExibeSemaforos(mapa.Cruzamentos.ToArray());

            Thread.Sleep(25);
        }

        static void MainOld(string[] args)
        {
            Rua r1 = new Rua("Rua 1", 10000);
            Rua r2 = new Rua("Rua 2", 10000);
            Rua r3 = new Rua("Rua 3", 10000);
            Rua r4 = new Rua("Rua 4", 10000);

            Caminho caminho1 = new Caminho();
            caminho1.Add(r1);
            caminho1.Add(r2);

            Caminho caminho2 = new Caminho();
            caminho2.Add(r1);
            caminho2.Add(r4);

            Cruzamento cz1 = new Cruzamento(r1, r2, r3, r4);
            Controlador ct1 = new Controlador(cz1);

            Carro c1 = new Carro("Carro 1", caminho1, 0.05f, 80);

            /*c1.TrocaDeRua += new Carro.CarroTrocaHandler(c1_TrocaDeRua);
            c1.AguardandoSemaforo += new Carro.CarroAguardandoHandler(c1_AguardandoSemaforo);
            c1.ImpedimentoDeProgresso += new Carro.CarroImpedidoHandler(c1_ImpedimentoDeProgresso);*/
            c1.TerminoCaminho += new Carro.CarroTerminoHandler(c1_TerminoCaminho);
            c1.InicioCaminho += new Carro.CarroInicioHandler(c1_InicioCaminho);

            Carro c2 = new Carro("Carro 2", caminho2, 0.055f, 120);

            /*c2.TrocaDeRua += new Carro.CarroTrocaHandler(c1_TrocaDeRua);
            c2.AguardandoSemaforo += new Carro.CarroAguardandoHandler(c1_AguardandoSemaforo);
            c2.ImpedimentoDeProgresso += new Carro.CarroImpedidoHandler(c1_ImpedimentoDeProgresso);*/
            c2.TerminoCaminho += new Carro.CarroTerminoHandler(c1_TerminoCaminho);
            c2.InicioCaminho += new Carro.CarroInicioHandler(c1_InicioCaminho);

            c1.Iniciar();
            carros = 1;

            while (carros > 0)
            {
                ct1.Atuar();

                if (TempoDoSistema.Valor == 20)
                {
                    c2.Iniciar();
                }

                c1.Mover();
                c2.Mover();

                Console.Clear();
                ExibeCarros(c1, c2);
                ExibeSemaforos(cz1);
                //ExibeSensores(cz1);

                TempoDoSistema.Incrementar();
                Thread.Sleep(100);
            }

            //DesenhaMapa();

            Console.ReadLine();
        }

        static void DesenhaMapa()
        {
            List<Rua> ruas = new List<Rua>();

            for (int i = 0; i < 12; i++)
            {
                ruas.Add(new Rua("Rua " + i, 5));
            }

            List<Cruzamento> cruzamentos = new List<Cruzamento>();

            for (int i = 0; i < 4; i++)
            {
                cruzamentos.Add(new Cruzamento(ruas[i], ruas[i + 3], ruas[i + 5], ruas[i + 2]));
            }

            Mapa m = new Mapa(cruzamentos.ToArray());
        }

        static void ExibeCarros(params Carro[] lista_carros)
        {
            Console.WriteLine("Tempo: {0}\n", TempoDoSistema.Valor.ToString().PadLeft(4, '0'));

            Console.WriteLine("TABELA DE PROGRESSO");
            Console.WriteLine("Carro\tRua\tProgresso\t\t\t\tVelocidade");
            foreach (Carro c in lista_carros.Where(c => c.Caminho.RuaAtual != null).OrderBy(c => c.Caminho.RuaAtual.Id).ThenBy(c => c.Posicao))
            {
                string carro, rua = "", progresso = "", velocidade = "";

                carro = c.Id;
                if (c.Caminho.RuaAtual != null)
                {
                    rua = c.Caminho.RuaAtual.Id;

                    int pg = Convert.ToInt32((c.Posicao / c.Caminho.RuaAtual.Tamanho) * 30);

                    for (int i = 0; i < pg; i++)
                        progresso += "=";

                    for (int i = 0; i < 30 - pg; i++)
                        progresso += "_";

                    velocidade = c.Velocidade.ToString() + "m/s";
                }

                Console.WriteLine("{0}\t{1}\t[{2}]\t{3}", carro.Substring(0, 6), rua, progresso, velocidade);
            }

            Console.WriteLine("\n");
        }

        static void ExibeSemaforos(params Cruzamento[] cz)
        {
            Console.WriteLine("ESTADO DOS SEMAFOROS");
            Console.WriteLine("Rua\t\tEstado\t\tCountdown");

            for (int i = 0; i < cz.Length; i++)
            {
                foreach (Rua r in cz[i].Ruas.Where(r => r.Semaforo != null))
                {
                    string carros = "";

                    foreach (Carro c in r.CarrosNaRua)
                        carros += c.Id + " ";

                    //Console.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}", r.Id, r.Semaforo.Estado.ToString(), r.Semaforo.TempoRestante.ToString().PadLeft(2, '0'), "(" + carros + ")");
                    Console.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}", r.Id, r.Semaforo.Estado.ToString(), r.Semaforo.TempoRestante.ToString().PadLeft(2, '0'), "");
                }
            }

            Console.WriteLine("\n");
        }

        static void ExibeSensores(Cruzamento cz)
        {
            Console.WriteLine("ESTADO DOS SENSORES");
            Console.WriteLine("Rua\t\tSensor\t\t\tValor");

            foreach (Rua r in cz.Ruas)
            {
                Console.WriteLine("{0}\t\t{1}\t\t{2}", r.Id, r.Sensores[0].Nome, r.Sensores[0].Resultado.ToString() + "m/s");
            }
        }

        static void c1_InicioCaminho(Carro c)
        {
            //Console.WriteLine("{1} - O carro {0} iniciou seu caminho", c.Id, TempoDoSistema.Valor.ToString().PadLeft(4, '0'));
            carros++;
        }

        static void c1_ImpedimentoDeProgresso(Carro impedido, Carro impedidor)
        {
            Console.WriteLine("{0} - O carro {1} está impedindo o carro {2} de progredir", TempoDoSistema.Valor.ToString().PadLeft(4, '0'), impedidor.Id, impedido.Id);
        }

        static void c1_AguardandoSemaforo(Carro c)
        {
            Console.WriteLine("{1} - O carro {0} está aguardando no semáforo", c.Id, TempoDoSistema.Valor.ToString().PadLeft(4, '0'));
        }

        static void c1_TerminoCaminho(Carro c)
        {
            //Console.WriteLine("{1} - O carro {0} terminou seu caminho", c.Id, TempoDoSistema.Valor.ToString().PadLeft(4, '0'));
            carros--;
        }

        static void c1_TrocaDeRua(Carro c, Rua origem, Rua destino)
        {
            Console.WriteLine("{3} - O carro {0} saiu da rua {1} e entrou na rua {2}", c.Id, origem.Id, destino.Id, TempoDoSistema.Valor.ToString().PadLeft(4, '0'));
        }
    }
}

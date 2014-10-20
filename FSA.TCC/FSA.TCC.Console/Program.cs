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
        static int carros = 0;
        static int tempo = 0;

        static void Main(string[] args)
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
                ct1.Avancar();

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
                Thread.Sleep(175);
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
            foreach (Carro c in lista_carros)
            {
                string carro, rua = "", progresso = "", velocidade = "";

                carro = c.Id;
                if (c.Caminho.RuaAtual != null)
                {
                    rua = c.Caminho.RuaAtual.Id;

                    int pg = Convert.ToInt32((c.Posicao / c.Caminho.RuaAtual.Tamanho) * 30);

                    for(int i = 0; i < pg; i++)
                        progresso += "=" ;

                    for (int i = 0; i < 30 - pg; i++)
                        progresso += "_";

                    velocidade = c.Velocidade.ToString() + "m/s";
                }

                Console.WriteLine("{0}\t{1}\t[{2}]\t{3}", carro, rua, progresso, velocidade);
            }

            Console.WriteLine("\n");
        }

        static void ExibeSemaforos(Cruzamento cz)
        {
            Console.WriteLine("ESTADO DOS SEMAFOROS");
            Console.WriteLine("Rua\t\tEstado\t\tCountdown");

            foreach (Rua r in cz.Ruas)
            {
                string carros = "";

                foreach(Carro c in r.CarrosNaRua)
                    carros += c.Id + " ";

                Console.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}", r.Id, r.Semaforo.Estado.ToString(),r.Semaforo.TempoRestante.ToString().PadLeft(2, '0'), "(" + carros + ")");
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

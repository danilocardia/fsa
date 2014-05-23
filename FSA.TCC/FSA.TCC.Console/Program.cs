using System;
using System.Collections.Generic;
using System.Text;
using FSA.TCC.Simulador;

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

            Carro c1 = new Carro("Carro 1", caminho1);
            c1.Velocidade = 180;
            
            c1.TrocaDeRua += new Carro.CarroTrocaHandler(c1_TrocaDeRua);
            c1.TerminoCaminho += new Carro.CarroTerminoHandler(c1_TerminoCaminho);
            c1.AguardandoSemaforo += new Carro.CarroAguardandoHandler(c1_AguardandoSemaforo);
            c1.ImpedimentoDeProgresso += new Carro.CarroImpedidoHandler(c1_ImpedimentoDeProgresso);
            c1.InicioCaminho += new Carro.CarroInicioHandler(c1_InicioCaminho);

            Carro c2 = new Carro("Carro 2", caminho2);
            c2.Velocidade = 360;

            c2.TrocaDeRua += new Carro.CarroTrocaHandler(c1_TrocaDeRua);
            c2.TerminoCaminho += new Carro.CarroTerminoHandler(c1_TerminoCaminho);
            c2.AguardandoSemaforo += new Carro.CarroAguardandoHandler(c1_AguardandoSemaforo);
            c2.ImpedimentoDeProgresso += new Carro.CarroImpedidoHandler(c1_ImpedimentoDeProgresso);
            c2.InicioCaminho += new Carro.CarroInicioHandler(c1_InicioCaminho);

            c1.Iniciar();

            while (carros > 0)
            {
                ct1.Avancar();

                if (TempoDoSistema.Valor == 26)
                {
                    c2.Iniciar();
                }

                c1.Mover();
                c2.Mover();

                TempoDoSistema.Incrementar();
            }

            Console.ReadLine();
        }

        static void c1_InicioCaminho(Carro c)
        {
            Console.WriteLine("{1} - O carro {0} iniciou seu caminho", c.Id, TempoDoSistema.Valor.ToString().PadLeft(4, '0'));
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
            Console.WriteLine("{1} - O carro {0} terminou seu caminho", c.Id, TempoDoSistema.Valor.ToString().PadLeft(4, '0'));
            carros--;
        }

        static void c1_TrocaDeRua(Carro c, Rua origem, Rua destino)
        {
            Console.WriteLine("{3} - O carro {0} saiu da rua {1} e entrou na rua {2}", c.Id, origem.Id, destino.Id, TempoDoSistema.Valor.ToString().PadLeft(4, '0'));
        }
    }
}

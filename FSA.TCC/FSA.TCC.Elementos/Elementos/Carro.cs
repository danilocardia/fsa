using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FSA.TCC.Simulador
{
    public class Carro
    {
        public delegate void CarroTrocaHandler(Carro c, Rua origem, Rua destino);
        public delegate void CarroAguardandoHandler(Carro c);
        public delegate void CarroTerminoHandler(Carro c);
        public delegate void CarroInicioHandler(Carro c);
        public delegate void CarroImpedidoHandler(Carro impedido, Carro impedidor);
        public event CarroTrocaHandler TrocaDeRua;
        public event CarroAguardandoHandler AguardandoSemaforo;
        public event CarroTerminoHandler TerminoCaminho;
        public event CarroInicioHandler InicioCaminho;
        public event CarroImpedidoHandler ImpedimentoDeProgresso;

        public string Id { get; set; }
        public Caminho Caminho { get; set; }
        public float Posicao { get; set; }
        public float Velocidade { get; set; } // m/s
        public float Tamanho
        {
            get { return 2; }
        }

        private bool isIniciado = false;

        public Carro(string id, Caminho c)
        {
            Id = id;
            Caminho = c;
            Posicao = 0;
        }

        public void Iniciar()
        {
            Caminho.RuaAtual.CarrosNaRua.Add(this); // coloca o carro na rua atual
            isIniciado = true;

            // Notifica o inicio do caminho
            InicioCaminho(this);
        }

        public void Mover()
        {
            if (Caminho.RuaAtual == null || isIniciado == false)
                return;
                        
            AtualizarPosicao();

            if (Posicao >= Caminho.RuaAtual.Tamanho)
            {
                if (Caminho.RuaAtual.Semaforo.Estado == EstadoSemaforo.Aberto)
                {
                    Rua anterior = Caminho.RuaAtual;

                    if (Caminho.Avancar())
                    {
                        anterior.CarrosNaRua.Remove(this); // tira o carro da rua anterior
                        Caminho.RuaAtual.CarrosNaRua.Add(this); // coloca o carro na rua atual

                        TrocaDeRua(this, anterior, Caminho.RuaAtual);
                        Posicao = 0;
                    }
                    else
                    {
                        TerminoCaminho(this);
                    }
                }
                else
                {
                    AguardandoSemaforo(this);
                }
            }
        }

        public void AtualizarPosicao()
        {
            float avanco = this.Velocidade;
            var carroEmFrente = Caminho.RuaAtual.CarrosNaRua.Where(cr => cr.Posicao >= this.Posicao && cr != this).OrderBy(cr => cr.Posicao).FirstOrDefault();

            // se não tem carro em frente ou o avanco é menor que a distancia entre os dois carros
            if (carroEmFrente == null || (avanco < (carroEmFrente.Posicao - carroEmFrente.Tamanho - this.Posicao)))
            {
                // avança a posição do carro
                this.Posicao += avanco;
            }
            // senão, o avanco é maior que a distancia entre o carro e o carro em frente
            else
            {
                // o carro fica meio metro atrás do carro da frente
                this.Posicao = carroEmFrente.Posicao - carroEmFrente.Tamanho - 0.5f;

                // notifica o impedimento de progresso
                ImpedimentoDeProgresso(this, carroEmFrente);
            }
        }
    }
}

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

        public DadosCarro Dados { get; set; }

        public string Id { get; set; }
        public Caminho Caminho { get; set; }
        public float Posicao { get; set; }
        private float _velocidade = 0;
        public float Velocidade
        {
            get
            {
                return _velocidade;
            }
        } // km/h
        public float VelocidadeLimite { get; set; } // km/h
        public float Aceleracao { get; set; }
        public float Tamanho
        {
            get { return 2; }
        }

        private int tempoInicio = -1;
        public bool isIniciado = false;
        public StatusCarroEnum Status = StatusCarroEnum.AguardandoEntrada;

        public Carro(string id, Caminho c, float aceleracao, float velocidadeLimite)
        {
            Id = id;
            Caminho = new Caminho(c);
            Posicao = 0;
            Aceleracao = aceleracao / 3.6f;
            VelocidadeLimite = velocidadeLimite;
            Dados = new DadosCarro();
        }

        public void Iniciar()
        {
            Caminho.RuaAtual.CarrosNaRua.Add(this); // coloca o carro na rua atual
            isIniciado = true;

            // Notifica o inicio do caminho
            if (InicioCaminho != null)
            {
                InicioCaminho(this);
            }

            Dados.instanteEntradaSistema = TempoDoSistema.Valor;
        }

        public void Mover()
        {
            if (Caminho.RuaAtual == null || isIniciado == false)
                return;


            if (Posicao <= Caminho.RuaAtual.Tamanho)
            {
                AtualizarPosicao();
            }
            else
            {
                _velocidade = 0;

                if (Caminho.RuaAtual.Semaforo == null || Caminho.RuaAtual.Semaforo.Estado == EstadoSemaforo.Aberto)
                {
                    Rua anterior = Caminho.RuaAtual;

                    if (Caminho.Avancar())
                    {
                        anterior.CarrosNaRua.Remove(this); // tira o carro da rua anterior
                        Caminho.RuaAtual.CarrosNaRua.Add(this); // coloca o carro na rua atual

                        if (TrocaDeRua != null)
                        {
                            TrocaDeRua(this, anterior, Caminho.RuaAtual);
                        }
                        Posicao = 0;

                        this.Status = StatusCarroEnum.EmMovimento;
                        this.Dados.instantesTrocaRua.Add(TempoDoSistema.Valor);
                    }
                    else
                    {
                        anterior.CarrosNaRua.Remove(this); // tira o carro da rua anterior                        
                        if (TerminoCaminho != null)
                        {
                            TerminoCaminho(this);
                        }

                        this.Status = StatusCarroEnum.CaminhoConcluido;
                        this.Dados.instanteSaidaSistema = TempoDoSistema.Valor;
                    }
                }
                else
                {
                    // O carro está parado
                    tempoInicio = -1;

                    if (AguardandoSemaforo != null)
                    {
                        AguardandoSemaforo(this);
                    }

                    this.Status = StatusCarroEnum.ParadoSemaforo;
                    this.Dados.tempoSemaforo += 1;
                }
            }
        }

        public void AtualizarPosicao()
        {
            // seleciona os carros que estao na frente
            var carroEmFrente = Caminho.RuaAtual.CarrosNaRua.Where(cr => cr.Posicao >= Posicao && cr != this).OrderBy(cr => cr.Posicao).FirstOrDefault();

            AtualizarVelocidade(carroEmFrente);

            // se não tem carro em frente ou o avanco é menor que a distancia entre os dois carros
            if (carroEmFrente == null || (Velocidade < (carroEmFrente.Posicao - carroEmFrente.Tamanho - Posicao)))
            {
                // avança a posição do carro
                Posicao += Velocidade;

                this.Status = StatusCarroEnum.EmMovimento;
                this.Dados.tempoMovimento += 1;
            }
            // senão, o avanco é maior que a distancia entre o carro e o carro em frente
            else
            {
                // o carro fica um metro atrás do carro da frente
                float novaPosicao = carroEmFrente.Posicao - carroEmFrente.Tamanho - 1f;

                // atualiza a velocidade para a realmente utilizada
                _velocidade = novaPosicao - Posicao;

                // atualiza a posicao
                Posicao = novaPosicao;

                // notifica o impedimento de progresso
                if (ImpedimentoDeProgresso != null)
                {
                    ImpedimentoDeProgresso(this, carroEmFrente);
                }

                if (carroEmFrente.Status == StatusCarroEnum.ParadoSemaforo)
                {
                    this.Status = StatusCarroEnum.ParadoSemaforo;
                    this.Dados.tempoSemaforo += 1;
                    _velocidade = 0;
                }
                else
                {
                    this.Status = StatusCarroEnum.EmMovimento;
                    this.Dados.tempoMovimento += 1;
                }
            }
        }

        public void AtualizarVelocidade(Carro carroEmFrente = null)
        {
            if (tempoInicio == -1)
                tempoInicio = TempoDoSistema.Valor;

            float novaVelocidade = Aceleracao * (float)Math.Pow((TempoDoSistema.Valor - tempoInicio), 2);

            // se a velocidade calculada for maior que a velocidade limite, retorna a velocidade limite, caso contrário retorna a velocidade calculada
            _velocidade = VelocidadeLimite > novaVelocidade ? novaVelocidade : VelocidadeLimite;

            //if (this.Caminho.RuaAtual.Semaforo == null || this.Caminho.RuaAtual.Semaforo.Estado == EstadoSemaforo.Fechado)
            //{
                float posicaoObstaculo = 0;

                if (carroEmFrente != null)
                {
                    posicaoObstaculo = carroEmFrente.Posicao;
                }
                else
                {
                    posicaoObstaculo = Caminho.RuaAtual.Tamanho;
                }

                float segundosAteColisao = (posicaoObstaculo - Posicao) / Velocidade;

                if (segundosAteColisao <= 1)
                {
                    _velocidade = (posicaoObstaculo - Posicao) + 1;
                }
                else
                {
                    // usando a equação de torricelli (v = v0 + at, invertendo ficou a = (v - v0) / t, mas como v deverá ser zero, então: a = v0 / t
                    float aceleracaoParaParar = Velocidade / segundosAteColisao;
                    _velocidade -= aceleracaoParaParar;
                }
            //}
        }
    }

    public enum StatusCarroEnum {
        AguardandoEntrada,
        ParadoSemaforo,
        EmMovimento,
        CaminhoConcluido
    }

    public class DadosCarro
    {
        public int instanteEntradaSistema = 0;
        public int tempoMovimento = 0;
        public int tempoSemaforo = 0;
        public List<int> instantesTrocaRua = new List<int>();
        public int instanteSaidaSistema = 0;
    }
}

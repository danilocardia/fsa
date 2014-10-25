using System;
using System.Collections.Generic;
using System.Text;

namespace FSA.TCC.Simulador
{
    public class Semaforo
    {
        private int contador;
        private int ultimoInstanteDeAtualizacao = 0;
        private Dictionary<EstadoSemaforo, int> configuracao;
        public delegate void TrocaDeEstadoHandler();

        public event TrocaDeEstadoHandler TrocaDeEstado;
        public EstadoSemaforo Estado { get; set; }
        public int TempoRestante
        {
            get
            {
                return configuracao[Estado] - contador;
            }
        }

        public Semaforo()
        {
            configuracao = new Dictionary<EstadoSemaforo, int>();
            configuracao[EstadoSemaforo.Aberto] = 40;
            configuracao[EstadoSemaforo.Fechado] = 40;

            Estado = EstadoSemaforo.Fechado;
        }

        private void TrocaEstado()
        {
            if (Estado == EstadoSemaforo.Aberto)
            {
                Estado = EstadoSemaforo.Fechado;
            }
            else
            {
                Estado = EstadoSemaforo.Aberto;
            }

            if (TrocaDeEstado != null)
                TrocaDeEstado();
        }

        public void Avancar()
        {
            if (TempoDoSistema.Valor == ultimoInstanteDeAtualizacao)
                return;

            if (contador == configuracao[Estado])
            {
                TrocaEstado();
                contador = 0;
            }
            else
            {
                contador++;
            }

            ultimoInstanteDeAtualizacao = TempoDoSistema.Valor;
        }
    }

    public enum EstadoSemaforo
    {
        Aberto,
        Fechado
    }
}

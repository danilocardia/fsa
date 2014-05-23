using System;
using System.Collections.Generic;
using System.Text;

namespace FSA.TCC.Simulador
{
    public class Semaforo
    {
        public EstadoSemaforo Estado { get; set; }

        private int contador;
        private Dictionary<EstadoSemaforo, int> configuracao;

        public Semaforo()
        {
            configuracao = new Dictionary<EstadoSemaforo, int>();
            configuracao[EstadoSemaforo.Aberto] = 30;
            configuracao[EstadoSemaforo.Fechado] = 30;

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
        }

        public void Avancar()
        {
            if (contador == configuracao[Estado])
            {
                TrocaEstado();
                contador = 0;
            }
            else
            {
                contador++;
            }
        }
    }

    public enum EstadoSemaforo
    {
        Aberto,
        Fechado
    }
}

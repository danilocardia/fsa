using System;
using System.Collections.Generic;
using System.Text;

namespace FSA.TCC.Simulador
{
    public class Controlador
    {
        private Cruzamento Cruzamento { get; set; }

        public Controlador(Cruzamento cruzamento)
        {
            Cruzamento = cruzamento;

            EstadoSemaforo es = EstadoSemaforo.Fechado;
            foreach (Rua rua in cruzamento.Ruas)
            {
                if (rua.Semaforo != null)
                {
                    rua.Semaforo.Estado = es;

                    if (es == EstadoSemaforo.Aberto)
                        es = EstadoSemaforo.Fechado;
                    else
                        es = EstadoSemaforo.Aberto;

                    rua.Semaforo.TrocaDeEstado += Semaforo_TrocaDeEstado;
                }
            }
        }

        void Semaforo_TrocaDeEstado()
        {

        }

        public void Atuar()
        {
            foreach (Rua rua in Cruzamento.Ruas)
            {
                if (rua.Semaforo != null)
                    rua.Semaforo.Avancar();

                foreach (ISensor sensor in rua.Sensores)
                {
                    sensor.Calcular();
                }
            }
        }
    }
}

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

            foreach (Rua rua in cruzamento.Ruas)
            {
                if (rua.Semaforo != null)
                {
                    rua.Semaforo.TrocaDeEstado += Semaforo_TrocaDeEstado;
                }
            }
        }

        void Semaforo_TrocaDeEstado()
        {
            
        }

        public void Avancar()
        {
            foreach (Rua rua in Cruzamento.Ruas)
            {
                rua.Semaforo.Avancar();

                foreach (ISensor sensor in rua.Sensores)
                {
                    sensor.Calcular();
                }
            }
        }
    }
}

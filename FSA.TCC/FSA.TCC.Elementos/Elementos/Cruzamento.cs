using System;
using System.Collections.Generic;
using System.Text;

namespace FSA.TCC.Simulador
{
    public class Cruzamento
    {
        public List<Rua> Ruas { get; set; }

        public Cruzamento(Rua norte, Rua sul, Rua leste = null, Rua oeste = null, bool possuiSemaforo = true)
        {
            Ruas = new List<Rua>();

            Ruas.Add(norte);
            Ruas.Add(sul);

            if (leste != null)
            {
                Ruas.Add(leste);
            }

            if (oeste != null)
            {
                Ruas.Add(oeste);
            }

            if (!possuiSemaforo)
            {
                foreach (Rua r in Ruas)
                {
                    r.Semaforo = null; // Remove o semáforo da rua
                }
            }
        }
    }
}

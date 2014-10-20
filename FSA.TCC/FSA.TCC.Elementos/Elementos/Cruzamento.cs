using System;
using System.Collections.Generic;
using System.Text;

namespace FSA.TCC.Simulador
{
    public class Cruzamento
    {
        public List<Rua> Ruas { get; set; }

        public Cruzamento(Rua r1, Rua r2, Rua r3 = null, Rua r4 = null, bool possuiSemaforo = true)
        {
            Ruas = new List<Rua>();

            Ruas.Add(r1);
            Ruas.Add(r2);

            if (r3 != null)
            {
                Ruas.Add(r3);
            }

            if (r4 != null)
            {
                Ruas.Add(r4);
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

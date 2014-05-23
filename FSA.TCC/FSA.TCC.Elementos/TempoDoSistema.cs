using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSA.TCC
{
    public class TempoDoSistema
    {
        private static int tempo = 0;
        private static object sync = new object();

        public static void Incrementar()
        {
            lock (sync)
            {
                tempo++;
            }
        }

        public static int Valor
        {
            get
            {
                lock (sync)
                {
                    return tempo;
                }
            }
        }
    }
}

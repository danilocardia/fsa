using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSA.TCC.Simulador.Sensor
{
    public class Amostra
    {
        public int Tempo { get; set; }
        public float Valor { get; set; }

        public Amostra()
        {

        }

        public Amostra(float v)
        {
            Tempo = TempoDoSistema.Valor;
            Valor = v;
        }
    }
}

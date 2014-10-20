using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSA.TCC.Simulador
{
    public class Mapa
    {
        public List<Cruzamento> Cruzamentos = new List<Cruzamento>();

        public Mapa(params Cruzamento[] cruzamentos)
        {
            Cruzamentos.AddRange(cruzamentos);
        }        
    }
}

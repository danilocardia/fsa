using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSA.TCC.Simulador
{
    public class Simulador
    {
        public List<Controlador> Controladores { get; set; }

        public Simulador()
        {
            Inicializa();
        }

        public Simulador(params Controlador[] controladores)
        {
            Inicializa();
            Controladores.AddRange(controladores);
        }

        private void Inicializa()
        {
            Controladores = new List<Controlador>();
        }
    }
}

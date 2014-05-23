using System;
using System.Collections.Generic;
using System.Text;

namespace FSA.TCC.Simulador
{
    public class Caminho : List<Rua>
    {
        int atual = 0;
        public Rua RuaAtual
        {
            get
            {
                return atual < this.Count ? this[atual] : null;
            }
        }
        public bool Avancar()
        {
            if (this.Count == atual + 1)
            {
                atual++;
                return false;
            }

            atual++;
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FSA.TCC.Simulador
{
    public class Caminho : List<Rua>
    {
        public Caminho() { }

        public Caminho(Caminho origem)
        {
            for (int i = 0; i < origem.Count; i++)
                this.Add(origem[i]);

            Id = origem.Id;
        }

        public Caminho(string id, params Rua[] ruas)
        {
            Id = id;

            for (int i = 0; i < ruas.Length; i++)
                this.Add(ruas[i]);
        }

        public Caminho(params Rua[] ruas)
        {
            for (int i = 0; i < ruas.Length; i++)
                this.Add(ruas[i]);
        }

        int atual = 0;

        public string Id { get; set; }
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

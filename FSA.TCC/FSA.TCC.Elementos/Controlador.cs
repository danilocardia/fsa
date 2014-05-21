﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FSA.TCC.Elementos
{
    public class Controlador
    {
        private Cruzamento Cruzamento { get; set; }

        public Controlador(Cruzamento cruzamento)
        {
            Cruzamento = cruzamento;
        }

        public void Avancar()
        {
            foreach (Rua rua in Cruzamento.Ruas)
            {
                rua.Semaforo.Avancar();
            }
        }
    }
}

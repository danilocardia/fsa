using System;
using System.Collections.Generic;
using System.Text;

namespace FSA.TCC.Simulador
{
    public interface ISensor
    {
        string Nome { get; }
        Rua Rua { get; set; }
        float Resultado { get; set; }
        void Calcular();
    }
}

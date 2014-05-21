using System;
using System.Collections.Generic;
using System.Text;

namespace FSA.TCC.Elementos
{
    public interface ISensor
    {
        Rua Rua { get; set; }
        float Calcular();
    }
}

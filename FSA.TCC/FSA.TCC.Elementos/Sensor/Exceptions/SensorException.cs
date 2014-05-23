using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSA.TCC.Simulador.Sensor.Exceptions
{
    public class SensorSemRuaException : Exception
    {
        public SensorSemRuaException()
            : base("O sensor não possui rua para calcular")
        {

        }
    }
    public class RuaSemCarrosException : Exception
    {
        public RuaSemCarrosException()
            : base("A rua não possui carros para calcular")
        {

        }
    }
}

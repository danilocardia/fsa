using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSA.TCC.Elementos.Sensor.Exceptions
{
    public class SensorException : Exception
    {
        public SensorException(string msg)
            : base(msg)
        {

        }
    }
}

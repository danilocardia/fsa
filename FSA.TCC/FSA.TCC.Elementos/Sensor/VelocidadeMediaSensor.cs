using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSA.TCC.Elementos.Sensor.Exceptions;

namespace FSA.TCC.Elementos.Sensor
{
    public class VelocidadeMediaSensor : ISensor
    {
        public Rua Rua
        {
            get;
            set;
        }

        public float Calcular()
        {
            if (Rua == null)
                throw new SensorException("O sensor não possui rua para calcular");
                        
            foreach (Carro c in Rua.CarrosNaRua)
            {

            }

            return 0;
        }
    }
}

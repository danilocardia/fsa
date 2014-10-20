using FSA.TCC.Simulador.Sensor.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSA.TCC.Simulador.Sensor
{
    class QuantidadeCarrosSensor : ISensor
    {
        private class ValorCarro
        {

            public Carro Carro { get; set; }
            public List<Amostra> Amostras { get; set; }
            public float Resultado { get; set; }

            public ValorCarro(Carro c)
            {
                Carro = c;
                Amostras = new List<Amostra>();
            }
        };

        public string Nome
        {
            get { return "Quant. Carros na Rua"; }
        }

        public Rua Rua { get; set; }
        public float Resultado { get; set; }

        public QuantidadeCarrosSensor(Rua r)
        {
            Rua = r;
        }

        public void Calcular()
        {
            if (Rua == null)
                throw new SensorSemRuaException();

            this.Resultado = Rua.CarrosNaRua.Count;
        }
    }
}

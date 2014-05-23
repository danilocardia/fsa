using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSA.TCC.Simulador.Sensor.Exceptions;

namespace FSA.TCC.Simulador.Sensor
{
    public class VelocidadeMediaNoCaminhoSensor : ISensor
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
        
        private List<ValorCarro> ValoresCarro = new List<ValorCarro>();

        public Rua Rua { get; set; }
        public float Resultado { get; set; }

        public VelocidadeMediaNoCaminhoSensor()
        {

        }

        public VelocidadeMediaNoCaminhoSensor(Rua r)
        {
            Rua = r;
        }

        public void Calcular()
        {
            if (Rua == null)
                throw new SensorSemRuaException();

            if (Rua.CarrosNaRua.Count == 0)
            {
                this.Resultado = 0;
                return;
            }

            // colhe dados dos carros que já estão sendo analisados
            foreach (ValorCarro vc in ValoresCarro)
            {
                // se o carro a ser analisado está na rua
                if (Rua.CarrosNaRua.Contains(vc.Carro))
                {
                    // armazena mais uma posição para análise da velocidade média
                    vc.Amostras.Add(new Amostra(vc.Carro.Posicao));
                }
                // senão, remove ele da tabela de análise
                else
                {
                    ValoresCarro.Remove(vc);
                }
            }
            
            // colhe dados dos carros que não estão sendo analisados ainda
            foreach (Carro c in Rua.CarrosNaRua.Where(cc => !ValoresCarro.Select(vc => vc.Carro).Contains(cc)))
            {
                ValorCarro vc = new ValorCarro(c);
                vc.Amostras.Add(new Amostra(c.Posicao));

                ValoresCarro.Add(vc);
            }

            // calula a velocidade média a partir das posições e preenche a propriedade Resultado
            foreach (ValorCarro vc in ValoresCarro)
            {
                if (vc.Amostras.Count > 2)
                {
                    // Vm = (Xf - Xi) / (tf - ti)
                    vc.Resultado = (vc.Amostras.Max(a => a.Valor) - vc.Amostras.Min(a => a.Valor)) / (vc.Amostras.Max(a => a.Tempo) - vc.Amostras.Min(a => a.Tempo));
                }
            }

            this.Resultado = ValoresCarro.Average(vc => vc.Resultado);
        }
    }
}

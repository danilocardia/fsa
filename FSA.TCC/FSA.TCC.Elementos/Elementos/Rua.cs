using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSA.TCC.Simulador.Sensor;

namespace FSA.TCC.Simulador
{
    public class Rua
    {
        public string Id { get; set; }
        public float Tamanho { get; set; }
        public Semaforo Semaforo { get; set; }
        public List<Carro> CarrosNaRua { get; set; }
        public List<ISensor> Sensores { get; set; }

        public Rua(string id, int tamanho)
        {
            Id = id;
            Tamanho = tamanho;

            Semaforo = new Semaforo();
            CarrosNaRua = new List<Carro>();
            Sensores = new List<ISensor>();

            Sensores.Add(new VelocidadeMediaNoCaminhoSensor(this));
            Sensores.Add(new QuantidadeCarrosSensor(this));
        }
    }
}

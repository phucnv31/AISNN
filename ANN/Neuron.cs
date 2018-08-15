using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    public class Neuron
    {
        public List<Dendrite> Dendrites { get; set; }
        public double Bias { get; set; }
        public double Delta { get; set; }
        public double Value { get; set; }

        public int DendriteCount
        {
            get
            {
                return Dendrites.Count;
            }
        }

        public Neuron()
        {
            Random n = new Random(Environment.TickCount);
            this.Bias = n.NextDouble();

            this.Dendrites = new List<Dendrite>();
        }
    }
}

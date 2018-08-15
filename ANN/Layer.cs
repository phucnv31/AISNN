using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    public class Layer
    {
        public List<Neuron> Neurons { get; set; }
        public int NeuronCount
        {
            get
            {
                return Neurons.Count;
            }
        }

        public Layer(int numNeurons)
        {
            Neurons = new List<Neuron>(numNeurons);
        }
    }
}

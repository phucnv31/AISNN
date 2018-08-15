using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    public class Dendrite
    {
        public double Weight { get; set; }

        public Dendrite()
        {
            CryptoRandom n = new CryptoRandom();
            this.Weight = n.RandomValue;
        }
    }
}

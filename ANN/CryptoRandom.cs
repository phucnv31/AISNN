using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ANN
{
    public class CryptoRandom
    {
        public double RandomValue { get; set; }

        public CryptoRandom()
        {
            using (RNGCryptoServiceProvider p = new RNGCryptoServiceProvider())
            {
                Random r = new Random(p.GetHashCode());
                this.RandomValue = r.NextDouble();
            }
        }

    }
}

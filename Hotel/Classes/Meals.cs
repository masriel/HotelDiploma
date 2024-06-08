using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Classes
{
    public class Meals
    {
        private Dictionary<int, double> Prices = new Dictionary<int, double> { { 1, 700.00 }, { 2, 950.00 }, { 3, 800.00 } };

        public int ID { get; set; }

        public string Name { get; set; }

        public double Cost
        {
            get
            {
                if (Prices.ContainsKey(ID))
                {
                    return Prices[ID];
                }
                return 0;
            }
        }

        public int Quantity { get; set; }

    }
}

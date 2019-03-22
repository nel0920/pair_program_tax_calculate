using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Calculator
{
    public class TAX_RATE
    {
        private double tax_rate;
        private int level;
        private int price_stage;

        public TAX_RATE(double tax_rate, int price_stage, int level)
        {
            this.tax_rate = tax_rate;
            this.price_stage = price_stage;
            this.level = level;
        }

        public double Tax_rate { get => tax_rate; set => tax_rate = value; }
        public int Level { get => level; set => level = value; }
        public int Price_stage { get => price_stage; set => price_stage = value; }
    }

    public abstract class TAX_BRACKET
    {
        private HashSet<TAX_RATE> rates;

        protected TAX_BRACKET(int price_stage, double[] rates)
        {
            this.rates = new HashSet<TAX_RATE>();
            for (int i = 1; i <= rates.Length; i++)
            {
                this.rates.Add(new TAX_RATE(rates[i - 1], price_stage, i));
            }
        }

        protected TAX_BRACKET(HashSet<TAX_RATE> rates)
        {
            this.rates = rates;
        }


        public int Get_Total_Level()
        {
            return this.rates.Count;
        }

        public TAX_RATE Get_Tax_Rate_By_Level(int level)
        {
            foreach (TAX_RATE rate in this.rates)
                if (rate.Level == level)
                    return rate;
            return null;
        }
    }

    public class TAX_1819 : TAX_BRACKET
    {
        private const int PRICE_STAGE = 50000;
        private readonly static double[] RATES = new double[] { 0.02, 0.06, 0.1, 0.14, 0.17 };

        public TAX_1819() : base(PRICE_STAGE, RATES) { }

    }

    public class TAX_1718 : TAX_BRACKET
    {
        private const int PRICE_STAGE = 45000;
        private readonly static double[] RATES = new double[] { 0.02, 0.07, 0.12, 0.17 };

        public TAX_1718() : base(PRICE_STAGE, RATES) { }

    }

    public class TAX_1213 : TAX_BRACKET
    {
        private const int PRICE_STAGE = 40000;
        private readonly static double[] RATES = new double[] { 0.02, 0.07, 0.12, 0.17 };

        public TAX_1213() : base(PRICE_STAGE, RATES) { }

    }
}

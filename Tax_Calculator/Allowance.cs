using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Calculator
{
    public class Allowance
    {
        private double allow;

        public Allowance(double allow)
        {
            this.allow = allow;
        }

        public double Get_Allowance()
        {
            return this.allow;
        }
    }

    public class MPF_Allowance : Allowance
    {
        private const int MAX_BASE_MPF = 1500, MAX_MPF_SALARY = 30000, ANNUAL = 12;
        private const double MPF_RATE = 0.05;

        public MPF_Allowance(int income) : base(((Func<double>)(delegate ()
        {
            if (income > (MAX_MPF_SALARY * ANNUAL))
                return MAX_BASE_MPF * ANNUAL;
            else
                return income * MPF_RATE;
        }))())
        {

        }
    }
    public class Basic_Allowance : Allowance
    {
        private const int BASIC_ALLOWANCE = 132000;

        public Basic_Allowance(int income_1) : this(income_1, 2) { }

        public Basic_Allowance(int income_1, int income_2) : base(((Func<double>)(delegate ()
        {
            if (income_1 == 0)
                return 0;
            else if (income_2 == 0)
                return BASIC_ALLOWANCE * 2;
            else
                return BASIC_ALLOWANCE;
        }))())
        {

        }
    }
}

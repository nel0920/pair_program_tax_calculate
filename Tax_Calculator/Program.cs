using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            int income_1 = 0, income_2 = 0, status = TaxCalculator.STATUS_SINGLE;
            try
            {
                if (args[0] != null)
                    income_1 = Convert.ToInt32(args[0]);
            }catch(Exception ex)
            {

            }
            try
            {
                if (args[1] != null)
                {
                    income_2 = Convert.ToInt32(args[1]);
                    status = TaxCalculator.STATUS_MARRIED;
                }
            }
            catch (Exception ex)
            {

            }

            if (income_1 != 0 || income_2 != 0)
            {
                Console.WriteLine(new TaxCalculator(income_1, income_2, status).ToString());
                //Console.ReadLine();
            }

        }
    }

    public class TaxCalculator
    {
        public static int STATUS_SINGLE = 777, STATUS_MARRIED = 888;
       

        int f_income = 0, s_income = 0;
        int r_status = 0;//relationship status

        int f_mpf = 0, s_mpf = 0;
        int f_allowance = 0, s_allowance = 0;

        int f_c_income = 0, s_c_income = 0, t_c_income;
        int f_total = 0; int s_total = 0;

        int[] f_tax , s_tax, t_tax;
        TAX_BRACKET rates;


        public TaxCalculator(int f_income, int s_income, int r_status, TAX_BRACKET rates):this(f_income, s_income, r_status)
        {
            this.rates = rates;
        }

        public TaxCalculator(int f_income, int s_income, int r_status)
        {
            if (this.rates == null)
               this.rates = new TAX_1819();

            this.f_income = f_income;
            this.s_income = s_income;
            this.r_status = r_status;

           // Console.WriteLine(this.f_income.ToString() + ", "  + this.s_income.ToString());

            this.f_mpf = (int)new MPF_Allowance(f_income).Get_Allowance();
            this.s_mpf = (int)new MPF_Allowance(s_income).Get_Allowance();

            //Console.WriteLine(this.f_mpf.ToString() + ", " + this.s_mpf.ToString());

            this.f_allowance = (int)(new Basic_Allowance(this.f_income, this.s_income).Get_Allowance());
            this.s_allowance = (int)(new Basic_Allowance(this.s_income, this.f_income).Get_Allowance());

           // Console.WriteLine(this.f_allowance.ToString() + ", " + this.s_allowance.ToString());


            this.f_c_income = (int)Auto_Zero(this.f_income - this.f_mpf - this.f_allowance);
            this.s_c_income = (int)Auto_Zero(this.s_income - this.s_mpf - this.s_allowance);
            this.t_c_income = (int)Auto_Zero(this.f_c_income + this.s_c_income);

            //Console.WriteLine(this.f_total.ToString() + ", " + this.s_total.ToString());

            this.f_tax = Cal_Tax(f_c_income, this.rates);
            this.s_tax = Cal_Tax(s_c_income, this.rates);
            this.t_tax = Cal_Tax(t_c_income, this.rates);

            // Console.WriteLine(this.f_tax.Sum().ToString() + ", " + this.s_tax.Sum().ToString());




        }

        public override string ToString()
        {
            String str =
                  "                        You     Spouse" + "\n"
                + "Income                 :" + this.f_income.ToString() + ", " + this.s_income.ToString() + "\n"
                + "MPF                    :" + this.f_mpf.ToString() + ", " + this.s_mpf.ToString() + "\n"
                + "Net Income             :" + Auto_Zero(this.f_income - this.f_mpf).ToString() + ", " + Auto_Zero(this.s_income - this.s_mpf).ToString() +  "\n"
                + "Basic Allowance        :" + this.f_allowance.ToString() + ", " + this.s_allowance.ToString() + "\n"
                + "Net Charged Income     :" + this.f_c_income.ToString() + ", " + this.s_c_income.ToString() + ", " + this.t_c_income.ToString() + "\n"
                ;

            for(int i = 1; i<= this.rates.Get_Total_Level();i++)
            {
                TAX_RATE tax = this.rates.Get_Tax_Rate_By_Level(i);

                str = str
                + tax.Price_stage.ToString() + "@ " + (tax.Tax_rate * 100).ToString() + "%              :" + this.f_tax[i - 1].ToString() + ", " + this.s_tax[i - 1].ToString() + ", " + this.t_tax[i - 1].ToString() + "\n"
                ;
            }

            str = str
                + "Total Tax              :" + this.f_tax.Sum().ToString() + ", " + this.s_tax.Sum().ToString() + ", " + this.t_tax.Sum().ToString() + "\n"
                ;

            str +=
                ((this.f_tax.Sum() + this.s_tax.Sum()) > this.t_tax.Sum())
                ? "Based on entered information, you are suggested to pay the tax together and you need to pay $" + this.t_tax.Sum().ToString() + "."
                : "Based on entered information, you are suggested to pay the tax separetely and you need to pay $" + this.f_tax.Sum().ToString() + " and you spouse need to pay $" + this.s_tax.Sum().ToString() + "."
                ;

            return str;
        }


        public int[] Cal_Tax(int income, TAX_BRACKET rates)
        {
            int[] result = new int[rates.Get_Total_Level()];
            int t_income = income;

            for(int i =1; i<=rates.Get_Total_Level();i++)
            {
                if (t_income > 0)
                {
                    TAX_RATE rate = rates.Get_Tax_Rate_By_Level(i);

                    if (i<rates.Get_Total_Level())
                    {
                        result[i - 1] = (int)(rate.Price_stage * rate.Tax_rate);
                        t_income -= rate.Price_stage;
                    }
                    else
                    {
                        result[i - 1] = (int)(t_income * rate.Tax_rate);
                        t_income = 0;
                    }
                }
                else
                {
                    result[i-1] = 0;
                }
            }

            return result;
        }

        public double Auto_Zero<T>(T t ){
            try
            {
                if (Convert.ToDouble(t) <= 0)
                    return 0;
                else return Convert.ToDouble(t);
            }catch(Exception e)
            {
                return 0;
            }

        }
    }

   

   

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator;

namespace Profiler
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string input;
            List<double> array = new List<double>();
            double sum = 0;

            while ((input = Console.ReadLine()) != null && input != "")
            {
                double num = 0;
                if (!Double.TryParse(input, out num))
                {
                    Console.WriteLine("Neplatny vstup.");
                    return;
                }
                array.Add(num);
                sum += num;
            }

            int all = array.Count;

            double average = sum / (all);
            sum = 0;

            foreach (double d in array)
            {
                sum += MathLib.Power(d, 2);
            }


            Console.WriteLine(MathLib.Root((sum - (all * MathLib.Power(average, 2))) / (all - 1), 2));
        }
    }
}

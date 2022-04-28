using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator;

/// <summary>
/// Profiler namespace
/// </summary>
namespace Profiler
{
    /// <summary>
    /// Console application used to calculate the sample standard deviation
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The entry point of the application where the reading and calculation happens
        /// </summary>
        /// <param name="args">Arguments passed in command line</param>
        static void Main(string[] args)
        {
            string input;
            List<double> array = new List<double>();
            double sum = 0;

            while ((input = Console.ReadLine()) != null && input != "")
            {
                double num = 0;
                string[] stringNumbers = input.Split(new char[] { ' ', '\r', '\n', '\t' });
                foreach(string s in stringNumbers)
                {
                    if(!double.TryParse(s, out num))
                    {
                        Console.WriteLine("Neplatny vstup.");
                        return;
                    }

                    array.Add(num);
                    sum += num;
                }
            }

            int all = array.Count;

            double average = sum / all;
            sum = 0;

            foreach (double d in array)
            {
                sum += MathLib.Power(d, 2);
            }

            Console.WriteLine(MathLib.Root((sum - (all * MathLib.Power(average, 2))) / (all - 1), 2));
        }
    }
}

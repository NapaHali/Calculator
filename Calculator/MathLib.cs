using System;
using System.Numerics;
using System.Collections.Generic;

namespace Calculator
{
    public static class MathLib
    {

        public static double Add(double x, double y)
        {
            return x+y;
        }

        public static double Substract(double x, double y)
        {
            return x-y;
        }

        public static double Multiply(double x, double y)
        {
            return x*y;
        }

        public static double Divide(double x, double y)
        {
            if (y == 0)
                throw new System.DivideByZeroException();
            return x/y;
        }

        public static BigInteger Factorial(int n)
        {
            // int exponent = num == 0 ? 0 : (int)Math.Floor((Math.Log10(Math.Abs(num))));
            if(n == 0)
            {
                throw new ArithmeticException();
            }
            else if (n > 3248)
            {
                throw new OverflowException();
            }

            if(n == 1)
            {
                return BigInteger.One;
            }

            BigInteger result = 1;
            int count = n;
            while(count != 1)
            {
                result *= count;
                count--;
            }


            return result;
        }

        public static double Power(double x, int exponent)
        {
            double result = 1;
            if (exponent < 0)
            {
                for (int i = 0; i > exponent; i--)
                {
                    result /= x;
                }
            } else if (exponent > 0)
            {
                for (int i = 0; i < exponent; i++)
                {
                    result *= x;
                }
            }
            return result;
        }

        public static double Root(double x, int exponent)
        {
            if (x == 0 || x == 1) return x;
            if (exponent == 0) return double.NaN;

            int negativeSwitch = 1;

            if (x < 0 && exponent % 2 != 0)
            {
                negativeSwitch = -1;
                x = -x;
            }

            if (x < 0 && exponent % 2 == 0)
            {
                return double.NaN;
            }

            int precision = 100;
            double result = 1;
            for (int i = 0; i < precision; i++)
            {
                result = result - (Power(result, exponent) - x) / (exponent * Power(result, exponent - 1));
            }
            return result * negativeSwitch;
        }

        public static double Abs(double x)
        {
            return x >= 0 ? x : -x;
        }

        public static string ToScientificNotation(BigInteger num)
        {
            string result = num.ToString();
            short exponent = (short)(result.Length - 1);

            result = result.Insert(1, ".");
            result = result.Substring(0, 12); // Why 12? Because the result will be rounded to 10 decimal numbers (x.0000000000)
            result = result.Insert(result.Length, "E+" + exponent);

            return result;
        }
    }
}
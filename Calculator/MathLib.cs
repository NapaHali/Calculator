using System;
using System.Numerics;
using System.Collections.Generic;

/// <summary>
/// Main namespace of Calculator
/// </summary>
namespace Calculator
{
    /// <summary>
    /// Mathematical library used to perform mathematical operations for this project
    /// </summary>
    public static class MathLib
    {
        /// <summary>
        /// Returns sum of two numbers
        /// </summary>
        /// <param name="x">First value</param>
        /// <param name="y">Second value</param>
        /// <returns>double</returns>
        public static double Add(double x, double y)
        {
            return x+y;
        }

        /// <summary>
        /// Returns difference of two numbers
        /// </summary>
        /// <param name="x">First value</param>
        /// <param name="y">Second value</param>
        /// <returns>double</returns>
        public static double Substract(double x, double y)
        {
            return x-y;
        }

        /// <summary>
        /// Returns multiplication of two numbers
        /// </summary>
        /// <param name="x">First value</param>
        /// <param name="y">Second value</param>
        /// <returns>double</returns>
        public static double Multiply(double x, double y)
        {
            return x*y;
        }

        /// <summary>
        /// Returns division of two numbers
        /// </summary>
        /// <param name="x">First value</param>
        /// <param name="y">Second value</param>
        /// <returns>double</returns>
        /// <exception cref="System.DivideByZeroException">If divisor is equal to 0</exception>
        public static double Divide(double x, double y)
        {
            if (y == 0)
                throw new System.DivideByZeroException();
            return x/y;
        }

        /// <summary>
        /// Returns factorial of specified whole natural number
        /// </summary>
        /// <param name="n">Whole natural number</param>
        /// <returns>BigInteger</returns>
        /// <exception cref="System.ArithmeticException">If specified number is 0</exception>
        /// <exception cref="System.OverflowException">If specified number is greater than 3248, which results in RAX+RDX (128-byte) registers overflow</exception>
        public static BigInteger Factorial(int n)
        {
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

        /// <summary>
        /// Returns power of a number with whole exponent
        /// </summary>
        /// <param name="x">Number to be powered</param>
        /// <param name="exponent">Whole number as an exponent</param>
        /// <returns>double</returns>
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

        /// <summary>
        /// Returns general root with whole exponent of specified number
        /// </summary>
        /// <param name="x">Number</param>
        /// <param name="exponent">Whole number as an exponent</param>
        /// <returns>double</returns>
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

        /// <summary>
        /// Returns absolute value of a number
        /// </summary>
        /// <param name="x">Numeric value</param>
        /// <returns>double</returns>
        public static double Abs(double x)
        {
            return x >= 0 ? x : -x;
        }

        /// <summary>
        /// Transforms long illegible number to a number with scientific notation
        /// </summary>
        /// <param name="num">A number to be transformed</param>
        /// <returns>Number in string format (with 10 decimal places) with scientific notation</returns>
        public static string ToScientificNotation(BigInteger num)
        {
            string result = num.ToString();
            short exponent = (short)(result.Length - 1);

            result = result.Insert(1, ".");
            result = result.Substring(0, 12); // Why 12? Because the result will be rounded to 10 decimal numbers (x.0000000000)
            result = result.Insert(result.Length, "E+" + exponent);

            return result;
        }

        /// <summary>
        /// Transforms number (typeof double) to a number with scientific notation
        /// </summary>
        /// <param name="num">A number to be transformed</param>
        /// <returns>Number in string format (with 10 decimal places) with scientific notation</returns>
        public static string ToScientificNotation(double num)
        {
            string result = num.ToString();
            short exponent = (short)(result.Length - 1);

            result = result.Insert(1, ".");
            result = result.Substring(0, 12);
            result = result.Insert(result.Length, "E+" + exponent);

            return result;
        }

        /// <summary>
        /// Transforms number (typeof float) to a number with scientific notation
        /// </summary>
        /// <param name="num">A number to be transformed</param>
        /// <returns>Number in string format (with 2 decimal places) with scientific notation</returns>
        public static string ToScientificNotation(float num)
        {
            string result = num.ToString();
            short exponent = (short)(result.Length - 1);

            result = result.Insert(1, ".");
            result = result.Substring(0, 4); // Why 4? Because the input is in float, which does not have to have long decimal place count, so the result will be rounded to 2 decimal numbers (x.00)
            result = result.Insert(result.Length, "E+" + exponent);

            return result;
        }
    }
}
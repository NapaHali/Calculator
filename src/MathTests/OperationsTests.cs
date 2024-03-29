﻿using Calculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

/// <summary>
/// NUnit tests for MathLib operations
/// </summary>
namespace MathTests
{
    /// <summary>
    /// OperationsTests class initialized by NUnit tester
    /// </summary>
    [TestClass]
    public class OperationsTests
    {
        /// <summary>
        /// Decomposes number in string format that uses scientific notation
        /// </summary>
        /// <param name="value">The number with scientific notation in string format</param>
        /// <returns>Number decomposed to base and exponent after the scientific notation</returns>
        public static (decimal multiplier, int exponent) Decompose(string value)
        {
            var split = value.Split('e');
            return (decimal.Parse(split[0]), int.Parse(split[1]));
        }

        /// <summary>
        /// Returns the number of decimal places in specified number
        /// </summary>
        /// <param name="value">The number in which the count of decimal places will be calculated</param>
        /// <returns>Number of decimal places</returns>
        public static int GetDecimalPlaces(decimal value)
           => BitConverter.GetBytes(decimal.GetBits(value)[3])[2];

        /// <summary>
        /// Decomposes number in string format that uses scientific notation and returns it in BigInteger format
        /// </summary>
        /// <param name="value">The number in string format</param>
        /// <returns>BigInteger number without scientific notation</returns>
        public static BigInteger ParseExtended(string value)
        {
            var (multiplier, exponent) = Decompose(value);

            var decimalPlaces = GetDecimalPlaces(multiplier);
            var power = (int)Math.Pow(10, decimalPlaces);

            return (BigInteger.Pow(10, exponent) * (BigInteger)(multiplier * power)) / power;
        }

        /// <summary>
        /// TestMethod that tests the MathLib capability to evaluate addition
        /// </summary>
        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(2, MathLib.Add(1, 1));
            Assert.AreEqual(420.69, MathLib.Add(420, 0.69));
            Assert.AreEqual(-1337.000001, MathLib.Add(-4200.987654, 2863.987653), 0.0000001);
            Assert.AreEqual(0, MathLib.Add(69, -69));
            Assert.AreEqual(0, MathLib.Add(-69, 69));

            //Assert.ThrowsException<OverflowException>(() => MathLib.Add(long.MaxValue, long.MaxValue), "Overflow occured!");
        }

        /// <summary>
        /// TestMethod that tests the MathLib capability to evaluate substraction
        /// </summary>
        [TestMethod]
        public void Substract()
        {
            Assert.AreEqual(2, MathLib.Substract(4, 2));
            Assert.AreEqual(-2, MathLib.Substract(2, 4));
            Assert.AreEqual(6, MathLib.Substract(2, -4));

            Assert.AreEqual(420.99, MathLib.Substract(1024.1234, 603.1334), 0.001);
            Assert.AreNotEqual(420.99, MathLib.Substract(1024.1234, -603.1334), 0.001);
            Assert.AreEqual(1337.000001, MathLib.Substract(4200.987654, 2863.987653), 0.0000001);

            Assert.AreEqual(0, MathLib.Substract(420, 420));
            Assert.AreNotEqual(0, MathLib.Substract(420, -420));
            Assert.AreEqual(0, MathLib.Substract(long.MaxValue, long.MaxValue));

            //Assert.ThrowsException<OverflowException>(() => MathLib.Substract(long.MinValue, long.MaxValue), "Overflow occured!");
        }

        /// <summary>
        /// TestMethod that tests the MathLib capability to evaluate multiplication
        /// </summary>
        [TestMethod]
        public void Multiply()
        {
            Assert.AreEqual(4, MathLib.Multiply(2, 2));
            Assert.AreEqual(-4, MathLib.Multiply(2, -2));
            Assert.AreEqual(-4, MathLib.Multiply(-2, 2));

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(0, MathLib.Multiply(i, 0));
            }

            Assert.AreEqual(1073676289, MathLib.Multiply(short.MaxValue, short.MaxValue));
            Assert.AreEqual(1073741824, MathLib.Multiply(short.MinValue, short.MinValue));
            Assert.AreEqual(-1073709056, MathLib.Multiply(short.MinValue, short.MaxValue));

            Assert.AreEqual(314.7711, MathLib.Multiply(13.11, 24.01), 0.00001);
            Assert.AreEqual(1789053.191401, MathLib.Multiply(1337.123, 1337.987), 0.0000001);
            Assert.AreNotEqual(-1, MathLib.Multiply(-1, -1));
        }

        /// <summary>
        /// TestMethod that tests the MathLib capability to evaluate division
        /// </summary>
        [TestMethod]
        public void Divide()
        {
            Assert.AreEqual(1, MathLib.Divide(2, 2));
            Assert.AreEqual(-1, MathLib.Divide(-2, 2));
            Assert.AreEqual(-1, MathLib.Divide(2, -2));

            for (int i = 0; i < 100; i*=2)
            {
                Assert.ThrowsException<DivideByZeroException>(() => MathLib.Divide(i, 0), "You can not divide by zero!");
                i++;
            }

            Assert.AreEqual(0.5, MathLib.Divide(1, 2), 0.01);
            Assert.AreEqual(0.333, MathLib.Divide(1.25, 3.75), 0.0004);
            Assert.AreEqual(1.1586715867, MathLib.Divide(3.14, 2.71), 0.00000000009);
            Assert.AreEqual(-95.5, MathLib.Divide(1337, -14), 0.01);
            Assert.AreEqual(-2.5, MathLib.Divide(0.25, -0.1), 0.01);
        }

        /// <summary>
        /// TestMethod that tests the MathLib capability to evaluate factorial function
        /// </summary>
        [TestMethod]
        public void Factorial()
        {
            Assert.AreNotEqual(5, MathLib.Factorial(3));
          
            BigInteger[] factorials = { 0, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800, 479001600, 6227020800, 87178291200, 1307674368000, 
                20922789888000, 355687428096000, 6402373705728000, 121645100408832000, 2432902008176640000, BigInteger.Parse("51090942171709440000"),
                BigInteger.Parse("1124000727777607680000"), BigInteger.Parse("25852016738884976640000"), BigInteger.Parse("620448401733239439360000"),
                BigInteger.Parse("15511210043330985984000000"), BigInteger.Parse("403291461126605635584000000"), BigInteger.Parse("10888869450418352160768000000"),
                BigInteger.Parse("304888344611713860501504000000"), BigInteger.Parse("8841761993739701954543616000000") };

            for (int i = 0; i < 30; i++)
            {
                if(i == 0)
                {
                    Assert.ThrowsException<ArithmeticException>(() => MathLib.Factorial(i), "Arithmetic exception occured!");
                } else
                {
                    Assert.AreEqual(factorials[i], MathLib.Factorial(i));
                }
            }

            Assert.AreEqual("4.0238726007E+2567", MathLib.ToScientificNotation(MathLib.Factorial(1000)));
            Assert.AreEqual("3.3162750924E+5735", MathLib.ToScientificNotation(MathLib.Factorial(2000)));
            Assert.AreEqual("4.1493596034E+9130", MathLib.ToScientificNotation(MathLib.Factorial(3000)));
            Assert.AreEqual("1.9736342530E+9997", MathLib.ToScientificNotation(MathLib.Factorial(3248)));

            Assert.ThrowsException<OverflowException>(() => MathLib.Factorial(3249), "Overflow occured!");
        }

        /// <summary>
        /// TestMethod that tests the MathLib capability to evaluate power function
        /// </summary>
        [TestMethod]
        public void Power()
        {
            Assert.AreEqual(8, MathLib.Power(2, 3));
            Assert.AreEqual(-8, MathLib.Power(-2, 3));
            Assert.AreEqual(4, MathLib.Power(-2, 2));
            Assert.AreEqual(2389.979753, MathLib.Power(13.37, 3), 0.0000001);
            Assert.AreEqual(51461730813285.3, MathLib.Power(4.2, 22), 0.01);
            Assert.AreEqual(1, MathLib.Power(0, 0));
            Assert.AreEqual(0, MathLib.Power(0, 1));

            for (int i = -50; i < 50; i++)
            {
                Assert.AreEqual(1, MathLib.Power(i, 0));
            }
        }

        /// <summary>
        /// TestMethod that tests the MathLib capability to evaluate root function
        /// </summary>
        [TestMethod]
        public void Root()
        {
            Assert.AreEqual(2, MathLib.Root(4, 2));
            Assert.AreEqual(2, MathLib.Root(8, 3));
            Assert.AreEqual(-1.2599210498, MathLib.Root(-2, 3), 0.000000000099);
            Assert.AreEqual(-1.5157165665, MathLib.Root(-8, 5), 0.00000000002);

            for (int i = 0; i < 8; i++)
            {
                if(i % 2 == 0)
                {
                    Assert.AreEqual(double.NaN, MathLib.Root(-1, i));
                } else
                {
                    Assert.AreEqual(-1, MathLib.Root(-1, i));
                }
            }
        }

        /// <summary>
        /// TestMethod that tests the MathLib capability to evaluate absolute value of a number
        /// </summary>
        [TestMethod]
        public void Abs()
        {
            Assert.AreEqual(69, MathLib.Abs(69));
            Assert.AreEqual(420, MathLib.Abs(-420));
            Assert.AreEqual(1337, MathLib.Abs(-(-1337)));

            for (int i = 1; i < 50; i++)
            {
                Assert.AreNotEqual(-i, MathLib.Abs(i));
            }

            for (int i = -50; i < 0; i++)
            {
                Assert.AreEqual(-i, MathLib.Abs(i));
            }
          
            for (int i = 0; i < 50; i++)
            {
                Assert.AreEqual(i, MathLib.Abs(i));
            }
        }
    }
}

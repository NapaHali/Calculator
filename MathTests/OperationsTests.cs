using Calculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace MathTests
{
    [TestClass]
    public class OperationsTests
    {
        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(2, MathLib.Add(1, 1));

            Assert.AreEqual(420.69, MathLib.Add(420, 0.69));
            Assert.AreEqual(-1337.00000000099, MathLib.Add(-420000.987654321, 418663.98765432));
            Assert.AreEqual(-1337.00000000099, MathLib.Add(-420000.987654321, 418663.987654320));
            Assert.AreNotEqual(-1337.00000000099, MathLib.Add(-420000.987654321, 418663.987654301));

            Assert.ThrowsException<OverflowException>(() => MathLib.Add(long.MaxValue, long.MaxValue), "Overflow occured!");

            Assert.AreEqual(0, MathLib.Add(69, -69));
            Assert.AreEqual(0, MathLib.Add(-69, 69));
        }

        [TestMethod]
        public void Substract()
        {
            Assert.AreEqual(2, MathLib.Substract(4, 2));
            Assert.AreEqual(-2, MathLib.Substract(2, 4));
            Assert.AreEqual(6, MathLib.Substract(2, -4));

            Assert.AreEqual(420.69, MathLib.Substract(1024.1234, 603.4334));
            Assert.AreNotEqual(420.69, MathLib.Substract(1024.1234, 603.4434));
            Assert.AreEqual(-1337.000000001, MathLib.Substract(69420.123456789, 68083.123456788));

            Assert.AreEqual(0, MathLib.Substract(long.MaxValue, long.MaxValue));
            Assert.ThrowsException<OverflowException>(() => MathLib.Substract(long.MinValue, long.MaxValue), "Overflow occured!");

            Assert.AreEqual(0, MathLib.Substract(420, 420));
            Assert.AreNotEqual(0, MathLib.Substract(420, -420));
        }

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

            Assert.AreEqual(1073741824, MathLib.Multiply(short.MaxValue, short.MaxValue));
            Assert.AreEqual(1073741824, MathLib.Multiply(short.MinValue, short.MinValue));
            Assert.AreEqual(-1073741824, MathLib.Multiply(short.MinValue, short.MaxValue));

            Assert.AreEqual(29299.0698, MathLib.Multiply(69.69, 420.420));
            Assert.AreEqual(-1789054.677486701112635269, MathLib.Multiply(1337.123456789, -1337.987654321));
            Assert.AreNotEqual(-1, MathLib.Multiply(-1, -1));
        }

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

            Assert.AreEqual(0.16501461884047636026527847108322, MathLib.Divide(69.42, 420.69));
            Assert.AreEqual(6.0600691443388072601555747623163, MathLib.Divide(420.69, 69.42));
            Assert.AreEqual(-6.0600691443388072601555747623163, MathLib.Divide(420.69, -69.42));
            Assert.AreNotEqual(6.0600691443388072601555747623163, MathLib.Divide(-420.69, 69.42));
        }

        [TestMethod]
        public void Factorial()
        {
            BigInteger[] factorials = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800, 479001600, 6227020800, 87178291200, 1307674368000, 
                20922789888000, 355687428096000, 6402373705728000, 121645100408832000, 2432902008176640000, BigInteger.Parse("51090942171709440000"),
                BigInteger.Parse("1124000727777607680000"), BigInteger.Parse("25852016738884976640000"), BigInteger.Parse("620448401733239439360000"),
                BigInteger.Parse("15511210043330985984000000"), BigInteger.Parse("403291461126605635584000000"), BigInteger.Parse("10888869450418352160768000000"),
                BigInteger.Parse("304888344611713860501504000000"), BigInteger.Parse("8841761993739701954543616000000") };

            for (int i = 0; i < 30; i++)
            {
                Assert.AreEqual(factorials[i], MathLib.Factorial(i));
            }

            Assert.AreEqual(BigInteger.Parse("4.02387260077093773543702433923E+2567"), MathLib.Factorial(1000));
            Assert.AreEqual(BigInteger.Parse("3.3162750924506332411753933805763E+5735"), MathLib.Factorial(2000));
            Assert.AreEqual(BigInteger.Parse("4.1493596034378540855568670930866E+9130"), MathLib.Factorial(3000));
            Assert.AreEqual(BigInteger.Parse("1.9736342530860425312047080034031E+9997"), MathLib.Factorial(3248));
            Assert.AreNotEqual(5, MathLib.Factorial(3));

            Assert.ThrowsException<OverflowException>(() => MathLib.Factorial(3249), "Overflow occured!");
        }

        [TestMethod]
        public void Power()
        {
            Assert.AreEqual(8, MathLib.Power(2, 3));
            Assert.AreEqual(-8, MathLib.Power(-2, 3));
            Assert.AreEqual(4, MathLib.Power(-2, 2));
            Assert.AreEqual(5712003.219749941009, MathLib.Power(13.37, 6));
            Assert.AreEqual(406671.383849472, MathLib.Power(4.2, 9));
            Assert.AreEqual(1, MathLib.Power(0, 0));
            Assert.AreEqual(0, MathLib.Power(0, 1));

            for (int i = -50; i < 50; i++)
            {
                if(i < 0)
                {
                    Assert.AreEqual(-1, MathLib.Power(i, 0));
                } else
                {
                    Assert.AreEqual(1, MathLib.Power(i, 0));
                }
            }
        }

        [TestMethod]
        public void Root()
        {
            Assert.AreEqual(2, MathLib.Root(4, 2));
            Assert.AreEqual(2, MathLib.Root(8, 3));
            Assert.AreEqual(-1.2599210498949, MathLib.Root(-2, 3));
            Assert.AreEqual(-1.5157165665104, MathLib.Root(-8, 5));
            for (int i = 0; i < 8; i++)
            {
                if(i % 2 == 0)
                {
                    Assert.ThrowsException<ArithmeticException>(() => MathLib.Root(-1, i), "Invalid root!");
                } else
                {
                    Assert.AreEqual(-1, MathLib.Root(-1, i));
                }
            }
        }

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

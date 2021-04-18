using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoronoiEngine.Utilities;

namespace VoronoiTests
{
    [TestFixture]
    public class IntExtensionsTest
    {
        [TestCase(10, 5, 2)]
        [TestCase(16, 4, 4)]
        [TestCase(9, 3, 3)]
        [TestCase(36, 6, 6)]
        [TestCase(32, 8, 4)]
        [TestCase(7, 7, 1)]
        [TestCase(0, 1, 0)]
        [TestCase(8, 4, 2)]
        public void TestGetGreatestDivisors(int number, int divisorA, int divisorB)
        {
            var result = number.GetGreatestDivisors();

            Assert.AreEqual(divisorA, result.Item1);
            Assert.AreEqual(divisorB, result.Item2);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shipwreck.Math;

namespace Shipwreck.Test
{
    [TestClass]
    public class Vec2Test
    {
        [TestMethod]
        public void TestCreate()
        {
            var vec = new Vec2(1, 2);

            Assert.AreEqual(1f, vec.X, 1e-5f);
            Assert.AreEqual(2f, vec.Y, 1e-5f);
        }

        [TestMethod]
        public void TestLengthSquared()
        {
            var vec = new Vec2(3, 4);

            Assert.AreEqual(25f, vec.LengthSquared, 1e-5f);
        }

        [TestMethod]
        public void TestLength()
        {
            var vec = new Vec2(3, 4);

            Assert.AreEqual(5f, vec.Length, 1e-5f);
        }

        [TestMethod]
        public void TestNormalized()
        {
            var vec = new Vec2(3, 4);
            var vecNormalized = vec.Normalized;

            Assert.AreEqual(1f, vecNormalized.Length, 1e-5f);
            Assert.AreEqual(0.6f, vecNormalized.X, 1e-5f);
            Assert.AreEqual(0.8f, vecNormalized.Y, 1e-5f);
        }

        [TestMethod]
        public void TestAdd()
        {
            var vec = new Vec2(1, 2) + new Vec2(4, 8);

            Assert.AreEqual(5f, vec.X, 1e-5f);
            Assert.AreEqual(10f, vec.Y, 1e-5f);
        }

        [TestMethod]
        public void TestSub()
        {
            var vec = new Vec2(5, 10) - new Vec2(1, 2);

            Assert.AreEqual(4f, vec.X, 1e-5f);
            Assert.AreEqual(8f, vec.Y, 1e-5f);
        }

        [TestMethod]
        public void TestScale()
        {
            var vec = new Vec2(1, 2) * 3;

            Assert.AreEqual(3f, vec.X, 1e-5f);
            Assert.AreEqual(6f, vec.Y, 1e-5f);
        }
    }
}

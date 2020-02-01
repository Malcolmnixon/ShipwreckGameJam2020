using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shipwreck.Math;

namespace Shipwreck.Test
{
    [TestClass]
    public class Vec3Test
    {
        [TestMethod]
        public void TestCreate()
        {
            var vec = new Vec3(1, 2, 3);

            Assert.AreEqual(1f, vec.X, 1e-5f);
            Assert.AreEqual(2f, vec.Y, 1e-5f);
            Assert.AreEqual(3f, vec.Z, 1e-5f);
        }

        [TestMethod]
        public void TestLengthSquared()
        {
            var vec = new Vec3(3,4, 5);

            Assert.AreEqual(50f, vec.LengthSquared, 1e-5f);
        }

        [TestMethod]
        public void TestLength()
        {
            var vec = new Vec3(3, 4, 5);

            Assert.AreEqual(7.071068f, vec.Length, 1e-5f);
        }

        [TestMethod]
        public void TestNormalized()
        {
            var vec = new Vec3(3, 4, 5);
            var vecNormalized = vec.Normalized;

            Assert.AreEqual(1f, vecNormalized.Length, 1e-5f);
            Assert.AreEqual(0.4242641f, vecNormalized.X, 1e-5f);
            Assert.AreEqual(0.5656854f, vecNormalized.Y, 1e-5f);
            Assert.AreEqual(0.7071068f, vecNormalized.Z, 1e-5f);
        }

        [TestMethod]
        public void TestAdd()
        {
            var vec = new Vec3(1, 2, 4) + new Vec3(8, 16, 32);

            Assert.AreEqual(9f, vec.X, 1e-5f);
            Assert.AreEqual(18f, vec.Y, 1e-5f);
            Assert.AreEqual(36f, vec.Z, 1e-5f);
        }

        [TestMethod]
        public void TestSub()
        {
            var vec = new Vec3(9, 18, 36) - new Vec3(1, 2, 4);

            Assert.AreEqual(8f, vec.X, 1e-5f);
            Assert.AreEqual(16f, vec.Y, 1e-5f);
            Assert.AreEqual(32f, vec.Z, 1e-5f);
        }

        [TestMethod]
        public void TestScale()
        {
            var vec = new Vec3(1, 2, 3) * 3;

            Assert.AreEqual(3f, vec.X, 1e-5f);
            Assert.AreEqual(6f, vec.Y, 1e-5f);
            Assert.AreEqual(9f, vec.Z, 1e-5f);
        }
    }
}

using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shipwreck.World;

namespace Shipwreck.Test
{
    [TestClass]
    public class LocalWorldTest
    {
        [TestMethod]
        public void CreateLocal()
        {
            // Create a local world
            using var world = new LocalWorld();

            // Start the world
            world.Start();

            // Wait 1 second
            Thread.Sleep(1000);

            // Verify state
            Assert.IsNull(world.LocalPlayer);
            Assert.IsNull(world.LocalAstronaut);
            Assert.AreEqual(0, world.Players.Players.Count);
            Assert.AreEqual(0, world.State.Astronauts.Count);
            Assert.AreEqual(0, world.State.Asteroids.Count);
        }

        [TestMethod]
        public void LocalPlayer()
        {
            // Create a local world
            using var world = new LocalWorld();

            // Start the world
            world.Start();

            // Create the local player
            var localPlayer = world.CreateLocalPlayer("Test");

            // Wait 1 second
            Thread.Sleep(1000);

            // Verify state
            Assert.IsNotNull(world.LocalPlayer);
            Assert.IsNotNull(world.LocalAstronaut);
            Assert.AreEqual(1, world.Players.Players.Count);
            Assert.AreEqual(1, world.State.Astronauts.Count);
            Assert.AreEqual(0, world.State.Asteroids.Count);
        }
    }
}

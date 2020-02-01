using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shipwreck.World;
using Shipwreck.WorldData;
using Shipwreck.WorldNet;

namespace Shipwreck.Test
{
    [TestClass]
    public class NetWorldTest
    {
        [TestMethod]
        public void Connect()
        {
            // Create server
            using var server = new NetServerWorld();
            server.Start();

            // Build client on loop-back
            using var client = new NetClientWorld(IPAddress.Loopback);
            client.Start();
        }

        [TestMethod]
        public void Synchronize()
        {
            // Create server
            using var server = new NetServerWorld();
            server.Start();

            // Build client 1 on loop-back
            using var client1 = new NetClientWorld(IPAddress.Loopback);
            client1.Start();
            client1.CreateLocalPlayer("Client 1 Player");

            // Build client 2 on loop-back
            using var client2 = new NetClientWorld(IPAddress.Loopback);
            client2.Start();
            client2.CreateLocalPlayer("Client 2 Player");

            // Wait for updates
            Thread.Sleep(5000);

            // Ensure game is waiting
            Assert.IsNotNull(client1.LocalPlayer);
            Assert.IsNotNull(client2.LocalPlayer);
            Assert.IsNotNull(client1.LocalAstronaut);
            Assert.IsNotNull(client2.LocalAstronaut);
            Assert.AreEqual(2, server.Players.Players.Count);
            Assert.AreEqual(2, client1.Players.Players.Count);
            Assert.AreEqual(2, client2.Players.Players.Count);
            Assert.AreEqual(2, server.State.Astronauts.Count);
            Assert.AreEqual(2, client1.State.Astronauts.Count);
            Assert.AreEqual(2, client2.State.Astronauts.Count);
            Assert.AreEqual(GameMode.Waiting, server.State.Mode);
            Assert.AreEqual(GameMode.Waiting, client1.State.Mode);
            Assert.AreEqual(GameMode.Waiting, client2.State.Mode);
            Assert.AreEqual(1, server.Players.Players.Count(p => p.Name == "Client 1 Player"));
            Assert.AreEqual(1, client1.Players.Players.Count(p => p.Name == "Client 1 Player"));
            Assert.AreEqual(1, client2.Players.Players.Count(p => p.Name == "Client 1 Player"));
            Assert.AreEqual(1, server.Players.Players.Count(p => p.Name == "Client 2 Player"));
            Assert.AreEqual(1, client1.Players.Players.Count(p => p.Name == "Client 2 Player"));
            Assert.AreEqual(1, client2.Players.Players.Count(p => p.Name == "Client 2 Player"));

            // Wait for game to start
            Thread.Sleep((int)(GameConstants.PlayerWaitTime * 1000));

            // Ensure game has started
            Assert.AreEqual(GameMode.Playing, server.State.Mode);
            Assert.AreEqual(GameMode.Playing, client1.State.Mode);
            Assert.AreEqual(GameMode.Playing, client2.State.Mode);
        }
    }
}

using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shipwreck.WorldLan;

namespace Shipwreck.Test
{
    [TestClass]
    public class LanWorldTest
    {
        [TestMethod]
        public void DiscoverAndConnect()
        {
            // Create a server
            using var server = new LanServerWorld("Unit Test World");
            server.Start();

            // Create discovery
            using var discovery = new LanDiscovery();
            discovery.Start();

            // Sleep 5 seconds for discovery
            Thread.Sleep(5000);

            // Get the servers
            var servers = discovery.GameServers;
            Assert.IsTrue(servers.ContainsKey("Unit Test World"));

            // Connect to server
            using var client = new LanClientWorld(servers["Unit Test World"]);
            client.Start();
        }
    }
}

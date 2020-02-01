using Shipwreck.WorldNet;

namespace Shipwreck.WorldLan
{
    /// <summary>
    /// Lan-server world
    /// </summary>
    public class LanServerWorld : NetServerWorld
    {
        /// <summary>
        /// Discovery provider
        /// </summary>
        private readonly NetDiscovery.IProvider _discoveryProvider;

        /// <summary>
        /// Discovery server
        /// </summary>
        private readonly NetDiscovery.IServer _discoveryServer;

        /// <summary>
        /// Initializes a new instance of the LanServerWorld class
        /// </summary>
        /// <param name="name">Lan game name</param>
        public LanServerWorld(string name)
        {
            _discoveryProvider = new NetDiscovery.Udp.UdpProvider(LanConstants.DiscoveryPort);
            _discoveryServer = _discoveryProvider.CreateServer();
            _discoveryServer.Identity = name;
        }

        public override void Dispose()
        {
            // Dispose of the world
            base.Dispose();

            // Stop network communications
            _discoveryProvider.Dispose();
            _discoveryServer.Dispose();
        }

        public override void Start()
        {
            // Start the world
            base.Start();

            // Start networking
            _discoveryServer.Start();
            _discoveryProvider.Start();
        }
    }
}

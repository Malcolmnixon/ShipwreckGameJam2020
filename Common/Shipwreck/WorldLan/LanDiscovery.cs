using System;
using System.Collections.Generic;
using System.Net;

namespace Shipwreck.WorldLan
{
    /// <summary>
    /// Lan-discovery class
    /// </summary>
    public class LanDiscovery : IDisposable
    {
        /// <summary>
        /// Discovery provider
        /// </summary>
        private readonly NetDiscovery.IProvider _discoveryProvider;

        /// <summary>
        /// Discovery client
        /// </summary>
        private readonly NetDiscovery.IClient _discoveryClient;

        /// <summary>
        /// Dictionary of servers
        /// </summary>
        private readonly Dictionary<string, IPAddress> _servers = new Dictionary<string, IPAddress>();

        /// <summary>
        /// Initializes a new instance of the LanDiscovery class
        /// </summary>
        public LanDiscovery()
        {
            _discoveryProvider = new NetDiscovery.Udp.UdpProvider(LanConstants.DiscoveryPort);
            _discoveryClient = _discoveryProvider.CreateClient();
            _discoveryClient.Discovery += DiscoveryClientOnDiscovery;
        }

        /// <summary>
        /// Gets a dictionary of game servers
        /// </summary>
        public Dictionary<string, IPAddress> GameServers
        {
            get
            {
                lock (_servers)
                {
                    return new Dictionary<string, IPAddress>(_servers);
                }
            }
        }

        public void Start()
        {
            _discoveryClient.Start();
            _discoveryProvider.Start();
        }

        public void Dispose()
        {
            _discoveryClient.Dispose();
            _discoveryProvider.Dispose();
        }

        private void DiscoveryClientOnDiscovery(object sender, NetDiscovery.DiscoveryEventArgs e)
        {
            lock (_servers)
            {
                _servers[e.Identity] = e.Address;
            }
        }
    }
}

using System.Net;
using Shipwreck.WorldNet;

namespace Shipwreck.WorldLan
{
    /// <summary>
    /// Lan-client world
    /// </summary>
    public class LanClientWorld : NetClientWorld
    {
        public LanClientWorld(IPAddress server) : base(server)
        {
        }
    }
}

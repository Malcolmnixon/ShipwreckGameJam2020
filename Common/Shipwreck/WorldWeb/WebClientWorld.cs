using System.Net;
using Shipwreck.WorldNet;

namespace Shipwreck.WorldWeb
{
    /// <summary>
    /// Web-client world
    /// </summary>
    public class WebClientWorld : NetClientWorld
    {
        public WebClientWorld() : base(IPAddress.Parse("204.48.22.8"))
        {
        }
    }
}

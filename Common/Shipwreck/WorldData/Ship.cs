using System;
using Newtonsoft.Json;
using Shipwreck.Math;

namespace Shipwreck.WorldData
{
    /// <summary>
    /// Ship status
    /// </summary>
    public class Ship
    {
        /// <summary>
        /// GUID for ship pilot (or Guid.None if no pilot)
        /// </summary>
        public Guid Pilot { get; set; }

        /// <summary>
        /// Wing 1 health (0..100)
        /// </summary>
        public float Wing1Health { get; set; }

        /// <summary>
        /// Wing 2 health (0..100)
        /// </summary>
        public float Wing2Health { get; set; }

        /// <summary>
        /// Wing 3 health (0..100)
        /// </summary>
        public float Wing3Health { get; set; }

        /// <summary>
        /// Gets a value indicating whether the ship is shielded
        /// </summary>
        public bool Shielded { get; set; }

        /// <summary>
        /// Total health of ship
        /// </summary>
        [JsonIgnore]
        public float TotalHealth => Wing1Health + Wing2Health + Wing3Health;
    }
}

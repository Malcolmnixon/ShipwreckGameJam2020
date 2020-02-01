using System;
using Shipwreck.Math;

namespace Shipwreck.WorldData
{
    /// <summary>
    /// Asteroid information
    /// </summary>
    public class Asteroid
    {
        /// <summary>
        /// Gets or sets the asteroid Guid
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the player that fired this asteroid
        /// </summary>
        public Guid Owner { get; set; }

        /// <summary>
        /// Gets or sets the asteroid position
        /// </summary>
        public Vec3 Position { get; set; }

        /// <summary>
        /// Gets or sets the asteroid velocity
        /// </summary>
        public Vec3 Velocity { get; set; }
    }
}

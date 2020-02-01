using System;
using Newtonsoft.Json;
using Shipwreck.Math;

namespace Shipwreck.WorldData
{
    public class Astronaut
    {
        /// <summary>
        /// Gets or sets the player guid for this astronaut
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the position
        /// </summary>
        public Vec2 Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity
        /// </summary>
        public Vec2 Velocity { get; set; }

        /// <summary>
        /// Gets the astronaut position in 3D space
        /// </summary>
        [JsonIgnore]
        public Vec3 Position3D
        {
            get
            {
                var sinX = (float)System.Math.Sin(Position.X);
                var cosX = (float)System.Math.Cos(Position.X);
                return new Vec3(sinX * 10, Position.Y, cosX * 10);
            }
        }
    }
}

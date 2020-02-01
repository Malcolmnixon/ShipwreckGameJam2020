using System;
using Newtonsoft.Json;
using Shipwreck.Math;

namespace Shipwreck.WorldData
{
    /// <summary>
    /// Astronaut information
    /// </summary>
    public class Astronaut
    {
        /// <summary>
        /// Astronaut radius from ship
        /// </summary>
        public const float Radius = 10f;

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
                return new Vec3(sinX * Radius, Position.Y, -cosX * Radius);
            }
        }

        /// <summary>
        /// Serialize to JSON
        /// </summary>
        /// <returns>JSON text</returns>
        public string ToJson() => JsonConvert.SerializeObject(this);

        /// <summary>
        /// Deserialize from JSON
        /// </summary>
        /// <param name="json">JSON text</param>
        /// <returns>Deserialized instance</returns>
        public static Astronaut FromJson(string json) => JsonConvert.DeserializeObject<Astronaut>(json);
    }
}

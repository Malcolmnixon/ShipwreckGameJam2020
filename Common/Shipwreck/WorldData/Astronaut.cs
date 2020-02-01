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
        /// Perform the 2D to 3D position transform
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vec3 To3DPosition(Vec2 position)
        {
            var sinX = (float)System.Math.Sin(position.X);
            var cosX = (float)System.Math.Cos(position.X);
            return new Vec3(sinX * Radius, position.Y * Radius, -cosX * Radius);
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

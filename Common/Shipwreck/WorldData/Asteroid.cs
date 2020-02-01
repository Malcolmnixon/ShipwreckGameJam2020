using System;
using Newtonsoft.Json;
using Shipwreck.Math;

namespace Shipwreck.WorldData
{
    /// <summary>
    /// Asteroid information
    /// </summary>
    public class Asteroid
    {
        /// <summary>
        /// Radius at which asteroids are fired from
        /// </summary>
        public const float FireRadius = 100f;

        /// <summary>
        /// Radius at which asteroids are killed
        /// </summary>
        public const float KillRadius = 120f;

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
        public static Asteroid FromJson(string json) => JsonConvert.DeserializeObject<Asteroid>(json);
    }
}

using System;
using Newtonsoft.Json;
using Shipwreck.Math;
using Shipwreck.World;

namespace Shipwreck.WorldData
{
    /// <summary>
    /// Astronaut mode
    /// </summary>
    public enum AstronautMode
    {
        /// <summary>
        /// Astronaut
        /// </summary>
        Astronaut,

        /// <summary>
        /// Pilot
        /// </summary>
        Pilot,

        /// <summary>
        /// Astronaut-healing
        /// </summary>
        AstronautHealing,

        /// <summary>
        /// Pilot-shielding
        /// </summary>
        PilotShielding
    };

    /// <summary>
    /// Astronaut information
    /// </summary>
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
        /// Astronaut mode
        /// </summary>
        public AstronautMode Mode { get; set; }

        /// <summary>
        /// Gets the astronaut 3D position
        /// </summary>
        [JsonIgnore]
        public Vec3 Position3D => To3DPosition(Position);

        /// <summary>
        /// Perform the 2D to 3D position transform
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vec3 To3DPosition(Vec2 position)
        {
            var sinX = (float)System.Math.Sin(position.X);
            var cosX = (float)System.Math.Cos(position.X);
            return new Vec3(
                sinX * GameConstants.ShieldRadius,
                position.Y * GameConstants.ShieldRadius,
                -cosX * GameConstants.ShieldRadius);
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

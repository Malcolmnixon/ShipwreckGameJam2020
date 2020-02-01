using System;
using Newtonsoft.Json;

namespace Shipwreck.WorldData
{
    /// <summary>
    /// Types of players
    /// </summary>
    public enum PlayerType
    {
        None,
        Astronaut,
        Alien
    }

    /// <summary>
    /// Player information
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets or sets the player GUID
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the player name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the player type
        /// </summary>
        public PlayerType Type { get; set; }

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
        public static Player FromJson(string json) => JsonConvert.DeserializeObject<Player>(json);
    }
}

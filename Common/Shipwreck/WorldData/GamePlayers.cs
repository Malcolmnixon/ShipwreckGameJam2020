using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shipwreck.WorldData
{
    /// <summary>
    /// Collection of all players
    /// </summary>
    public class GamePlayers
    {
        /// <summary>
        /// Gets or sets the list of game players
        /// </summary>
        public List<Player> Players { get; set; }

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
        public static GamePlayers FromJson(string json) => JsonConvert.DeserializeObject<GamePlayers>(json);
    }
}

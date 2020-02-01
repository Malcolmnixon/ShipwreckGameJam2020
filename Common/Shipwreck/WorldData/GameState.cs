using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shipwreck.WorldData
{
    /// <summary>
    /// Game state information
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Gets or sets the ship
        /// </summary>
        public Ship Ship { get; set; }

        /// <summary>
        /// Gets or sets the list of astronauts
        /// </summary>
        public List<Astronaut> Astronauts { get; set; }

        /// <summary>
        /// Gets or sets the list of asteroids
        /// </summary>
        public List<Asteroid> Asteroid { get; set; }

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
        public static GameState FromJson(string json) => JsonConvert.DeserializeObject<GameState>(json);
    }
}

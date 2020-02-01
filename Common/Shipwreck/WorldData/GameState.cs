using System.Collections.Generic;

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
    }
}

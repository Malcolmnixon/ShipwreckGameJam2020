using System;

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
    }
}

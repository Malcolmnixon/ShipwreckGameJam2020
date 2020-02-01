using System;
using Shipwreck.Math;
using Shipwreck.WorldData;

namespace Shipwreck
{
    public interface IWorld : IDisposable
    {
        /// <summary>
        /// Gets the local player (or null if none created)
        /// </summary>
        Player LocalPlayer { get; }

        /// <summary>
        /// Gets the local astronaut (if provided by server)
        /// </summary>
        Astronaut LocalAstronaut { get; }

        /// <summary>
        /// Gets the game players
        /// </summary>
        GamePlayers Players { get; }

        /// <summary>
        /// Gets the game state
        /// </summary>
        GameState State { get; }

        /// <summary>
        /// Start the world
        /// </summary>
        void Start();

        /// <summary>
        /// Trigger a world update
        /// </summary>
        void Update();

        /// <summary>
        /// Create a local player
        /// </summary>
        /// <returns>Local player</returns>
        Player CreateLocalPlayer(string name);

        /// <summary>
        /// Fire an asteroid
        /// </summary>
        /// <param name="position">Initial position</param>
        /// <param name="velocity">Velocity</param>
        /// <returns>New asteroid</returns>
        Asteroid FireAsteroid(Vec3 position, Vec3 velocity);
    }
}

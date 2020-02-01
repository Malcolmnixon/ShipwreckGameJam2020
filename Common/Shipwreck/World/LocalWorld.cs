using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shipwreck.Math;
using Shipwreck.WorldData;

namespace Shipwreck.World
{
    /// <summary>
    /// Local world
    /// </summary>
    public class LocalWorld : BaseWorld
    {
        public override Player CreateLocalPlayer(string name)
        {
            // Create the local player
            var player = base.CreateLocalPlayer(name);

            lock (WorldLock)
            {
                // Add to the players (if new)
                if (!Players.Players.Exists(p => p.Guid == player.Guid))
                    Players.Players.Add(player);
            }

            return player;
        }

        protected override void Tick(float deltaTime)
        {
            // Perform base tick
            base.Tick(deltaTime);

            // Handle moving players to astronauts
            if (State.Astronauts.Count < 2)
            {
                // TODO: Consider doing this randomly

                // Pick the first alien player
                var player = Players.Players.FirstOrDefault(p => p.Type == PlayerType.Alien);
                if (player != null)
                {
                    // Promote player to astronaut
                    player.Type = PlayerType.Astronaut;

                    // Add a new astronaut for the player
                    State.Astronauts.Add(new Astronaut {Guid = player.Guid});
                }
            }

            // Ensure our LocalAstronaut stays up to date
            if (LocalPlayer != null)
                LocalAstronaut = State.Astronauts.FirstOrDefault(a => a.Guid == LocalPlayer.Guid);
        }
    }
}

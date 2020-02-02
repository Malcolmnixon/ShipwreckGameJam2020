using System;
using System.Collections.Generic;
using System.Linq;
using Shipwreck.Math;
using Shipwreck.WorldData;

namespace Shipwreck.World
{
    /// <summary>
    /// Local world
    /// </summary>
    public class LocalWorld : BaseWorld
    {
        /// <summary>
        /// Random source
        /// </summary>
        private readonly Random _random = new Random();

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

            // Process all players
            foreach (var player in Players.Players)
            {
                switch (player.Type)
                {
                    case PlayerType.Pilot:
                    case PlayerType.Astronaut:
                        // Pilots or astronauts stay
                        break;

                    case PlayerType.Alien:
                        // See if we need more astronauts
                        if (State.Astronauts.Count < GameConstants.Astronauts)
                        {
                            // Promote alien to astronaut
                            player.Type = PlayerType.Astronaut;
                            State.Astronauts.Add(new Astronaut { Guid = player.Guid });
                            PlayersUpdated = true;
                        }
                        break;

                    case PlayerType.None:
                        // See if we need more astronauts
                        if (State.Astronauts.Count < GameConstants.Astronauts)
                        {
                            // Promote none to astronaut
                            player.Type = PlayerType.Astronaut;
                            State.Astronauts.Add(new Astronaut { Guid = player.Guid });
                            PlayersUpdated = true;
                        }
                        else
                        {
                            // Promote none to alien
                            player.Type = PlayerType.Alien;
                            PlayersUpdated = true;
                        }
                        break;

                    case PlayerType.Observer:
                        // We cannot recruit observers
                        break;
                }
            }

            // Handle game-mode transition
            switch (State.Mode)
            {
                case GameMode.Waiting:
                {
                    // Update remaining time based on whether we have players
                    if (Players.Players.Count == 0)
                        State.RemainingTime = GameConstants.PlayerWaitTime;
                    else
                        State.RemainingTime -= deltaTime;

                    // Handle start of game
                    if (State.RemainingTime <= 0.0f)
                    {
                        // Reset the ship to full health
                        State.Ship = new Ship
                        {
                            Wing1Health = 100f,
                            Wing2Health = 100f,
                            Wing3Health = 100f
                        };

                        // Destroy all asteroids
                        State.Asteroids = new List<Asteroid>();

                        // Start the game
                        State.RemainingTime = GameConstants.PlayTime;
                        State.Mode = GameMode.Playing;
                    }
                    break;
                }

                case GameMode.Playing:
                {
                    // Handle abandoned game
                    if (Players.Players.Count == 0)
                    {
                        // Transition to waiting
                        State.RemainingTime = 0.0f;
                        State.Mode = GameMode.Waiting;
                        break;
                    }

                    // Track current game
                    State.RemainingTime -= deltaTime;
                    if (State.RemainingTime <= 0.0f || State.Ship.TotalHealth < 50.0f)
                    {
                        // Finish the game
                        State.RemainingTime = GameConstants.FinishedTime;
                        State.Mode = GameMode.Finished;
                    }
                    break;
                }

                case GameMode.Finished:
                {
                    State.RemainingTime -= deltaTime;
                    if (State.RemainingTime <= 0.0f)
                    {
                        // Transition to waiting
                        State.RemainingTime = 0.0f;
                        State.Mode = GameMode.Waiting;
                    }
                    break;
                }
            }

            // Handle asteroids while playing
            if (State.Mode == GameMode.Playing)
            {
                // Handle AI asteroids
                if (State.Asteroids.Count < GameConstants.MinAsteroidCount)
                    // Randomize ~1s for asteroid firing
                    if (_random.NextDouble() < deltaTime / GameConstants.AsteroidFireRate)
                    {
                        var r = _random.NextDouble() * System.Math.PI * 2;
                        var x = (float) System.Math.Sin(r) * GameConstants.AsteroidFireDistance;
                        var z = (float) System.Math.Cos(r) * GameConstants.AsteroidFireDistance;
                        var y = (float) (_random.NextDouble() - 0.5) * GameConstants.AsteroidFireDistance;
                        var pos = new Vec3(x, y, z).Normalized * GameConstants.AsteroidFireDistance;
                        var vel = (-pos).Normalized * (8f + (float) _random.NextDouble() * 4f);
                        State.Asteroids.Add(
                            new Asteroid
                            {
                                Guid = Guid.NewGuid(),
                                Position = pos,
                                Velocity = vel
                            });
                    }

                // Get the list of asteroids that collide with the station
                var colliding = State.Asteroids.Where(a => a.Position.LengthSquared < 64).ToList();
                var damage = State.Ship.Shielded ? 0f : colliding.Count * 0.4f;
                State.Ship.Wing2Health -= damage;
                State.Ship.Wing3Health -= damage;
                State.Ship.Wing1Health -= damage;

                // Handle deleting asteroids
                State.Asteroids = State.Asteroids
                    .Except(colliding)
                    .Where(a => a.Position.LengthSquared < GameConstants.AsteroidDeleteDistanceSquared)
                    .ToList();
            }
        }
    }
}

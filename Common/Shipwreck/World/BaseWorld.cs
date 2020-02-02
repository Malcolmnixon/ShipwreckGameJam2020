using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Shipwreck.Math;
using Shipwreck.WorldData;

namespace Shipwreck.World
{
    public class BaseWorld : IWorld
    {
        /// <summary>
        /// Cancellation token to stop world
        /// </summary>
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();

        /// <summary>
        /// Stopwatch for time measurement
        /// </summary>
        private readonly Stopwatch _time = Stopwatch.StartNew();

        /// <summary>
        /// World update thread
        /// </summary>
        private readonly Thread _thread;

        /// <summary>
        /// Last update in milliseconds
        /// </summary>
        private long _lastUpdate;

        /// <summary>
        /// Initializes a new instance of the BaseWorld class
        /// </summary>
        public BaseWorld()
        {
            // Build a local guid for the player in the world
            LocalGuid = Guid.NewGuid();

            // Construct local player, but player is None
            LocalPlayer = new Player
            {
                Guid = LocalGuid,
                Name = string.Empty,
                Type = PlayerType.None
            };

            // Construct local astronaut, but only valid when player is Astronaut
            LocalAstronaut = new Astronaut
            {
                Guid = LocalGuid,
            };

            _thread = new Thread(ThreadProc);
        }

        /// <summary>
        /// Lock object for accessing world data
        /// </summary>
        protected object WorldLock { get; } = new object();

        /// <summary>
        /// Local Guid
        /// </summary>
        public Guid LocalGuid { get; }

        /// <summary>
        /// Gets or sets the local player
        /// </summary>
        public Player LocalPlayer { get; }

        /// <summary>
        /// Gets or sets the local astronaut
        /// </summary>
        public Astronaut LocalAstronaut { get; }

        /// <summary>
        /// Get all other astronauts
        /// </summary>
        public List<Astronaut> OtherAstronauts => State.Astronauts.Where(a => a.Guid != LocalGuid).ToList();

        /// <summary>
        /// Gets or sets the players
        /// </summary>
        public GamePlayers Players { get; private set; } = new GamePlayers
        {
            Players = new List<Player>()
        };

        /// <summary>
        /// Gets or sets whether the players are updated
        /// </summary>
        protected bool PlayersUpdated { get; set; }

        /// <summary>
        /// Gets or sets the game state
        /// </summary>
        public GameState State { get; private set; } = new GameState
        {
            Mode = GameMode.Waiting,
            RemainingTime = GameConstants.PlayerWaitTime,
            Ship = new Ship
            {
                Wing1Health = 50,
                Wing2Health = 50,
                Wing3Health = 50,
                Shielded = false
            },
            Astronauts = new List<Astronaut>(),
            Asteroids = new List<Asteroid>()
        };

        public virtual void Dispose()
        {
            if (!_thread.IsAlive)
                return;

            _cancel.Cancel();
            _thread.Join();
        }

        public virtual void Start()
        {
            _time.Restart();
            _lastUpdate = 0;
            _thread.Start();
        }

        public void Update()
        {
            FireTick();
        }

        public virtual Player CreateLocalPlayer(string name)
        {
            lock (WorldLock)
            {
                // Set name and request player be an alien
                LocalPlayer.Name = name;
                LocalPlayer.Type = PlayerType.None;
                PlayersUpdated = true;
                return LocalPlayer;
            }
        }

        public virtual Asteroid FireAsteroid(Vec3 position, Vec3 velocity)
        {
            lock (WorldLock)
            {
                // Create asteroid
                var asteroid = new Asteroid
                {
                    Guid = Guid.NewGuid(),
                    Owner = LocalGuid,
                    Position = position,
                    Velocity = velocity
                };

                // Add to our game state
                State.Asteroids.Add(asteroid);

                // Asteroid fired
                Logger.Log("BaseWorld.FireAsteroid - fired");

                // Return the new asteroid
                return asteroid;
            }
        }

        /// <summary>
        /// Handle world ticks
        /// </summary>
        protected virtual void Tick(float deltaTime)
        {
            //Logger.Log($"BaseWorld.Tick - updating world");

            // Advance asteroids
            foreach (var asteroid in State.Asteroids)
                asteroid.Position += asteroid.Velocity * deltaTime;

            // Advance all astronauts not played by the local player
            foreach (var astronaut in OtherAstronauts)
                astronaut.Position += astronaut.Velocity * deltaTime;

            // Update the game astronaut matching our local
            if (LocalPlayer.Type == PlayerType.Astronaut)
            {
                var gameLocalAstronaut = State.Astronauts.FirstOrDefault(a => a.Guid == LocalGuid);
                if (gameLocalAstronaut != null)
                {
                    gameLocalAstronaut.Position = LocalAstronaut.Position;
                    gameLocalAstronaut.Velocity = LocalAstronaut.Velocity;
                    gameLocalAstronaut.Mode = LocalAstronaut.Mode;
                }
            }
        }

        protected void UpdatePlayers(GamePlayers newPlayers)
        {
            lock (WorldLock)
            {
                // Save the new players
                Players = newPlayers;

                // Find if the game thinks we have a player
                var gameLocalPlayer = Players.Players.FirstOrDefault(p => p.Guid == LocalGuid);
                if (gameLocalPlayer != null)
                {
                    // Use the server-provided player type
                    LocalPlayer.Type = gameLocalPlayer.Type;
                }
                else
                {
                    // Server has nothing, set us to none
                    LocalPlayer.Type = PlayerType.None;
                }
            }
        }

        /// <summary>
        /// Update the game state merging in all information possible
        /// </summary>
        /// <param name="newState"></param>
        protected void UpdateState(GameState newState)
        {
            lock (WorldLock)
            {
                // Save the new state
                State = newState;
            }
        }

        /// <summary>
        /// Fire the tick function with the world locked
        /// </summary>
        private void FireTick()
        {
            lock (WorldLock)
            {
                // Track time
                var now = _time.ElapsedMilliseconds;
                var deltaTime = (now - _lastUpdate) * 0.001f;
                _lastUpdate = now;

                // Fire the tick function with the delta-time and the world locked for update
                Tick(deltaTime);
            }
        }

        /// <summary>
        /// Thread procedure to tick the world
        /// </summary>
        private void ThreadProc()
        {
            // Loop until asked to cancel
            while (!_cancel.IsCancellationRequested)
            {
                // Wait for around 50ms
                _cancel.Token.WaitHandle.WaitOne(50);

                // Fire the tick function
                FireTick();
            }
        }
    }
}

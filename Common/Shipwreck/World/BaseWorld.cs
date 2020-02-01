using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            _thread = new Thread(ThreadProc);
        }

        /// <summary>
        /// Lock object for accessing world data
        /// </summary>
        protected object WorldLock { get; } = new object();

        /// <summary>
        /// Gets or sets the local player
        /// </summary>
        public Player LocalPlayer { get; private set; }

        /// <summary>
        /// Gets or sets the local astronaut
        /// </summary>
        public Astronaut LocalAstronaut { get; protected set; }

        /// <summary>
        /// Gets or sets the players
        /// </summary>
        public GamePlayers Players { get; set; } = new GamePlayers
        {
            Players = new List<Player>()
        };

        /// <summary>
        /// Gets or sets the game state
        /// </summary>
        public GameState State { get; set; } = new GameState
        {
            Ship = new Ship
            {
                CenterTorsoHealth = 100,
                LeftWingHealth = 100,
                RightWingHealth = 100,
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
                return LocalPlayer ?? (LocalPlayer = new Player
                {
                    Guid = Guid.NewGuid(),
                    Name = name,
                    Type = PlayerType.Alien
                });
            }
        }

        public virtual Asteroid FireAsteroid(Vec3 position, Vec3 velocity)
        {
            lock (WorldLock)
            {
                // Ensure we have a local player
                if (LocalPlayer == null)
                    throw new Exception("Cannot fire asteroid without a local player");

                // Create asteroid
                var asteroid = new Asteroid
                {
                    Guid = Guid.NewGuid(),
                    Owner = LocalPlayer.Guid,
                    Position = position,
                    Velocity = velocity
                };

                // Add to our game state
                State.Asteroids.Add(asteroid);

                // Return the new asteroid
                return asteroid;
            }
        }

        /// <summary>
        /// Handle world ticks
        /// </summary>
        protected virtual void Tick(float deltaTime)
        {
            // Advance asteroids
            foreach (var asteroid in State.Asteroids)
                asteroid.Position += asteroid.Velocity * deltaTime;

            // Advance all astronauts not played by the local player
            foreach (var astronaut in State.Astronauts)
                if (astronaut.Guid != LocalPlayer.Guid)
                    astronaut.Position += astronaut.Velocity * deltaTime;
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

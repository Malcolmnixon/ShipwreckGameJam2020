using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Shipwreck.Math;
using Shipwreck.World;
using Shipwreck.WorldData;

namespace Shipwreck.WorldNet
{
    /// <summary>
    /// Network-client world
    /// </summary>
    public class NetClientWorld : RemoteWorld
    {
        /// <summary>
        /// Communications connection
        /// </summary>
        private readonly NetComms.IConnection _communicationsConnection;

        /// <summary>
        /// Age of astronaut
        /// </summary>
        private float _astronautAge;

        public NetClientWorld(IPAddress server)
        {
            var provider = new NetComms.Udp.UdpProvider(NetConstants.UdpPort);
            _communicationsConnection = provider.CreateClient(server);
            _communicationsConnection.Notification += OnServerNotification;
            _communicationsConnection.ConnectionDropped += OnServerConnectionDropped;
        }

        public override void Dispose()
        {
            _communicationsConnection.Dispose();
            base.Dispose();
        }

        public override void Start()
        {
            base.Start();
            _communicationsConnection.Start();
        }

        private void OnServerConnectionDropped(object sender, NetComms.ConnectionEventArgs e)
        {
            // TODO: Handle connection dropped
        }

        private void OnServerNotification(object sender, NetComms.NotificationEventArgs e)
        {
            try
            {
                // Skip if no type
                if (e.Notification.Length == 0)
                    return;

                // Get the packet type
                var type = e.Notification[0];

                // Get the payload JSON
                var payloadJson = Encoding.ASCII.GetString(e.Notification, 1, e.Notification.Length - 1);

                lock (WorldLock)
                {
                    switch (type)
                    {
                        case NetConstants.PlayersPacket:
                        {
                            var players = GamePlayers.FromJson(payloadJson);

                            // If we have a local player then synchronize with new data
                            if (LocalPlayer != null)
                            {
                                // Find if the server has any new information
                                var serverPlayer = players.Players.FirstOrDefault(p => p.Guid == LocalPlayer.Guid);
                                if (serverPlayer != null)
                                {
                                    LocalPlayer.Type = serverPlayer.Type;
                                }
                            }

                            // Write the new players
                            Players = players;
                            break;
                        }

                        case NetConstants.StatePacket:
                        {
                            var state = GameState.FromJson(payloadJson);

                            // If we have a local astronaut then preserve our driven information
                            if (LocalAstronaut != null)
                            {
                                var serverAstronaut =
                                    state.Astronauts.FirstOrDefault(a => a.Guid == LocalAstronaut.Guid);
                                if (serverAstronaut != null)
                                {
                                    serverAstronaut.Position = LocalAstronaut.Position;
                                    serverAstronaut.Velocity = LocalAstronaut.Velocity;
                                    LocalAstronaut = serverAstronaut;
                                }
                            }

                            // Write the new game state
                            State = state;
                            break;
                        }
                    }
                }
            }
            catch
            {
                // Ignore all errors as we don't want the game to fail when given junk
            }
        }

        public override Player CreateLocalPlayer(string name)
        {
            // Create player
            var player = base.CreateLocalPlayer(name);

            // Construct player packet
            var playerBytes = new List<byte> { NetConstants.PlayerPacket };
            playerBytes.AddRange(Encoding.ASCII.GetBytes(player.ToJson()));

            // Send player packet to server
            _communicationsConnection.SendNotification(playerBytes.ToArray());

            // Return player
            return player;
        }

        public override Asteroid FireAsteroid(Vec3 position, Vec3 velocity)
        {
            // Fire the asteroid
            var asteroid = base.FireAsteroid(position, velocity);

            // Construct asteroid packet
            var asteroidBytes = new List<byte> { NetConstants.AsteroidPacket };
            asteroidBytes.AddRange(Encoding.ASCII.GetBytes(asteroid.ToJson()));

            // Send asteroid packet to server
            _communicationsConnection.SendNotification(asteroidBytes.ToArray());

            // Return asteroid
            return asteroid;
        }

        protected override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            // Skip if we have no local astronaut to send
            if (LocalAstronaut == null)
                return;

            // Don't send player state if we sent less than 0.1 seconds ago
            _astronautAge += deltaTime;
            if (_astronautAge < NetConstants.AstronautPeriod)
                return;

            // Construct asteroid packet
            var astronautBytes = new List<byte> { NetConstants.AstronautPacket };
            astronautBytes.AddRange(Encoding.ASCII.GetBytes(LocalAstronaut.ToJson()));

            // Send asteroid packet to server
            _communicationsConnection.SendNotification(astronautBytes.ToArray());
            _astronautAge = 0f;
        }
    }
}

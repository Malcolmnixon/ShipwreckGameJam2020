using System;
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
            Logger.Log($"NetClientWorld.NetClientWorld({server}) - using UDP {NetConstants.UdpPort}");
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
            Logger.Log("NetClientWorld.Start - initiating communications");

            // Start the remote world-mirror
            base.Start();

            // Connect to the server
            _communicationsConnection.Start();
        }

        public override Player CreateLocalPlayer(string name)
        {
            // Create player
            var player = base.CreateLocalPlayer(name);

            // Construct player packet
            var playerBytes = new List<byte> { NetConstants.PlayerPacket };
            playerBytes.AddRange(Encoding.ASCII.GetBytes(player.ToJson()));

            // Send player packet to server
            Logger.Log($"NetClientWorld.CreateLocalPlayer - sent player to server");
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
            Logger.Log($"NetClientWorld.CreateLocalPlayer - sent asteroid to server");
            _communicationsConnection.SendNotification(asteroidBytes.ToArray());

            // Return asteroid
            return asteroid;
        }

        protected override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            // Skip if we aren't an astronaut
            if (LocalPlayer.Type != PlayerType.Astronaut)
                return;

            // Don't send player state if we sent less than 0.1 seconds ago
            _astronautAge += deltaTime;
            if (_astronautAge < NetConstants.AstronautPeriod)
                return;

            // Construct asteroid packet
            var astronautBytes = new List<byte> { NetConstants.AstronautPacket };
            astronautBytes.AddRange(Encoding.ASCII.GetBytes(LocalAstronaut.ToJson()));

            // Send asteroid packet to server
            Logger.Log($"NetClientWorld.CreateLocalPlayer - sent local astronaut to server");
            _communicationsConnection.SendNotification(astronautBytes.ToArray());
            _astronautAge = 0f;
        }

        private void OnServerConnectionDropped(object sender, NetComms.ConnectionEventArgs e)
        {
            Logger.Log("NetClientWorld.OnServerConnectionDropped");
        }

        private void OnServerNotification(object sender, NetComms.NotificationEventArgs e)
        {
            Logger.Log("NetClientWorld.OnServerNotification");

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
                                var newPlayers = GamePlayers.FromJson(payloadJson);
                                Logger.Log($"NetClientWorld.OnServerNotification - got players");

                                // If we have a local player then synchronize with new data
                                if (LocalPlayer != null)
                                {
                                    // Find if the server has any new information
                                    var serverPlayer = newPlayers.Players.FirstOrDefault(p => p.Guid == LocalPlayer.Guid);
                                    if (serverPlayer != null)
                                    {
                                        LocalPlayer.Type = serverPlayer.Type;
                                    }
                                }

                                // Write the new players
                                UpdatePlayers(newPlayers);
                                break;
                            }

                        case NetConstants.StatePacket:
                            {
                                var newState = GameState.FromJson(payloadJson);
                                Logger.Log($"NetClientWorld.OnServerNotification - got state");

                                // Write the new game state
                                UpdateState(newState);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"NetClientWorld.OnServerNotification - failed with {ex}", ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Shipwreck.World;
using Shipwreck.WorldData;

namespace Shipwreck.WorldNet
{
    /// <summary>
    /// Network-server world
    /// </summary>
    public class NetServerWorld : LocalWorld
    {
        /// <summary>
        /// Communications server
        /// </summary>
        private readonly NetComms.IServer _communicationsServer;

        /// <summary>
        /// Age of game players
        /// </summary>
        private float _playersAge;

        /// <summary>
        /// Age of the game state
        /// </summary>
        private float _stateAge;

        public NetServerWorld()
        {
            Logger.Log($"NetServerWorld.NetServerWorld - using UDP {NetConstants.UdpPort}");
            var provider = new NetComms.Udp.UdpProvider(NetConstants.UdpPort);
            _communicationsServer = provider.CreateServer();
            _communicationsServer.NewConnection += OnClientNewConnection;
            _communicationsServer.ConnectionDropped += OnClientConnectionDropped;
            _communicationsServer.Notification += OnClientNotification;
        }

        public override void Dispose()
        {
            // Dispose of the world
            base.Dispose();

            // Stop network communications
            _communicationsServer.Dispose();
        }

        public override void Start()
        {
            Logger.Log("NetServerWorld.Start - initiating communications");

            // Start the world
            base.Start();

            // Start networking
            _communicationsServer.Start();
        }

        protected override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            // Send game-players when the period expires
            _playersAge += deltaTime;
            if (PlayersUpdated || _playersAge >= NetConstants.PlayersPeriod)
            {
                // Clear update flags
                PlayersUpdated = false;
                _playersAge = 0f;

                // Construct players packet
                var playersBytes = new List<byte> {NetConstants.PlayersPacket};
                playersBytes.AddRange(Encoding.ASCII.GetBytes(Players.ToJson()));

                // Send game-players packet and clear time
                Logger.Log($"NetServerWorld.Tick - sent players");
                _communicationsServer.SendNotification(playersBytes.ToArray());
            }

            // Send game-state when the period expires
            _stateAge += deltaTime;
            if (_stateAge >= NetConstants.StatePeriod)
            {
                // Clear update flags
                _stateAge = 0f;

                // Construct state packet
                var stateBytes = new List<byte> {NetConstants.StatePacket};
                stateBytes.AddRange(Encoding.ASCII.GetBytes(State.ToJson()));

                // Send game-state packet and clear time
                Logger.Log($"NetServerWorld.Tick - sent state");
                _communicationsServer.SendNotification(stateBytes.ToArray());
            }
        }

        private void OnClientNewConnection(object sender, NetComms.ConnectionEventArgs e)
        {
            Logger.Log($"NetServerWorld.OnClientNewConnection - new client");
        }


        private void OnClientNotification(object sender, NetComms.NotificationEventArgs e)
        {
            Logger.Log("NetServerWorld.OnClientNotification");
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
                        case NetConstants.PlayerPacket:
                        {
                            var player = Player.FromJson(payloadJson);
                            Logger.Log($"NetServerWorld.OnClientNotification - got player {player.Guid}");
                            Players.Players.RemoveAll(p => p.Guid == player.Guid);
                            Players.Players.Add(player);

                            // Save the player guid
                            e.Connection.AssociatedData = player.Guid;
                            break;
                        }

                        case NetConstants.AstronautPacket:
                        {
                            var astronaut = Astronaut.FromJson(payloadJson);
                            Logger.Log($"NetServerWorld.OnClientNotification - got astronaut update {astronaut.Guid}");
                            State.Astronauts.RemoveAll(a => a.Guid == astronaut.Guid);
                            State.Astronauts.Add(astronaut);
                            break;
                        }

                        case NetConstants.AsteroidPacket:
                        {
                            // Only accept asteroids when playing
                            if (State.Mode == GameMode.Playing)
                            {
                                var asteroid = Asteroid.FromJson(payloadJson);
                                Logger.Log($"NetServerWorld.OnClientNotification - got fired asteroid {asteroid.Guid}");
                                State.Asteroids.RemoveAll(a => a.Guid == asteroid.Guid);
                                State.Asteroids.Add(asteroid);
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"NetServerWorld.OnClientNotification - failed with {ex}", ex);
            }
        }

        private void OnClientConnectionDropped(object sender, NetComms.ConnectionEventArgs e)
        {
            // Get the player GUID
            var playerGuid = (Guid?)e.Connection.AssociatedData ?? Guid.Empty;
            Logger.Log($"NetServerWorld.OnClientConnectionDropped - lost {playerGuid}");

            // Lock while updating game state
            lock (WorldLock)
            {
                Players.Players.RemoveAll(p => p.Guid == playerGuid);
                State.Astronauts.RemoveAll(a => a.Guid == playerGuid);
            }
        }
    }
}

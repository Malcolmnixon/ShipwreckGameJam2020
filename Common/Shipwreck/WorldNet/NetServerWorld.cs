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
            var provider = new NetComms.Udp.UdpProvider(NetConstants.UdpPort);
            _communicationsServer = provider.CreateServer();
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
            if (_playersAge >= NetConstants.PlayersPeriod)
            {
                // Construct players packet
                var playersBytes = new List<byte> {NetConstants.PlayersPacket};
                playersBytes.AddRange(Encoding.ASCII.GetBytes(Players.ToJson()));

                // Send game-players packet and clear time
                _communicationsServer.SendNotification(playersBytes.ToArray());
                _playersAge = 0f;
            }

            // Send game-state when the period expires
            _stateAge += deltaTime;
            if (_stateAge >= NetConstants.StatePeriod)
            {
                // Construct state packet
                var stateBytes = new List<byte> {NetConstants.StatePacket};
                stateBytes.AddRange(Encoding.ASCII.GetBytes(State.ToJson()));

                // Send game-state packet and clear time
                _communicationsServer.SendNotification(stateBytes.ToArray());
                _stateAge = 0f;
            }
        }

        private void OnClientNotification(object sender, NetComms.NotificationEventArgs e)
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
                        case NetConstants.PlayerPacket:
                        {
                            var player = Player.FromJson(payloadJson);
                            Players.Players.RemoveAll(p => p.Guid == player.Guid);
                            Players.Players.Add(player);

                            // Save the player guid
                            e.Connection.AssociatedData = player.Guid;
                            break;
                        }

                        case NetConstants.AstronautPacket:
                        {
                            var astronaut = Astronaut.FromJson(payloadJson);
                            State.Astronauts.RemoveAll(a => a.Guid == astronaut.Guid);
                            State.Astronauts.Add(astronaut);
                            break;
                        }

                        case NetConstants.AsteroidPacket:
                        {
                            var asteroid = Asteroid.FromJson(payloadJson);
                            State.Asteroids.RemoveAll(a => a.Guid == asteroid.Guid);
                            State.Asteroids.Add(asteroid);
                            break;
                        }
                    }
                }
            }
            catch
            {
                // Ignore all errors as we don't want the server to fail when given junk
            }
        }

        private void OnClientConnectionDropped(object sender, NetComms.ConnectionEventArgs e)
        {
            // Get the player GUID
            var playerGuid = (Guid?)e.Connection.AssociatedData ?? Guid.Empty;

            // Lock while updating game state
            lock (WorldLock)
            {
                Players.Players.RemoveAll(p => p.Guid == playerGuid);
                State.Astronauts.RemoveAll(a => a.Guid == playerGuid);
            }
        }
    }
}

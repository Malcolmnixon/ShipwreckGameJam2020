namespace Shipwreck.WorldNet
{
    public static class NetConstants
    {
        /// <summary>
        /// UDP port for game
        /// </summary>
        public const int UdpPort = 51238;

        /// <summary>
        /// Period at which players is sent
        /// </summary>
        public const float PlayersPeriod = 4f;

        /// <summary>
        /// Period at which state is sent
        /// </summary>
        public const float StatePeriod = 0.1f;

        /// <summary>
        /// Period at which astronauts are sent
        /// </summary>
        public const float AstronautPeriod = 0.1f;

        /// <summary>
        /// Identifier for players packet (server -> clients)
        /// </summary>
        public const byte PlayersPacket = 0x01;

        /// <summary>
        /// Identifier for state packet (server -> clients)
        /// </summary>
        public const byte StatePacket = 0x02;

        /// <summary>
        /// Identifier for player packet (client -> server)
        /// </summary>
        public const byte PlayerPacket = 0x81;

        /// <summary>
        /// Identifier for astronaut packet (client -> server)
        /// </summary>
        public const byte AstronautPacket = 0x82;

        /// <summary>
        /// Identifier for asteroid packet (client -> server
        /// </summary>
        public const byte AsteroidPacket = 0x83;
    }
}

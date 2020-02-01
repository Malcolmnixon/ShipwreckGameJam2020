namespace Shipwreck.World
{
    /// <summary>
    /// Game constants
    /// </summary>
    public static class GameConstants
    {
        /// <summary>
        /// Count of astronauts
        /// </summary>
        public const int Astronauts = 2;

        /// <summary>
        /// Wait duration when stuck with only one player
        /// </summary>
        public const float PlayerWaitTime = 10.0f;

        /// <summary>
        /// Game play time
        /// </summary>
        public const float PlayTime = 120.0f;

        /// <summary>
        /// Finished summary time
        /// </summary>
        public const float FinishedTime = 10.0f;

        /// <summary>
        /// Minimum asteroid count (less than this and the AI will fire one)
        /// </summary>
        public const int MinAsteroidCount = 3;

        /// <summary>
        /// Distance at which to fire asteroids
        /// </summary>
        public const float AsteroidFireDistance = 100f;

        /// <summary>
        /// Distance at which to delete asteroids
        /// </summary>
        public const float AsteroidDeleteDistance = 120f;

        /// <summary>
        /// Asteroid delete distance squared
        /// </summary>
        public const float AsteroidDeleteDistanceSquared = AsteroidDeleteDistance * AsteroidDeleteDistance;
    }
}

using Shipwreck.Math;

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
        public const int MinAsteroidCount = 6;

        /// <summary>
        /// Distance at which to fire asteroids
        /// </summary>
        public const float AsteroidFireDistance = 100f;

        /// <summary>
        /// Rate of auto-fire
        /// </summary>
        public const float AsteroidFireRate = 40f;

        /// <summary>
        /// Damage taken by asteroid hit
        /// </summary>
        public const float AsteroidDamage = 10f;

        /// <summary>
        /// Distance at which to delete asteroids
        /// </summary>
        public const float AsteroidDeleteDistance = 120f;

        /// <summary>
        /// Asteroid delete distance squared
        /// </summary>
        public const float AsteroidDeleteDistanceSquared = AsteroidDeleteDistance * AsteroidDeleteDistance;

        /// <summary>
        /// Ship radius
        /// </summary>
        public const float ShipRadius = 6f;

        /// <summary>
        /// Ship radius squared
        /// </summary>
        public const float ShipRadiusSquared = ShipRadius * ShipRadius;

        /// <summary>
        /// Shield radius (also used for astronauts)
        /// </summary>
        public const float ShieldRadius = 10f;

        /// <summary>
        /// Astronaut radius
        /// </summary>
        public const float AstronautRadius = 10f;

        /// <summary>
        /// Shield radius squared
        /// </summary>
        public const float ShieldRadiusSquared = ShieldRadius * ShieldRadius;

        /// <summary>
        /// Heal rate (per second)
        /// </summary>
        public const float HealRate = 3f;

        /// <summary>
        /// Radius of the wing
        /// </summary>
        public const float WingRadius = 4.6f;

        /// <summary>
        /// Radius of the wing squared
        /// </summary>
        public const float WingRadiusSquared = WingRadius * WingRadius;

        /// <summary>
        /// Location of wing 1 collision sphere
        /// </summary>
        public static readonly Vec3 Wing1Position = new Vec3(-8.74f, 0, 0);

        /// <summary>
        /// Location of wing 2 collision sphere
        /// </summary>
        public static readonly Vec3 Wing2Position = new Vec3(4.37f, 0, 7.57f);

        /// <summary>
        /// Location of wing 3 collision sphere
        /// </summary>
        public static readonly Vec3 Wing3Position = new Vec3(4.37f, 0, -7.57f);
    }
}

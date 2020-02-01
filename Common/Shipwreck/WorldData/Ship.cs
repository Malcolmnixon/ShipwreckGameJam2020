using System;

namespace Shipwreck.WorldData
{
    /// <summary>
    /// Ship status
    /// </summary>
    public class Ship
    {
        /// <summary>
        /// GUID for ship pilot
        /// </summary>
        public Guid Pilot { get; set; }

        /// <summary>
        /// Center torso health (0..100)
        /// </summary>
        public float CenterTorsoHealth { get; set; }

        /// <summary>
        /// Left wing health (0..100)
        /// </summary>
        public float LeftWingHealth { get; set; }

        /// <summary>
        /// Right wing health (0..100)
        /// </summary>
        public float RightWingHealth { get; set; }

        /// <summary>
        /// Gets a value indicating whether the ship is shielded
        /// </summary>
        public bool Shielded { get; set; }
    }
}

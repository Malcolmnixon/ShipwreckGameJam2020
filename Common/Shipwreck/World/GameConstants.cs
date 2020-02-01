using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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
    }
}

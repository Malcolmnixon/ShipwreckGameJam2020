using Newtonsoft.Json;

namespace Shipwreck.Math
{
    /// <summary>
    /// 2D Vector struct
    /// </summary>
    public struct Vec2
    {
        /// <summary>
        /// Initializes a new instance of the Vec2 structure
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the X component
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the Y component
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets the vector length squared
        /// </summary>
        [JsonIgnore]
        public float LengthSquared => X * X + Y * Y;

        /// <summary>
        /// Gets the vector length
        /// </summary>
        [JsonIgnore]
        public float Length => (float) System.Math.Sqrt(LengthSquared);

        /// <summary>
        /// Gets a normalized version of this vector
        /// </summary>
        [JsonIgnore]
        public Vec2 Normalized
        {
            get
            {
                // Get vector length and handle tiny vector
                var len = Length;
                if (len < 1e-5) return new Vec2(0, 1);

                // Return normalized vector
                return this * (1 / len);
            }
        }

        /// <summary>
        /// Vector negation
        /// </summary>
        /// <param name="v">Vector</param>
        /// <returns>Negated vector</returns>
        public static Vec2 operator -(Vec2 v) => new Vec2(-v.X, -v.Y);

        /// <summary>
        /// Add vectors
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>Vector a + b</returns>
        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);

        /// <summary>
        /// Subtract vectors
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>Vector a - b</returns>
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);

        /// <summary>
        /// Scale a vector
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="s">Scale</param>
        /// <returns>Scaled vector</returns>
        public static Vec2 operator *(Vec2 v, float s) => new Vec2(v.X * s, v.Y * s);
    }
}

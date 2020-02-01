using Newtonsoft.Json;

namespace Shipwreck.Math
{
    /// <summary>
    /// 3D vector struct
    /// </summary>
    public struct Vec3
    {
        /// <summary>
        /// Initializes a new instance of the Vec3 struct
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        /// <param name="z">Z component</param>
        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
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
        /// Gets or sets the Z component
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Gets the vector length squared
        /// </summary>
        [JsonIgnore]
        public float LengthSquared => X * X + Y * Y + Z * Z;

        /// <summary>
        /// Gets the vector length
        /// </summary>
        [JsonIgnore]
        public float Length => (float)System.Math.Sqrt(LengthSquared);

        /// <summary>
        /// Gets a normalized version of this vector
        /// </summary>
        [JsonIgnore]
        public Vec3 Normalized
        {
            get
            {
                // Get vector length and handle tiny vector
                var len = Length;
                if (len < 1e-5) return new Vec3(0, 1, 0);

                // Return normalized vector
                return this * (1 / len);
            }
        }

        /// <summary>
        /// Vector negation
        /// </summary>
        /// <param name="v">Vector</param>
        /// <returns>Negated vector</returns>
        public static Vec3 operator -(Vec3 v) => new Vec3(-v.X, -v.Y, -v.Z);

        /// <summary>
        /// Add vectors
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>Vector a + b</returns>
        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        /// <summary>
        /// Subtract vectors
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>Vector a - b</returns>
        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <summary>
        /// Scale a vector
        /// </summary>
        /// <param name="v">Vector</param>
        /// <param name="s">Scale</param>
        /// <returns>Scaled vector</returns>
        public static Vec3 operator *(Vec3 v, float s) => new Vec3(v.X * s, v.Y * s, v.Z * s);
    }
}

using System;
using Shipwreck.Math;

/// <summary>
/// Alien information
/// </summary>
public class Alien
{
    /// <summary>
    /// Alien radius from ship
    /// </summary>
    public const float Radius = Shipwreck.WorldData.Asteroid.FireRadius;

    /// <summary>
    /// Gets or sets the position
    /// </summary>
    public Vec2 Position { get; set; }

    /// <summary>
    /// Gets or sets the velocity
    /// </summary>
    public Vec2 Velocity { get; set; }

    /// <summary>
    /// Gets the alien position in 3D space
    /// </summary>
    public Vec3 Position3D
    {
        get
        {
            var sinX = (float)System.Math.Sin(Position.X);
            var cosX = (float)System.Math.Cos(Position.X);
            return new Vec3(sinX * Radius, Position.Y * Radius, -cosX * Radius);
        }
    }
}
using Shipwreck.Math;
using UnityEngine;

public static class Vec2Helpers
{
    public static Vector2 ToVector2(this Vec2 vec) => new Vector2(vec.X, vec.Y);

    public static Vec2 ToVec2(this Vector2 vec) => new Vec2(vec.x, vec.y);
}

public static class Vec3Helpers
{
    public static Vector3 ToVector3(this Vec3 vec) => new Vector3(vec.X, vec.Y, vec.Z);

    public static Vec3 ToVec3(this Vector3 vec) => new Vec3(vec.x, vec.y, vec.z);
}
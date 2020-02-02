using Shipwreck.Math;
using Shipwreck.WorldData;
using UnityEngine;

public class AstronautPlayerController : AstronautController
{
    public Vec2 Position;

    public Vec2 Velocity;

    // Update is called once per frame
    void Update()
    {
        // Update position
        Position.X += InputManagerData.Mouse.x * 0.6f * Time.deltaTime;
        Position.Y += InputManagerData.Mouse.y * 0.6f * Time.deltaTime;

        // Clamp position
        while (Position.X < -Mathf.PI) Position.X += Mathf.PI * 2;
        while (Position.X > Mathf.PI) Position.X -= Mathf.PI * 2;
        Position.Y = Mathf.Clamp(Position.Y, -1f, 1f);
    }
}

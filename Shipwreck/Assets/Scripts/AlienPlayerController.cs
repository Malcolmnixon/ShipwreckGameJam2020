using Shipwreck.Math;
using UnityEngine;

public class AlienPlayerController : MonoBehaviour
{
    public Vec2 Position;

    public Vec2 Velocity;

    public AudioSource fireSfx;

    /// <summary>
    /// Alien radius from ship
    /// </summary>
    public const float Radius = Shipwreck.WorldData.Asteroid.FireRadius;

    private Vector2 _position;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(Vector3.zero);
    }

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

        // Calculate 3D position from 2D
        var sinX = (float)System.Math.Sin(Position.X);
        var cosX = (float)System.Math.Cos(Position.X);
        transform.position = new Vector3(sinX * Radius, Position.Y * Radius, -cosX * Radius);
        transform.LookAt(Vector3.zero);
    }
}

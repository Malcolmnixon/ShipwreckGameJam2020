using Shipwreck.Math;
using Shipwreck.WorldData;
using UnityEngine;

public class AstronautPlayerController : MonoBehaviour
{
    public Vec2 Position;

    public Vec2 Velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        // Update position
        Position.X += horizontal * 0.6f * Time.deltaTime;
        Position.Y += vertical * 0.6f * Time.deltaTime;

        // Clamp position
        while (Position.X < -Mathf.PI) Position.X += Mathf.PI * 2;
        while (Position.X > Mathf.PI) Position.X -= Mathf.PI * 2;
        Position.Y = Mathf.Clamp(Position.Y, -1f, 1f);

        // Update transform
        transform.position = Astronaut.To3DPosition(Position).ToVector3();
        transform.LookAt(Vector3.zero);
    }
}

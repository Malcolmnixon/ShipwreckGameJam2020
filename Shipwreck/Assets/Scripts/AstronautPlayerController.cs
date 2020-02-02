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
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");


        // Handle screen touches.
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 touchPos = touch.position;
            if (touchPos.x < (Screen.width * 0.45))
            {
                horizontal = -1;
            }
            else if (touchPos.x > (Screen.width * 0.55))
            {
                horizontal = 1;
            }
            if (touchPos.y < (Screen.height * 0.45))
            {
                horizontal = -1;
            }
            else if (touchPos.y > (Screen.height * 0.55))
            {
                horizontal = 1;
            }
        }

        // Update position
        Position.X += horizontal * 0.6f * Time.deltaTime;
        Position.Y += vertical * 0.6f * Time.deltaTime;

        // Clamp position
        while (Position.X < -Mathf.PI) Position.X += Mathf.PI * 2;
        while (Position.X > Mathf.PI) Position.X -= Mathf.PI * 2;
        Position.Y = Mathf.Clamp(Position.Y, -1f, 1f);
    }
}

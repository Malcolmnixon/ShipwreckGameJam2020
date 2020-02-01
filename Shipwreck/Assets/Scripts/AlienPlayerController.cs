using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shipwreck.Math;

public class AlienPlayerController : MonoBehaviour
{
    private Alien alien;
    private float WalkSpeed = 40f;
    private float TurnSpeed = 180f;

    private float rotate;
    private Vec2 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        alien = new Alien();
        transform.LookAt(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        // Handle screen touches.
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPos = touch.position;
                if (touchPos.x < (Screen.width * 0.3)) 
                {
                    horizontal = -1;
                }
                else if (touchPos.x > (Screen.width * 0.7)) 
                {
                    horizontal = 1;
                }
                if (touchPos.y < (Screen.height * 0.3)) 
                {
                    horizontal = -1;
                }
                else if (touchPos.y > (Screen.height * 0.7)) 
                {
                    horizontal = 1;
                }

            }
        }


        // Get position
        var pos = alien.Position;

        // Update position
        pos.X += horizontal * 0.6f * Time.deltaTime;
        pos.Y += vertical * 0.6f * Time.deltaTime;

        // Clamp position
        while (pos.X < -Mathf.PI) pos.X += Mathf.PI * 2;
        while (pos.X > Mathf.PI) pos.X -= Mathf.PI * 2;
        pos.Y = Mathf.Clamp(pos.Y, -1f, 1f);

        // Write position
        alien.Position = pos;

        // Update transform
        transform.position = alien.Position3D.ToVector3();

        // Reset View if moved
        if ( (pos - lastPos).LengthSquared < 0.1 ) {
            transform.LookAt(Vector3.zero);
        }
        lastPos = pos;
    }
}

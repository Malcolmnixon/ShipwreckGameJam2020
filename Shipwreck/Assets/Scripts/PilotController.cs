using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotController : MonoBehaviour
{
    /// <summary>
    /// Alien radius from ship
    /// </summary>
    public const float Radius = 10f; // easiest way to keep the same rotate feel as Astronaut regular

    private Vector2 _position;

    // Start is called before the first frame update
    void Start()
    {
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
        var pos = _position;

        // Update position
        pos.x += horizontal * 0.6f * Time.deltaTime;
        pos.y += vertical * 0.6f * Time.deltaTime;

        // Clamp position
        while (pos.x < -Mathf.PI) pos.x += Mathf.PI * 2;
        while (pos.x > Mathf.PI) pos.x -= Mathf.PI * 2;
        pos.y = Mathf.Clamp(pos.y, -1f, 1f);

        // Write position
        _position = pos;

        // Calculate 3D position from 2D
        var sinX = (float)System.Math.Sin(_position.x);
        var cosX = (float)System.Math.Cos(_position.x);
        transform.position = new Vector3(sinX * Radius, _position.y * Radius, -cosX * Radius);
        transform.LookAt(Vector3.zero);
    }
}

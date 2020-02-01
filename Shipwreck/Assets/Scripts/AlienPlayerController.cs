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

/*    void Update() {
 
        if (Input.GetMouseButton (0)) {

            var halfWidth = (Screen.width / 2);
            var halfHeight = (Screen.height / 2);

            var directionX = Input.mousePosition.x < halfWidth ? -1 : 1;
            var directionY = Input.mousePosition.y < halfHeight ? -1 : 1;

            rotate = (Input.mousePosition.x - halfWidth) / halfWidth;
            z = (Input.mousePosition.y - halfHeight) / halfHeight;
        } else {
            rotate = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }
        
        rotate = rotate * TurnSpeed * Time.deltaTime;
        z = z * WalkSpeed * Time.deltaTime; 
    }
    */

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

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

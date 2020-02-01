using Shipwreck.WorldData;
using UnityEngine;

public class AstronautPlayerController : MonoBehaviour
{
    public Astronaut Astronaut;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Astronaut == null)
            return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        // Get position
        var pos = Astronaut.Position;

        // Update position
        pos.X += horizontal * 0.6f * Time.deltaTime;
        pos.Y += vertical * 0.6f * Time.deltaTime;

        // Clamp position
        while (pos.X < -Mathf.PI) pos.X += Mathf.PI * 2;
        while (pos.X > Mathf.PI) pos.X -= Mathf.PI * 2;
        pos.Y = Mathf.Clamp(pos.Y, -1f, 1f);

        // Write position
        Astronaut.Position = pos;

        // Update transform
        transform.position = Astronaut.Position3D.ToVector3();
        transform.LookAt(Vector3.zero);
    }
}

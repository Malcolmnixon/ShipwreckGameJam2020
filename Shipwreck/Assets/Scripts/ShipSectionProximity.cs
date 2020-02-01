using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSectionProximity : MonoBehaviour
{

    [SerializeField]
    private cakeslice.Outline outline;

    public bool PlayerNear { get; private set; }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("Ship Section: Entered");
            PlayerNear = true;
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("Ship Section: Exited");
            PlayerNear = false;
            outline.enabled = false;
        }
    }
}

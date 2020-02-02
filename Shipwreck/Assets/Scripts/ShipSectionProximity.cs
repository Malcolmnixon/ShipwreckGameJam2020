using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSectionProximity : MonoBehaviour
{

    [SerializeField]
    private cakeslice.Outline outline;


    [SerializeField]
    public Renderer healthColored;

    public UnityEngine.UI.Text healthText;

    public GameObject smoking;

    public bool PlayerNear { get; private set; }

    public float health { get; private set; }

    private void Start()
    {
        PlayerNear = false;
        outline.enabled = false;
    }

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

    public void SetHealth(float amount)
    {
        health = amount;
        healthText.text = $"{amount:#}%";
        smoking.SetActive(health < 45f);
    }
}

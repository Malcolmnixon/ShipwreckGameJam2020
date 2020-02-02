using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public ShipSectionProximity[] wings;
    
    public ShipSectionProximity controlModule;

    public GameObject shield;
    
    public GameObject nameCanvas;
    
    public UnityEngine.UI.Text nameLabel; 


    public ShipSectionProximity GetNear() {
        if (controlModule.PlayerNear) {
            return controlModule;
        }
        foreach (var wing in wings) {
            if (wing.PlayerNear) {
                return wing;
            }
        }
        return null;
    }

    public bool isNearControlModule() => controlModule.PlayerNear;

    // 0 = none
    public int getNearWing() {
        for (var i = 0; i < wings.Length; i++) {
            if (wings[i].PlayerNear) {
                return i + 1;
            }
        }
        return 0;
    }

    public void UpdateHealth(float h1, float h2, float h3)
    {
        wings[0].SetHealth(h1);
        wings[1].SetHealth(h2);
        wings[2].SetHealth(h3);
    }

    public void SetSheilded(bool shielded, string name)
    {
        shield.SetActive(shielded);
        nameCanvas.transform.LookAt(Camera.main.transform);
        nameLabel.text = name;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public ShipSectionProximity[] wings;
    
    public ShipSectionProximity controlModule;


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
}

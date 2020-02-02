using Shipwreck.Math;
using Shipwreck.WorldData;
using UnityEngine;

public class AstronautController : MonoBehaviour
{

    public GameObject visibleAstronaut;

    public bool Piloting { get => visibleAstronaut.activeInHierarchy; set => visibleAstronaut.SetActive(!value); }
    
    public AudioSource sfx;

    public bool Healing
    {
        get => sfx.isPlaying; set
        {
            if (value) {
                sfx.UnPause();
             } else {
                //sfx.Pause();
                sfx.UnPause();
             }
        }
    }

}

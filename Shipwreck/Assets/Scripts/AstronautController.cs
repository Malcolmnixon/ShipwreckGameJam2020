using Shipwreck.Math;
using Shipwreck.WorldData;
using UnityEngine;
using UnityEngine.UI;

public class AstronautController : MonoBehaviour
{

    public GameObject visibleAstronaut;

    public bool Piloting { get => visibleAstronaut.activeInHierarchy; set => visibleAstronaut.SetActive(!value); }
    
    public Text nameLabel;

    public string Name { get => nameLabel.text; set => nameLabel.text = value; }
    
    public AudioSource sfx;

    public bool Healing
    {
        get => sfx.isPlaying; set
        {
            if (value) {
                sfx.UnPause();
             } else {
                sfx.Pause();
             }
        }
    }

}

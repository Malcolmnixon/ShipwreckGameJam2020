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

    public GameObject JetPack;


    public bool Healing
    {
        get => sfx.isPlaying; set
        {
            if (value) {
                JetPack.GetComponent<Renderer>().material.color = Color.green;
                sfx.UnPause();
             } else {
                JetPack.GetComponent<Renderer>().material.color = Color.red;
                sfx.Pause();
             }
        }
    }

}

using Shipwreck.Math;
using Shipwreck.WorldData;
using UnityEngine;

public class AstronautController : MonoBehaviour
{

    public GameObject visibleAstronaut;

    public bool Piloting { get => visibleAstronaut.activeInHierarchy; set => visibleAstronaut.SetActive(!value); }

}

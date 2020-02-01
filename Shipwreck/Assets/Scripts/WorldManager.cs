using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

    [Header("Resources")]

    public Transform WorldRoot;
    
    [Header("Prefabs")]
    
    [SerializeField]
    public GameObject PlayerAstronautPrefab;

    [SerializeField]
    public GameObject AstronautPrefab;

    [SerializeField]
    public GameObject  PlayerAlienPrefab;
    
    [Space]

    [SerializeField]
    public GameObject AsteroidPrefab;

    private GameObject _player;

    //private readonly Dictionary<Guid, GameObject> _remotePlayers = new Dictionary<Guid, GameObject>();

    //private readonly Dictionary<Guid, GameObject> _remoteMonster = new Dictionary<Guid, GameObject>();

    //private IWorld _world;


    // Start is called before the first frame update
    void Start()
    {
        // TODO
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

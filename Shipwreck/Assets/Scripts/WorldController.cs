using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shipwreck;
using System;
using System.Linq;

public class WorldController : MonoBehaviour
{

    [Header("Resources")]

    public Transform WorldRoot;
    
    [Header("Prefabs")]

    [SerializeField]
    public GameObject  PlayerAlienPrefab;
    
    [SerializeField]
    public GameObject PlayerAstronautPrefab;

    [Space]
    
    [SerializeField]
    public GameObject AstronautPrefab;

    [SerializeField]
    public GameObject AsteroidPrefab;

    private GameObject _player;

    private readonly Dictionary<Guid, GameObject> _remoteAstronauts = new Dictionary<Guid, GameObject>();

    private readonly Dictionary<Guid, GameObject> _remoteAsteroids = new Dictionary<Guid, GameObject>();

    private IWorld _world;


    // Start is called before the first frame update
    void Start()
    {
        CreateLocalWorld();
    }

    private void CreateLocalWorld()
    {
        _world = new Shipwreck.World.LocalWorld();
        _world.CreateLocalPlayer("name");
        _world.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

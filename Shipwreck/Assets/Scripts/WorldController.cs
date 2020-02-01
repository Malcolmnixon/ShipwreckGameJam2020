using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shipwreck;
using System;
using System.Linq;
using Shipwreck.WorldData;

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

    private readonly Dictionary<Guid, GameObject> _localAstronauts = new Dictionary<Guid, GameObject>();

    private readonly Dictionary<Guid, GameObject> _localAsteroids = new Dictionary<Guid, GameObject>();

    private IWorld _world;


    // Start is called before the first frame update
    void Start()
    {
        // Grab the world from the builder
        _world = GameObject.FindObjectOfType<WorldBuilder>().World;
    }

    // Update is called once per frame
    void Update()
    {
        // Request a state-update before drawing
        _world.Update();

        // Update the asteroids
        UpdateAsteroids(_world.State.Asteroids);
    }

    private void UpdateAsteroids(List<Asteroid> gameAsteroids)
    {
        // Update asteroids
        foreach (var gameAsteroid in gameAsteroids)
        {
            // Get the local asteroid
            if (!_localAsteroids.TryGetValue(gameAsteroid.Guid, out var localAsteroid))
            {
                // Create asteroid
                localAsteroid = Instantiate(AsteroidPrefab);
                _localAsteroids[gameAsteroid.Guid] = localAsteroid;
            }

            // Update asteroid
            localAsteroid.transform.position = gameAsteroid.Position.ToVector3();
        }

        // Remove deleted asteroids
        foreach (var oldAsteroid in _localAsteroids.Where(la => gameAsteroids.All(ga => ga.Guid != la.Key)).ToList())
        {
            Destroy(oldAsteroid.Value);
            _localAsteroids.Remove(oldAsteroid.Key);
        }
    }
}

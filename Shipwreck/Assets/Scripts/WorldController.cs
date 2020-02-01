using System.Collections.Generic;
using UnityEngine;
using Shipwreck;
using System;
using System.Linq;
using Shipwreck.World;
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

    [Header("Options")]

    [Range(-30f, 0f)]
    public float distanceFromAstronautPlayer = -15.0f;
    

    [Range(-30f, 0f)]
    public float distanceFromAlienPlayer = -1.0f;

    private GameObject _player;

    private GameObject AlienPlayerController;
    private readonly Dictionary<Guid, GameObject> _localAstronauts = new Dictionary<Guid, GameObject>();

    private readonly Dictionary<Guid, GameObject> _localAsteroids = new Dictionary<Guid, GameObject>();

    private IWorld _world;


    // Start is called before the first frame update
    void Start()
    {
        // Grab the world from the builder
        _world = GameObject.FindObjectOfType<WorldBuilder>().World;

        // Hack for debugging
        if (_world == null)
        {
            Debug.Log("Creating local World now...");
            _world = new LocalWorld();
            _world.Start();
            _world.Players.Players.Add(new Player { Guid = Guid.NewGuid() });
            _world.Players.Players.Add(new Player { Guid = Guid.NewGuid() });
            _world.CreateLocalPlayer("Test Player");
            _world.LocalPlayer.Type = PlayerType.Alien;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_world == null) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); // return to main
        }

        // Request a state-update before drawing
        _world.Update();

        // Update the asteroids
        UpdateAsteroids(_world.State.Asteroids);

        // Update the astronauts
        UpdateAstronauts(_world.State.Astronauts);

        UpdateAlien(_world.LocalPlayer.Type);
    }

    private void UpdateAlien(PlayerType type)
    {
        if (AlienPlayerController == null && type == PlayerType.Alien) {
            AlienPlayerController = Instantiate(PlayerAlienPrefab);
            AlienPlayerController.transform.SetParent(WorldRoot);
            Camera.main.transform.SetParent(AlienPlayerController.transform);
            Camera.main.transform.position = new Vector3(0,0,distanceFromAlienPlayer);
        } else if (AlienPlayerController != null && type != PlayerType.Alien) {
            Destroy(AlienPlayerController);
        }
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

            // Fade asteroid
            var dist = gameAsteroid.Position.Length;
            var meshRenderer = localAsteroid.GetComponent<MeshRenderer>();
            var material = meshRenderer.material;
            var color = material.color;
            color.a = dist > 100f ? 0 : dist < 70f ? 1f : (100f - dist) / 30f;
            material.color = color;
        }

        // Remove deleted asteroids
        foreach (var oldAsteroid in _localAsteroids.Where(la => gameAsteroids.All(ga => ga.Guid != la.Key)).ToList())
        {
            Destroy(oldAsteroid.Value);
            _localAsteroids.Remove(oldAsteroid.Key);
        }
    }

    private void UpdateAstronauts(List<Astronaut> gameAstronauts)
    {
        // Update the astronauts
        foreach (var gameAstronaut in gameAstronauts)
        {
            // Get the local astronaut
            if (!_localAstronauts.TryGetValue(gameAstronaut.Guid, out var localAstronaut))
            {
                if (_world.LocalAstronaut?.Guid == gameAstronaut.Guid)
                {
                    // Build a local player astronaut prefab
                    localAstronaut = Instantiate(PlayerAstronautPrefab);
                    var controller = localAstronaut.GetComponent<AstronautPlayerController>();
                    controller.Astronaut = _world.LocalAstronaut;

                    Camera.main.transform.SetParent(controller.transform);
                    Camera.main.transform.position = new Vector3(0,0,distanceFromAstronautPlayer);
                }
                else
                {
                    // Create remotely controlled astronaut
                    localAstronaut = Instantiate(AstronautPrefab);
                }

                // Add to dictionary
                _localAstronauts[gameAstronaut.Guid] = localAstronaut;
            }

            // Update astronaut
            localAstronaut.transform.position = gameAstronaut.Position3D.ToVector3();
        }

        // Remove deleted astronauts
        foreach (var oldAstronaut in _localAstronauts.Where(la => gameAstronauts.All(ga => ga.Guid != la.Key)).ToList())
        {
            Destroy(oldAstronaut.Value);
            _localAstronauts.Remove(oldAstronaut.Key);
        }
    }
}

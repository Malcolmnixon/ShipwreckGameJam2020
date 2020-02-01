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

    public ShipController ship;
    
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

    private GameObject _astronautPlayerControl;

    private GameObject _alienPlayerController;

    private readonly Dictionary<Guid, GameObject> _localAstronauts = new Dictionary<Guid, GameObject>();

    private readonly Dictionary<Guid, GameObject> _localAsteroids = new Dictionary<Guid, GameObject>();

    private IWorld _world;

    /// <summary>
    /// Type the player was last
    /// </summary>
    private PlayerType _lastPlayerType = PlayerType.None;


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
        if (_world == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); // return to main
            return;
        }

        // Request a state-update before drawing
        _world.Update();

        // Update the asteroids
        UpdateAsteroids();

        // Update the other astronauts
        UpdateOtherAstronauts();

        // Update the local player
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        // Get the current player type
        var playerType = _world.LocalPlayer.Type;

        // Delete alien player controller if no-longer needed
        if (_alienPlayerController != null && (playerType != PlayerType.Alien && playerType != PlayerType.Observer))
        {
            Camera.main.transform.SetParent(WorldRoot);
            Destroy(_alienPlayerController);
            _alienPlayerController = null;
        }

        // Delete astronaut player controller if no-longer needed
        if (_astronautPlayerControl != null && playerType != PlayerType.Astronaut)
        {
            Camera.main.transform.SetParent(WorldRoot);
            Destroy(_astronautPlayerControl);
            _astronautPlayerControl = null;
        }

        // Create alien player if needed
        if (_alienPlayerController == null && (playerType == PlayerType.Alien || playerType == PlayerType.Observer))
        {
            _alienPlayerController = Instantiate(PlayerAlienPrefab);
            _alienPlayerController.transform.SetParent(WorldRoot);

            Camera.main.transform.SetParent(_alienPlayerController.transform);
            Camera.main.transform.position = new Vector3(0, 0, distanceFromAlienPlayer);
            Camera.main.transform.LookAt(Vector3.zero);
        }

        // Create astronaut player controller if needed
        if (_astronautPlayerControl == null && playerType == PlayerType.Astronaut)
        {
            _astronautPlayerControl = Instantiate(PlayerAstronautPrefab);
            _astronautPlayerControl.transform.SetParent(WorldRoot);

            Camera.main.transform.SetParent(_astronautPlayerControl.transform);
            Camera.main.transform.position = new Vector3(0, 0, distanceFromAstronautPlayer);
            Camera.main.transform.LookAt(Vector3.zero);
        }

        // Handle alien actions
        if (_alienPlayerController != null && playerType == PlayerType.Alien)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                _world.FireAsteroid(
                    _alienPlayerController.transform.position.ToVec3(),
                    _alienPlayerController.transform.TransformVector(Vector3.forward * 10).ToVec3()
                );
            }
        }

        // Handle astronaut actions
        if (_astronautPlayerControl != null)
        {
            // Read 2D position from controller
            var controller = _astronautPlayerControl.GetComponent<AstronautPlayerController>();
            _world.LocalAstronaut.Position = controller.Position;

            // Update 3D position from 2D position
            _astronautPlayerControl.transform.position = Astronaut.To3DPosition(_world.LocalAstronaut.Position).ToVector3();
            _astronautPlayerControl.transform.LookAt(Vector3.zero);

            int wingNear = ship.getNearWing();
            if (_world.State.Ship.Pilot == _world.LocalPlayer.Guid)
            {
                if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Cancel"))
                {
                    LeavePilot();
                }
                else if (Input.GetButton("Fire1"))
                {
                    ActivateSheilds();
                }
            }
            else if (Input.GetButton("Fire1") && wingNear > 0)
            {
                HealWing(wingNear);
            }
            else if (Input.GetButtonDown("Fire1") && ship.isNearControlModule())
            {
                EnterPilot();
            }
        }
    }

    private void EnterPilot()
    {
        Debug.LogWarning("Piloting Not Implemented.");
    }

    private void LeavePilot()
    {
        Debug.LogWarning("Piloting Not Implemented.");
    }

    private void ActivateSheilds()
    {
        Debug.LogWarning("Sheilds Not Implemented.");
    }

    private void HealWing(int wingNear)
    {
        Debug.LogWarning("Healing Not Implemented.");
    }

    private void UpdateAsteroids()
    {
        // Get the asteroids
        var gameAsteroids = _world.State.Asteroids;

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
            color.a = dist > 100f ? 0 : dist < 90f ? 1f : (100f - dist) / 10f;
            material.color = color;
        }

        // Remove deleted asteroids
        foreach (var oldAsteroid in _localAsteroids.Where(la => gameAsteroids.All(ga => ga.Guid != la.Key)).ToList())
        {
            Destroy(oldAsteroid.Value);
            _localAsteroids.Remove(oldAsteroid.Key);
        }
    }

    private void UpdateOtherAstronauts()
    {
        // Get the other astronauts
        var otherAstronauts = _world.OtherAstronauts;

        // Update the astronauts
        foreach (var gameAstronaut in otherAstronauts)
        {
            // Get the local astronaut
            if (!_localAstronauts.TryGetValue(gameAstronaut.Guid, out var localAstronaut))
            {
                // Create remotely controlled astronaut
                localAstronaut = Instantiate(AstronautPrefab);

                // Add to dictionary
                _localAstronauts[gameAstronaut.Guid] = localAstronaut;
            }

            // Update astronaut
            localAstronaut.transform.position = Astronaut.To3DPosition(gameAstronaut.Position).ToVector3();
            localAstronaut.transform.LookAt(Vector3.zero);
        }

        // Remove deleted astronauts
        foreach (var oldAstronaut in _localAstronauts.Where(la => otherAstronauts.All(ga => ga.Guid != la.Key)).ToList())
        {
            Destroy(oldAstronaut.Value);
            _localAstronauts.Remove(oldAstronaut.Key);
        }
    }
}

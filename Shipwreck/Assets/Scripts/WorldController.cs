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
    public GameObject PlayerAlienPrefab;

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

    [Range(-50f, 0f)]
    public float distanceFromPilotPlayer = -25.0f;


    [Range(-30f, 0f)]
    public float distanceFromAlienPlayer = -1.0f;


    [Range(0.001f, 10f)]
    public float AsteroidFireWaitTotal = 1.0f;

    [Range(1f, 10f)]
    public float SheildSecondsTotal = 4.0f;

    [Range(0.001f, 10f)]
    public float SheildDelayTotal = 1.0f;


    public float _sheildSeconds = 4.0f;

    public float _sheildDelay = 0.0f;

    public float _asteroidFireWait = 1.0f;

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
            _world.CreateLocalPlayer("Test Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If waiting then go back to waiting screen
        if (_world.State.Mode == GameMode.Waiting)
        {
            _world.LocalPlayer.Type = PlayerType.None;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            return;
        } 

        // If game finishes then go to results screen
        if (_world.State.Mode == GameMode.Finished)
        {
            _world.LocalPlayer.Type = PlayerType.None;
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
            return;
        }

        // Request a state-update before drawing
        _world.Update();

        // Update the asteroids
        UpdateAsteroids();

        // Update the other astronauts
        UpdateOtherAstronauts();

        // Update the Ship Health / Sheild
        UpdateShip();

        // Update the local player
        UpdatePlayer();
    }

    private void UpdateShip()
    {
        ship.UpdateHealth(
            _world.State.Ship.Wing1Health,
            _world.State.Ship.Wing2Health,
            _world.State.Ship.Wing3Health
        );

        var pilotAstronaut = _world.State.Astronauts.FirstOrDefault(a => a.Mode  == AstronautMode.Pilot || a.Mode  == AstronautMode.PilotShielding);
        var pilotPlayer = _world.Players.Players.FirstOrDefault(p => p.Guid == ( pilotAstronaut?.Guid ?? Guid.Empty ));
        ship.SetSheilded(_world.State.Ship.Shielded, pilotPlayer?.Name ?? String.Empty);

        if (_world.State.Ship.Shielded && _sheildSeconds > 0)
        {
            _sheildSeconds -= Time.deltaTime;
        }
        else if (_sheildSeconds < SheildSecondsTotal)
        {
            _sheildSeconds += Time.deltaTime;
        }
        _sheildDelay -= Time.deltaTime;
        ship.SetSheildText(100 * _sheildSeconds / SheildSecondsTotal, _sheildSeconds < SheildSecondsTotal);
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
            _asteroidFireWait -= Time.deltaTime;

            if ((InputManagerData.PrimaryButtonClicked || InputManagerData.SecondaryButtonClicked) && 
                _asteroidFireWait <= 0f &&
                _world.State.Asteroids.Count < GameConstants.MaxAsteroidCount)
            {
                _world.FireAsteroid(
                    _alienPlayerController.transform.position.ToVec3(),
                    _alienPlayerController.transform.TransformVector(Vector3.forward * 10).ToVec3()
                );
                var controller = _alienPlayerController.GetComponent<AlienPlayerController>();
                controller.fireSfx.Play();
                _asteroidFireWait = AsteroidFireWaitTotal;
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

            switch (_world.LocalAstronaut.Mode)
            {
                case AstronautMode.Astronaut:
                    // Check controls
                    if (InputManagerData.PrimaryButtonDown && ship.isNearControlModule())
                    {
                        _world.LocalAstronaut.Mode = AstronautMode.Pilot;
                    }
                    else if (InputManagerData.PrimaryButtonDown && wingNear > 0)
                    {
                        _world.LocalAstronaut.Mode = AstronautMode.AstronautHealing;
                    }
                    break;

                case AstronautMode.Pilot:
                    // Check controls
                    if (InputManagerData.SecondaryButtonDown || Input.GetButtonDown("Cancel"))
                    {
                        _world.LocalAstronaut.Mode = AstronautMode.Astronaut;
                    }
                    else if (InputManagerData.PrimaryButtonDown && _sheildDelay < 0f)
                    {
                        _world.LocalAstronaut.Mode = AstronautMode.PilotShielding;
                    }
                    break;

                case AstronautMode.AstronautHealing:
                    // Check controls
                    if (!InputManagerData.PrimaryButtonDown)
                    {
                        _world.LocalAstronaut.Mode = AstronautMode.Astronaut;
                    }
                    break;

                case AstronautMode.PilotShielding:
                    // Check controls
                    if (!InputManagerData.PrimaryButtonDown || _sheildSeconds < 0)
                    {
                        _world.LocalAstronaut.Mode = AstronautMode.Pilot;
                        _sheildDelay = SheildDelayTotal;
                    }
                    break;
            }

            controller.Piloting = _world.LocalAstronaut.Mode == AstronautMode.Pilot ||
                                  _world.LocalAstronaut.Mode == AstronautMode.PilotShielding;
            controller.Healing = _world.LocalAstronaut.Mode == AstronautMode.AstronautHealing;
            controller.Name = _world.LocalPlayer.Name;
            // TODO camera
        }
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

            var controller = localAstronaut.GetComponent<AstronautController>();
            controller.Piloting = gameAstronaut.Mode == AstronautMode.Pilot
                                || gameAstronaut.Mode == AstronautMode.PilotShielding;
            controller.Healing = gameAstronaut.Mode == AstronautMode.AstronautHealing;
            controller.Name = _world.Players.Players.FirstOrDefault(p => p.Guid == gameAstronaut.Guid)?.Name ?? string.Empty;
        }

        // Remove deleted astronauts
        foreach (var oldAstronaut in _localAstronauts.Where(la => otherAstronauts.All(ga => ga.Guid != la.Key)).ToList())
        {
            Destroy(oldAstronaut.Value);
            _localAstronauts.Remove(oldAstronaut.Key);
        }
    }
}

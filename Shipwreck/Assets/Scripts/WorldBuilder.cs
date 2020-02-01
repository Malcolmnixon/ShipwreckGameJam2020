using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shipwreck;
using Shipwreck.World;

public class WorldBuilder : MonoBehaviour
{
    
    public bool active { get { return _active; } }

    private bool _active = false;

    public IWorld World;

    // Start is called before the first frame update
    void Awake()
    {
        var gameDataManagers = GameObject.FindObjectsOfType<WorldBuilder>();
        if (gameDataManagers.Length > 1) {
            Destroy(this.gameObject);
        }

        _active = true;

        DontDestroyOnLoad(this.gameObject);

        // Start a new local world with a bogus local player
        World = new LocalWorld();
        World.Start();
        World.CreateLocalPlayer("Bob");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

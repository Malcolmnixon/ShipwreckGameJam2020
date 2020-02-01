using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shipwreck;
using System;
using UnityEngine.UI;
using Shipwreck.World;
using Shipwreck.WorldLan;

public class WorldBuilder : MonoBehaviour
{
    
    public bool active { get { return _active; } }

    private bool _active = false;

    public LanDiscovery discovery;

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
        
        discovery = new LanDiscovery();
        discovery.Start();
    }

    public Dictionary<string, System.Net.IPAddress> GetGames() => discovery.GameServers;

    public void CreatePrivate()
    {
        World = new LocalWorld();
        World.Start();
    }

    public void CreateLAN(Guid guid)
    {
        World = new LanServerWorld(guid.ToString());
        World.Start();
    }

    public void JoinLAN(System.Net.IPAddress server)
    {
        World = new LanClientWorld(server);
        World.Start();
    }

    public void NewPlayer(string name) 
    {
        World.CreateLocalPlayer(name);

    }
    
    public void LeaveWorld() 
    {
        World.Dispose();
        World = null;
    }

    public bool HasWorld()
    {
        return World != null;
    }

}

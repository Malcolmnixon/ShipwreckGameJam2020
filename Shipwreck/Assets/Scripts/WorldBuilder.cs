using System.Collections.Generic;
using UnityEngine;
using Shipwreck;
using System;
using Shipwreck.World;
using Shipwreck.WorldLan;
using Shipwreck.WorldWeb;

public class WorldBuilder : MonoBehaviour
{
    public Dictionary<string, System.Net.IPAddress> GetGames() => _discovery.GameServers;

    public IWorld World { get; private set; }

    private readonly LanDiscovery _discovery = new LanDiscovery();

    // Start is called before the first frame update
    void Awake()
    {
        // Find if a world builder has already been built
        var gameDataManagers = GameObject.FindObjectsOfType<WorldBuilder>();
        if (gameDataManagers.Length > 1) {
            // Kill this duplicate
            Destroy(gameObject);
            return;
        }

        // Make this WorldBuilder immortal between scene switches
        DontDestroyOnLoad(gameObject);

        // Attach logging
        Shipwreck.Logger.OnLog += (sender, args) => Debug.Log(args.Message);
        //NetComms.Logger.OnLog += (sender, args) => Debug.Log(args.Message);

        // Start discovery
        _discovery.Start();
    }

    public void CreatePrivate()
    {
        LeaveWorld();
        World = new LocalWorld();
        World.Start();
    }

    public void CreateLAN(Guid guid)
    {
        LeaveWorld();
        World = new LanServerWorld(guid.ToString());
        World.Start();
    }

    public void CreateWeb()
    {
        LeaveWorld();
        World = new WebClientWorld();
        World.Start();
    }

    public void JoinLAN(System.Net.IPAddress server)
    {
        LeaveWorld();
        World = new LanClientWorld(server);
        World.Start();
    }

    public void NewPlayer(string playerName)
    {
        World?.CreateLocalPlayer(playerName);
    }
    
    public void LeaveWorld() 
    {
        // Skip if no world
        if (World == null)
            return;

        // Dispose of existing world
        World.Dispose();
        World = null;
    }

    public bool HasWorld()
    {
        return World != null;
    }

}

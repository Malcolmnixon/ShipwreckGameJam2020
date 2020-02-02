using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shipwreck;
using System;
using UnityEngine.UI;
using Shipwreck.World;
using Shipwreck.WorldLan;
using Shipwreck.WorldWeb;

public class WorldBuilder : MonoBehaviour
{
    public readonly LanDiscovery discovery = new LanDiscovery();

    public Dictionary<string, System.Net.IPAddress> GetGames() => discovery.GameServers;

    public IWorld World;

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
        discovery.Start();
    }

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

    public void CreateWeb()
    {
        World = new WebClientWorld();
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

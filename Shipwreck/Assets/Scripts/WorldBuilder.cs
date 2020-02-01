using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shipwreck;
using System;
using UnityEngine.UI;
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
    }

    public void CreatePrivate()
    {
        World = new LocalWorld();
        World.Start();
    }

    public void CreateLAN(Guid guid)
    {
        //World = new LanServerWorld();
        //World.Start();
    }

    public void JoinLAN(Text itemText)
    {
        //World = new RemoteWorld();
        //World.Start();
    }

    public void NewPlayer(string name) 
    {
        World.CreateLocalPlayer(name);

    }
    
    public void LeaveWorld() 
    {
        throw new NotImplementedException();

    }

    public bool HasWorld()
    {
        return World != null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shipwreck;
using System;
using UnityEngine.UI;

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
        throw new NotImplementedException();
    }

    public void CreateLAN(Guid guid)
    {
        throw new NotImplementedException();
    }

    public void JoinLAN(Text itemText)
    {
        throw new NotImplementedException();
    }

    public void NewPlayer(string name) 
    {
        throw new NotImplementedException();

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shipwreck;

public class ResultScreenManager : MonoBehaviour
{
    
	public IWorld _world;
	public Text resultText;
    
	public Text countdownText;


    // Start is called before the first frame update
    void Start()
    {
        // Grab the world from the builder
        _world = GameObject.FindObjectOfType<WorldBuilder>().World;
        
        var result = _world.State.Ship.TotalHealth > 50.0f ? "survived" : "been destroyed";
        resultText.text = $"Result:\nThe Station has {result}";
    }

    // Update is called once per frame
    void Update()
    {
        countdownText.text = $"Loading in {_world.State.RemainingTime:#}s";

        if (_world.State.Mode != Shipwreck.WorldData.GameMode.Finished) 
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); // return to main
            return;
        }
    }
}

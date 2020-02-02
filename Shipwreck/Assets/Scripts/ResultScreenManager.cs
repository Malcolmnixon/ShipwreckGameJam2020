using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreenManager : MonoBehaviour
{
    
	public WorldBuilder worldInfo;
	public Text resultText;
    
	public Text countdownText;


    // Start is called before the first frame update
    void Start()
    {
        var totalHealth = worldInfo.World.State.Ship.CenterTorsoHealth 
            + worldInfo.World.State.Ship.LeftWingHealth 
            + worldInfo.World.State.Ship.RightWingHealth;
        var result = totalHealth > 50.0f ? "survived" : "been destroyed";
        resultText.text = $"Result:\nThe Station has {result}";
    }

    // Update is called once per frame
    void Update()
    {
        countdownText.text = $"Loading in {worldInfo.World.State.RemainingTime:#}s";

        if (worldInfo.World.State.Mode != Shipwreck.WorldData.GameMode.Finished) 
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); // return to main
            return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PushToSkip : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey || Input.touchCount > 1)
        {
            SceneManager.LoadScene(1);
        }
    }
}

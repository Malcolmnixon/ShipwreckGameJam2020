using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileQuit : MonoBehaviour
{
    void Awake()
    {
        var mobileQuitter = GameObject.FindObjectsOfType<MobileQuit>();
        if (mobileQuitter.Length > 1) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
		#if !UNITY_STANDALONE && !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape)) // Android Back
        {
            Application.Quit();
        }
		#endif
    }
}

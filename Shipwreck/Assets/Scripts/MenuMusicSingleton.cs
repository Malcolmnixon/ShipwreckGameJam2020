using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicSingleton : MonoBehaviour
{
    void Awake()
    {
        var menuMusics = GameObject.FindObjectsOfType<MenuMusicSingleton>();
        if (menuMusics.Length > 1) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}

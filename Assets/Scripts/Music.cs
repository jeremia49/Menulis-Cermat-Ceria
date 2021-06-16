using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    void Awake()
    {
        if (GameObject.FindObjectsOfType<AudioSource>().Length > 1)
        {
            Destroy(this.gameObject);
        }

        string enableSound = PlayerPrefs.GetString("sound", "yes");

        if (GameObject.FindObjectsOfType<AudioSource>().Length == 1)
        {
            DontDestroyOnLoad(transform.gameObject);
            if (enableSound == "yes")
            {
                transform.GetComponent<AudioSource>().Play();
            }
        }
    }
}

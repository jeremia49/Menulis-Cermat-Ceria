using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSettings : MonoBehaviour
{

    public Button button;
    public Sprite imageon;
    public Sprite imageoff;


    void Start()
    {
        

        string enableSound = PlayerPrefs.GetString("sound", "yes");
        if (enableSound == "yes")
        {
            button.GetComponent<Image>().sprite = imageon;
        }
        else
        {
            button.GetComponent<Image>().sprite = imageoff;
        }
    }

    public void toggleMusic()
    {
        string enableSound = PlayerPrefs.GetString("sound", "yes");
        if (enableSound == "yes")
        {
            PlayerPrefs.SetString("sound", "no");
            button.GetComponent<Image>().sprite = imageoff;
            GameObject.FindObjectOfType<AudioSource>().gameObject.GetComponent<AudioSource>().Pause();
        }
        else
        {
            PlayerPrefs.SetString("sound", "yes");
            button.GetComponent<Image>().sprite = imageon;
            GameObject.FindObjectOfType<AudioSource>().gameObject.GetComponent<AudioSource>().Play();
        }
    }
}

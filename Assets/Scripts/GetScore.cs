using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GetScore : MonoBehaviour
{
    public TextMeshProUGUI touchText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public Sprite image;

    public string level;
    public float score;

    // Start is called before the first frame update
    void Start()
    {
        level = PlayerPrefs.GetString("level");
        score = PlayerPrefs.GetFloat("score");

        levelText.text = level;
        scoreText.text = score.ToString() + " / 100";

        touchText.text = "Sentuh dimanapun untuk lanjut ke " + getNextLevel(level);

        if (score < 10)
        {
            return;
        }
        else if(score < 30)
        {
            star1.GetComponent<Image>().sprite = image;
        }else if(score < 70)
        {
            star1.GetComponent<Image>().sprite = image;
            star2.GetComponent<Image>().sprite = image;
        }
        else
        {
            star1.GetComponent<Image>().sprite = image;
            star2.GetComponent<Image>().sprite = image;
            star3.GetComponent<Image>().sprite = image;

        }
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            SceneManager.LoadSceneAsync(getNextLevel(level));
        }
    }

    private string getNextLevel(string current)
    {
        int level1 = 4;
        int level2 = 62;
        int level3 = 3;


        var splitted = current.Split('-');
        if (splitted[0] == "1")
        {
            if(Int32.Parse(splitted[1]) < level1)
            {
                return splitted[0] + "-" + (Int32.Parse(splitted[1]) + 1);
            }
            else
            {
                return "Level Sedang";
            }
        }else if (splitted[0] == "2")
        {
            if (Int32.Parse(splitted[1]) < level2)
            {
                return splitted[0] + "-" + (Int32.Parse(splitted[1]) + 1);
            }
            else
            {
                return "Level Sulit";
            }
        }
        else if (splitted[0] == "3")
        {
            if (Int32.Parse(splitted[1]) < level3)
            {
                return splitted[0] + "-" + (Int32.Parse(splitted[1]) + 1);
            }
            else
            {
                return "MenuUtama";
            }
        }

        return "MenuUtama";
    }
}

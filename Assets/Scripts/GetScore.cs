using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GetScore : MonoBehaviour
{
  
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public Sprite image;


    // Start is called before the first frame update
    void Start()
    {
        var level = PlayerPrefs.GetString("level");
        var score = PlayerPrefs.GetFloat("score");

        levelText.text = level;
        scoreText.text = score.ToString() + " / 100";

        if(score < 10)
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
}

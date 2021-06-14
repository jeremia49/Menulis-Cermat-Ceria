using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GetScore : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        var level = PlayerPrefs.GetString("level");
        var score = PlayerPrefs.GetFloat("score");

        levelText.text = level;
        scoreText.text = score.ToString() + " / 100";
    }
}

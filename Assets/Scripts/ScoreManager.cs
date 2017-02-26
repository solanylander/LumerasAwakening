using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    private int currentScore;
    private int maxScore;
    private float nextDecrement;
    [SerializeField]
    private Text scoreText;

    void Start()
    {
        maxScore = 1000;
        currentScore = 1000;
        nextDecrement = Time.time;
        scoreText = GetComponent<Text>();
    }

    void Update()
    {
        if (Time.time > nextDecrement)
        {
            decrementScore(1);
            nextDecrement = Time.time + 1.0f;
        }
    }

    public void decrementScore(int amount)
    {
        currentScore -= amount;
        scoreText.text = currentScore.ToString();
    }
}

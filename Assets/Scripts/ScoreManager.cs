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
    public GameObject scoreFlash;
    public static int highScore;
    public Text highScoreText;

    void Start()
    {
        maxScore = 1000;
        currentScore = 1000;
        nextDecrement = Time.time;
        scoreText = GetComponent<Text>();
        highScoreText = GetComponent<Text>();
        highScore = PlayerPrefs.GetInt("highScore", highScore);
    }

    void Update()
    {
        if (Time.time > nextDecrement)
        {
            decrementScore(1);
            nextDecrement = Time.time + 1.0f;
        }

        if (currentScore > highScore)
        {
            highScore = currentScore;
            highScoreText.GetComponent<Text>().text = "Best: " + currentScore;

            PlayerPrefs.SetInt("highScore", highScore);
        }
        //float t = Mathf.PingPong(Time.time, 0.25f) / 0.25f;
        //scoreFlash.transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, t);
        //if (scoreFlash.transform.localScale.Equals(Vector3.zero))
        //{
        //    scoreFlash.SetActive(false);
        //    scoreFlash.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        //}
    }

    public void decrementScore(int amount)
    {
        //if (amount >= 10)
        //{
        //    scoreFlash.GetComponent<Text>().text = (-amount).ToString();
        //    scoreFlash.SetActive(true);
        //}
        currentScore -= amount;
        scoreText.text = currentScore.ToString();
    }
}

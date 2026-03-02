using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    Board board;
    public TMP_Text timerText;
    public TMP_Text levelText;

    // Game Over Screen UI
    public TMP_Text finalTimeText;
    public TMP_Text finalLevelText;

    public GameObject gameOverScreen;

    public int currentLevel = 1;
    public float timeLimit = 20f; // Time limit in seconds
    public float timeRemaining;
    public bool timerIsRunning = false;
    private float totalTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        gameOverScreen.SetActive(false);
        timeRemaining = timeLimit;
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                totalTime += Time.deltaTime;
                timeRemaining -= Time.deltaTime;
                timerText.text = "TIME LEFT: " + Mathf.Ceil(timeRemaining).ToString();
                gameOverScreen.SetActive(false);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                gameOverScreen.SetActive(true);

            }
        }
        else if (board.gameOver)
        {
            finalTimeText.text = "TIME PLAYED: " + totalTime.ToString("F2");
            finalLevelText.text = "FINAL LEVEL: " + currentLevel.ToString();
            gameOverScreen.SetActive(true);
        }

    }
}

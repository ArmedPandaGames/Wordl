using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    Board board;
    WordPicker wordPicker;
    public TMP_Text timerText;
    public TMP_Text levelText;

    // Game Over Screen UI
    public TMP_Text finalTimeText;
    public TMP_Text finalLevelText;
    public TMP_Text SolutionText;

    public GameObject gameOverScreen;

    public int currentLevel = 1;
    public float timeLimit = 20f; // Time limit in seconds
    public float timeRemaining;
    public bool timerIsRunning = false;
    private float totalTime = 0f;

    public bool isPaused = false;
    public GameObject pauseMenu;
    public bool useTimer = true;


    // Start is called before the first frame update
    void Start()
    {
        useTimer = true;
        board = FindObjectOfType<Board>();
        wordPicker = FindObjectOfType<WordPicker>();
        gameOverScreen.SetActive(false);
        pauseMenu.SetActive(false);
        timeRemaining = timeLimit;
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning && useTimer)
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
                SoundPlayer.Instance.PlayOneShot("gameover");

            }
        }
        else if (board.gameOver)
        {
            finalTimeText.text = "TIME PLAYED: " + totalTime.ToString("F2");
            finalLevelText.text = "FINAL LEVEL: " + currentLevel.ToString();
            SolutionText.text = "SOLUTION: " + wordPicker.solution.ToUpper();
            gameOverScreen.SetActive(true);
            //SoundPlayer.Instance.PlayOneShot("gameover");
            return;

        }

    }

    public void PauseGame()
    {
        SoundPlayer.Instance.PlayOneShot("menu");
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void UseTimerToggle()
    {
        SoundPlayer.Instance.PlayOneShot("toggle");
        useTimer = !useTimer;
        if (useTimer)
        {
            timeRemaining = timeLimit;
            timerIsRunning = true;
        }
        else
        {
            timerIsRunning = false;
            timerText.text = "TIMER OFF";
        }
    }
}

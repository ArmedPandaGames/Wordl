using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    WordPicker wp;
    GameManager gm;
    KeyBoard kb;
    InfoTextManager infoTextManager;
    ParticlesManager[] pm;
    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E,
        KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
        KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O,
        KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
        KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y,
        KeyCode.Z
    };

    private Row[] rows;

    public bool gameOver = false;
    private bool gameWon = false;
    private int rowIndex = 0;
    private int columnIndex = 0;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
        wp = FindObjectOfType<WordPicker>();
        gm = FindObjectOfType<GameManager>();
        kb = FindObjectOfType<KeyBoard>();
        infoTextManager = FindObjectOfType<InfoTextManager>();
        pm = FindObjectsOfType<ParticlesManager>();
    }

    private void Update()
    {
        if (gm.timerIsRunning == false)
        {
            gameOver = true;
            return; // Don't process input if the timer isn't running
        }
        if (rows == null || rows.Length == 0)
        {
            Debug.LogWarning("Board has no rows initialized.");
            return;
        }

        if (rowIndex < 0) rowIndex = 0;
        if (rowIndex >= rows.Length) rowIndex = rows.Length - 1;

        Row currentRow = rows[rowIndex];
        if (currentRow == null || currentRow.tiles == null || currentRow.tiles.Length == 0)
        {
            Debug.LogWarning("Current row or its tiles are not initialized.");
            return;
        }
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (columnIndex > 0)
                {
                    columnIndex--;
                    if (columnIndex < currentRow.tiles.Length)
                        currentRow.tiles[columnIndex].SetLetter('\0');
                }
            }
            else if (columnIndex >= currentRow.tiles.Length)
            {
                // submit
                if (Input.GetKeyDown(KeyCode.Return) && gameOver == false)
                {
                    submitRow(currentRow);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return) && gameOver == false)
                {
                    infoTextManager.ChooseText(InfoTextState.NotEnoughLetters);
                    return;
                }

                for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
                {
                    if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
                    {
                        if (columnIndex < currentRow.tiles.Length)
                        {
                            currentRow.tiles[columnIndex].SetLetter((char)SUPPORTED_KEYS[i]);
                            columnIndex++;
                        }
                        break;
                    }
                }
            }
        }
    }

    public void submitRow(Row row)
    {

        // Validate that WordPicker and solution are ready
        if (wp == null || string.IsNullOrEmpty(wp.solution))
        {
            Debug.LogError("WordPicker or solution not initialized!");
            return;
        }

        if (row == null || row.tiles.Length == 0)
        {
            Debug.LogError("Invalid row!");
            return;
        }

        if (columnIndex >= row.tiles.Length)
        {


            // Validate the guess against the word list
            string guess = getCurrentGuess();
            bool isValidWord = false;
            foreach (string word in wp.wordList.words)
            {
                if (word == guess)
                {
                    Debug.Log("Valid guess: " + guess);
                    isValidWord = true;
                    break;
                }
            }

            if (isValidWord)
            {

                for (int i = 0; i < row.tiles.Length; i++)
                {
                    char guessedLetter = row.tiles[i].Letter;
                    char solutionLetter = wp.solution[i];

                    if (guessedLetter == solutionLetter)
                    {
                        // Correct letter in the correct position
                        row.tiles[i].GetComponent<Image>().color = new Color(0.4f, 0.65f, 0.3f, 0.8f);
                        kb.SetKeyState(guessedLetter, KeyState.Correct);
                        gm.timeRemaining += 2f; // Add 2 seconds for each correct letter in the correct position
                    }
                    else if (wp.solution.Contains(guessedLetter.ToString()) && guessedLetter != solutionLetter)
                    {
                        // Correct letter in the wrong position
                        row.tiles[i].GetComponent<Image>().color = new Color(0.8f, 0.65f, 0.3f, 0.8f);
                        kb.SetKeyState(guessedLetter, KeyState.WrongPosition);
                        gm.timeRemaining += 1f; // Add 1 second for each correct letter in the wrong position
                    }
                    else
                    {
                        // Incorrect letter
                        row.tiles[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
                        kb.SetKeyState(guessedLetter, KeyState.Wrong);
                    }
                }
            }
            else
            {
                Debug.Log("Invalid guess: " + guess);
                infoTextManager.ChooseText(InfoTextState.WordNotFound);
                isValidWord = false;
                // provide feedback to the player about the invalid guess
            }
            if (row.tiles[0].Letter == wp.solution[0] &&
                row.tiles[1].Letter == wp.solution[1] &&
                row.tiles[2].Letter == wp.solution[2] &&
                row.tiles[3].Letter == wp.solution[3] &&
                row.tiles[4].Letter == wp.solution[4])
            {
                OnWin();
                return;
            }

            // Not a correct guess
            // If this was the last row, it's game over
            if (rowIndex >= rows.Length - 1 && isValidWord)
            {
                gameOver = true;
                gm.timerIsRunning = false;
                Debug.Log("Game Over! The correct word was: " + wp.solution);
                return;
            }

            if (isValidWord)
            {
                // Advance to next row and reset column index for next guess
                rowIndex++;
                columnIndex = 0;
            }
        }
    }

    // Public method for UI Button OnClick to submit the current row
    public void SubmitCurrentRow()
    {
        if (gameOver) return;
        if (rows == null || rows.Length == 0) return;
        Row currentRow = rows[rowIndex];
        if (currentRow == null) return;
        if (columnIndex < currentRow.tiles.Length)
        {
            infoTextManager.ChooseText(InfoTextState.NotEnoughLetters);
            return;
        }
        submitRow(currentRow);
    }

    public void Backspace()
    {
        if (gameOver) return;
        if (rows == null || rows.Length == 0) return;
        Row currentRow = rows[rowIndex];
        if (currentRow == null) return;

        if (columnIndex > 0)
        {
            columnIndex--;
            if (columnIndex < currentRow.tiles.Length)
                currentRow.tiles[columnIndex].SetLetter('\0');
        }
    }
    public void SubmitLetter(int index)
    {
        if (gameOver) return;
        if (rows == null || rows.Length == 0) return;
        Row currentRow = rows[rowIndex];
        if (currentRow == null) return;

        if (index >= 0 && index < SUPPORTED_KEYS.Length)
        {
            if (columnIndex < currentRow.tiles.Length)
            {
                currentRow.tiles[columnIndex].SetLetter((char)SUPPORTED_KEYS[index]);
                columnIndex++;
            }

        }

    }

    public string getCurrentGuess()
    {
        if (rows == null || rows.Length == 0)
        {
            Debug.LogWarning("Board has no rows initialized.");
            return string.Empty;
        }

        Row currentRow = rows[rowIndex];
        if (currentRow == null || currentRow.tiles == null || currentRow.tiles.Length == 0)
        {
            Debug.LogWarning("Current row or its tiles are not initialized.");
            return string.Empty;
        }

        string guess = "";
        for (int i = 0; i < currentRow.tiles.Length; i++)
        {
            guess += currentRow.tiles[i].Letter;
        }
        return guess;
    }

    public void ResetBoard()
    {
        gameOver = false;
        gameWon = false;
        rowIndex = 0;
        columnIndex = 0;

        foreach (Row row in rows)
        {
            foreach (Tile tile in row.tiles)
            {
                tile.SetLetter('\0');
                tile.GetComponent<Image>().color = new Color(0.07f, 0.07f, 0.075f, 1f); // Reset to default color

            }

        }
        if (kb != null)
        {
            kb.ResetKeyboard();
        }
    }

    public void NewGame()
    {
        ResetBoard();
        if (wp != null)
        {
            wp.PickRandomWord();
        }
    }

    public void OnWin()
    {
        gameOver = true;
        gameWon = true;
        Debug.Log("You win!");
        infoTextManager.ChooseText(InfoTextState.LevelUp);
        gm.timeRemaining += 10f; // Add 10 seconds for winning the game
        gm.currentLevel++;

        foreach (ParticlesManager p in pm)
        {
            p.PlayParticles();
        }
        gm.levelText.text = "LEVEL: " + gm.currentLevel;
        NewGame();
        return;
    }

    public void RestartGame()
    {
        gm.currentLevel = 1;
        gm.timeRemaining = gm.timeLimit;
        gm.timerIsRunning = true;
        gm.levelText.text = "LEVEL: " + gm.currentLevel;
        NewGame();
    }
}

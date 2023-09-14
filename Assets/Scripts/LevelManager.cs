using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public enum GameState
    {
        GameRunning,
        GamePaused,
        GameOver,
        LevelFinished
    }

    public GameState currentState = GameState.GameRunning;

    public GameObject endLevelText, gameOverText;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        currentState = GameState.GameRunning;
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.GameRunning:
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    currentState = GameState.GamePaused;
                    PauseGame();
                }
                break;

            case GameState.GamePaused:
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    currentState = GameState.GameRunning;
                    ResumeGame();
                }
                break;
        }
    }

    void GoToMenu()
    {
        // Go to Menu logic
    }

    void PauseGame()
    {
        Time.timeScale = 0; 
        // Other Pause logic
    }

    void ResumeGame()
    {
        Time.timeScale = 1; 
        // Other Resume logic
    }

    public void EndLevel()
    {
        currentState = GameState.LevelFinished;
        endLevelText.SetActive(true);
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        gameOverText.SetActive(true);
    }
}

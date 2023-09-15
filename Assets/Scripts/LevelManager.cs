using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private Camera cam;
    public enum GameState
    {
        GameRunning,
        GamePaused,
        GameOver,
        LevelFinished
    }
    public enum LevelState
    {
        NormalDimension,
        ShadowDimension
    }
    public GameObject normalDimensionGrid, shadowDimensionGrid;

    public LevelState levelState = LevelState.NormalDimension;

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

        cam = Camera.main;
    }
    void Start()
    {
        currentState = GameState.GameRunning;
        levelState = LevelState.NormalDimension;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            switch (currentState)
            {
                case GameState.GameRunning:

                    currentState = GameState.GamePaused;
                    PauseGame();

                    break;

                case GameState.GamePaused:

                    currentState = GameState.GameRunning;
                    ResumeGame();

                    break;
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            ChangeCameraCullingMask();
        }
    }
    public void ChangeCameraCullingMask()
    {
        if (levelState == LevelState.NormalDimension)
        {
            SetToShadowDimension();
        }
        else
        {
            SetToNormalDimension();
        }
    }

    void SetToNormalDimension()
    {
        levelState = LevelState.NormalDimension;
        normalDimensionGrid.SetActive(true);
        shadowDimensionGrid.SetActive(false);
    }

    void SetToShadowDimension()
    {
        normalDimensionGrid.SetActive(false);
        shadowDimensionGrid.SetActive(true);
        levelState = LevelState.ShadowDimension;
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
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        currentState = GameState.LevelFinished;
        endLevelText.SetActive(true);
        StartCoroutine(LoadAfterDelay(nextScene, 3f));
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        gameOverText.SetActive(true);
        StartCoroutine(LoadAfterDelay(1, 3f));
    }

    IEnumerator LoadAfterDelay(int sceneNumber, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneNumber);
    }
}

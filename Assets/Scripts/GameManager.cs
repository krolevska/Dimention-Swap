using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int currentLevel;
    private string[] gameState;
    private bool isGameOver;
    private bool isGamePaused = false;
    private bool isGameRunning;
    private bool isLevelFinished;

    private string playerName;
    private int playerLives;
    private int playerScore;
    private bool normalDimention;
    private int collectiblesCount;

    private int soundVolume;
    private int musicVolume;
    private int graphicsQuality;

    private int highScore;
    private string highScorePlayerName;

    public GameObject endLevelText, gameOverText;

    public static GameManager instance;


    private void Awake()
    {
        // Make sure there's only one GM instance
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
    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        isGameOver = false;
        isGamePaused = false;
        isGameRunning = true;
        isLevelFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                SaveAllData();
                PauseGame();
                GoToMenu();
            }
        }

        if (playerLives == 0)
        {
            isGameOver = true;
        }
    }
    void GoToMenu()
    {

    }
    void PauseGame()
    {
        isGamePaused = true;
        isGameRunning = false;
        Time.timeScale = 0;  // Pause the game

        // Show a pause menu UI here.
    }

    void ResumeGame()
    {
        isGamePaused = false;
        isGameRunning = true;
        Time.timeScale = 1;  // Resume the game

        // Hide the pause menu UI here.
    }

    void AddPoint(int point)
    {
        playerScore += point;
    }

    public void EndLevel()
    {
        isLevelFinished = true;
        endLevelText.SetActive(true);
    }

    public void GameOver()
    {
        isGameOver = true;
        isGameRunning = false;
        gameOverText.SetActive(true);
    }

    [System.Serializable]
    public class SaveData
    {
        public int currentLevel;
        public bool normalDimention;
        public int collectiblesCount;

        public int playerScore;
        public string playerName;
        public int playerLives;

        public int soundVolume;
        public int musicVolume;
        public int graphicsQuality;

        public int highScore;
        public string highScorePlayerName;
    }
    private void LoadData()
    {
        if (File.Exists("playerData.json"))
        {
            string json = File.ReadAllText("playerData.json");
            SaveData playerData = JsonUtility.FromJson<SaveData>(json);
                        
            currentLevel = playerData.currentLevel;
            normalDimention = playerData.normalDimention;
            collectiblesCount = playerData.collectiblesCount;

            playerScore = playerData.playerScore;
            playerLives = playerData.playerLives;
            playerName = playerData.playerName;

            soundVolume = playerData.soundVolume;
            musicVolume = playerData.musicVolume;
            graphicsQuality = playerData.graphicsQuality;

            highScore = playerData.highScore;
            highScorePlayerName = playerData.highScorePlayerName;
        }
    }

    private void SaveAllData()
    {
        SaveData playerData = new SaveData
        {
            currentLevel = currentLevel,
            normalDimention = normalDimention,
            collectiblesCount = collectiblesCount,
            playerScore = playerScore,
            playerLives = playerLives,
            playerName = playerName,
            soundVolume = soundVolume,
            musicVolume = musicVolume,
            graphicsQuality = graphicsQuality,
            highScore = highScore,
            highScorePlayerName = highScorePlayerName
        };

        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText("playerData.json", json);
    }
}

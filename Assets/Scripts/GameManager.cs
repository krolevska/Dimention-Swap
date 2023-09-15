// GameManager.cs
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int currentLevel;
    private string playerName;
    private int playerLives;
    private int playerScore;
    private bool normalDimention;
    private int collectiblesCount;
    private float soundVolume;
    private float musicVolume;
    private int graphicsQuality;
    private int highScore;
    private string highScorePlayerName;

    private int previousSceneIndex;

    public int PlayerLives
    {
        get { return playerLives; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
        LoadData();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SaveAllData();
        }
    }

    public void StoreCurrentScene()
    {
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadPreviousScene()
    {
        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex);
        }
    }

    public void SaveAllData()
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

        public float soundVolume;
        public float musicVolume;
        public int graphicsQuality;

        public int highScore;
        public string highScorePlayerName;
    }
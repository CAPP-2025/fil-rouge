using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //public LevelLoader levelLoader;

    public int world { get; private set; }
    public int stage { get; private set; }
    public static int lives;
    public int coins { get; private set; }
    public bool menu;
    public bool[] levels = new bool[3];

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        menu = true;
        NewGame();
    }

    public void NewGame()
    {
        lives = 3;
        coins = 0;

        SceneManager.LoadScene($"Start Screen");
    }

    public void GameOver()
    {
        // TODO: show game over screen
        SceneManager.LoadScene($"GameOver");

        Invoke(nameof(NewGame), 3f);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene($"Start Screen");
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        Physics2D.IgnoreLayerCollision(3, 7, false);
        //StartCoroutine(levelLoader.LoadWithTransition(world, stage, menu));
        if (menu)
            SceneManager.LoadScene($"Start Screen");
        else
            SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NextLevel()
    {
        levels[stage - 1] = true;
        LoadMenu();
    }

    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        if (lives > 0) {
            LoadLevel(world, stage);
        } else {
            GameOver();
        }
    }

    public void AddCoin()
    {
        coins++;

        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void LoseLife()
    {
        lives--;
    }

    public void AddLife()
    {
        lives++;
    }

}

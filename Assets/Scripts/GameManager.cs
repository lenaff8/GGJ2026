using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 
    [SerializeField] private SceneName[] scenes; 
    private int currentScene = 0; 
    public int Score { get; private set; }
    public event Action<int> OnScoreChanged;
    
    public enum SceneName
    {
        MainMenu = 0,
        Lvl0,
        Lvl1,
        Lvl2,
        Lvl3,
        Lvl4,
        Lvl5
    }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        //SoundManager.Instance.Play();
    }

    public void LoadNextScene()
    {
        if (scenes.Length > currentScene)
        {
            SceneManager.LoadScene(scenes[++currentScene].ToString());
        }
        else
        {
            // no se que quereis hacer post last lvl XD
        }
    }
    
    public void LoadScene(int scene)
    {
        if (scenes.Length > scene)
        {
            currentScene = scene;
            SceneManager.LoadScene(scenes[scene].ToString());
        }
    }
    
    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public void ResetGame()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
        SceneManager.LoadScene(scenes[0].ToString());
    }
}
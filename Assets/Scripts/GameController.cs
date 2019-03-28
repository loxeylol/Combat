using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private PlayerController _playerOne;
    [SerializeField] private PlayerController _playerTwo;

    private int _combinedScorePlayerOne, _combinedScorePlayerTwo;
    private bool _isPaused;

    // --- Properties -------------------------------------------------------------------------------------------------
    private bool SceneChanged { get; set; }
    public int FirstPlayerScore => _playerOne.Score;
    public int SecondPlayerScore => _playerTwo.Score;
    public bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            _isPaused = value;
            Time.timeScale = _isPaused ? 0f : 1f;
        }
    }
    public float GameTimer
    {
        get; set;
    }

    public int SelectedLevel
    {
        get; set;
    }

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _isPaused = false;
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        SettingsManager.LevelRange = SceneManager.sceneCountInBuildSettings - 1;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        Debug.Log("Active scene changed");
        ResetGameStats();
        GetPlayerControllers();
    }

    private void Update()
    {
        if (SettingsManager.IsThereTimeLimit)
        {
            CheckGameTimer();
        }
        else
        {
            CheckMaxScore();
        }
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public void LoadLevelWithIndex(int index)
    {

        if (SceneManager.GetSceneByBuildIndex(index) != null)
        {
            SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        }
    }
    public void LoadNextLevel()
    {
        Debug.Log("Load Next level");
        if (!SceneChanged)
        {
            return;
        }

        SceneChanged = false;
        SetCombinedScore();
        ResetGameStats();

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextScene);
        else
            GameIsOver();
    }
    public void PauseGame()
    {
        IsPaused = !IsPaused;
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void GetPlayerControllers()
    {
        SceneChanged = true;
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0))
        {
            _playerOne = GameObject.Find("Player1").GetComponent<PlayerController>();
            _playerTwo = GameObject.Find("Player2").GetComponent<PlayerController>();
        }
    }

    private void CheckGameTimer()
    {
        GameTimer -= Time.deltaTime;
        if (GameTimer <= 0)
        {
            string a = FirstPlayerScore > SecondPlayerScore ? "PlayerOne Wins!" : "PlayerTwo Wins!";
            Debug.Log(a);
            LoadNextLevel();
        }

    }
    private void CheckMaxScore()
    {
        if (Mathf.Max(FirstPlayerScore, SecondPlayerScore) >= SettingsManager.MaxScore)
        {
            string winner = FirstPlayerScore > SecondPlayerScore ? "PlayerOne Wins!" : "PlayerTwo Wins!";
            Debug.Log(winner);

            LoadNextLevel();
        }
    }
    private void GameIsOver()
    {
        string winner = FirstPlayerScore > SecondPlayerScore ? "PlayerOne Wins!" : "PlayerTwo Wins!";
        Debug.Log(winner);
    }
    private void SetCombinedScore()
    {
        _combinedScorePlayerOne += FirstPlayerScore;
        _combinedScorePlayerTwo += SecondPlayerScore;
        Debug.Log(_combinedScorePlayerOne + "CombinedScorePlayerOne  -  CombinedScorePlayerTwo " + _combinedScorePlayerTwo);

    }

    private void ResetGameStats()
    {
        GameTimer = SettingsManager.GameTimer;
        IsPaused = false;
        if (MonoFactory.Instance != null)
        {
            MonoFactory.ReturnAllChildren();
        }
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
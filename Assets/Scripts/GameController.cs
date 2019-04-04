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

    private bool _isPaused;
    public Action<PlayerController> GameOver;
    // --- Properties -------------------------------------------------------------------------------------------------
    private bool LevelChanged { get; set; }
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
    public int CombinedScorePlayerOne { get; private set; }
    public int CombinedScorePlayerTwo { get; private set; }
    public float GameTimer
    {
        get; set;
    }

    public int SelectedLevel
    {
        get; set;
    }
    public int CurrentLevelIndex
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
        //SettingsManager.LevelRange = SceneManager.sceneCountInBuildSettings - 1;
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
        if (!LevelChanged)
        {
            return;
        }

        Debug.Log("Load Next level");
        LevelChanged = false;
        SetCombinedScore();
        if (CurrentLevelIndex > SettingsManager.LevelRange)
        {
            GameIsOver();
        }
        else
        {
            ResetGameStats();
        }

    }
    public void PauseGame()
    {
        IsPaused = !IsPaused;
    }
    public int GetHighScore()
    {
        return CombinedScorePlayerOne > CombinedScorePlayerTwo ? CombinedScorePlayerOne : CombinedScorePlayerTwo;
    }
    public void ResetCombinedPlayerScore()
    {
        CombinedScorePlayerOne = 0;
        CombinedScorePlayerTwo = 0;
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void GetPlayerControllers()
    {
        LevelChanged = true;
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
            LoadNextLevel();
        }

    }
    private void CheckMaxScore()
    {
        if (Mathf.Max(FirstPlayerScore, SecondPlayerScore) >= SettingsManager.MaxScore)
        {
            LoadNextLevel();
        }
    }
    private void GameIsOver()
    {
        if (CombinedScorePlayerOne > CombinedScorePlayerTwo)
        {
            GameOver?.Invoke(_playerOne);
        }
        else if (CombinedScorePlayerOne < CombinedScorePlayerTwo)
        {
            GameOver?.Invoke(_playerTwo);
        }
        else
        {
            GameOver?.Invoke(null);
        }

        ResetPlayer();
    }
    private void SetCombinedScore()
    {
        CombinedScorePlayerOne += FirstPlayerScore;
        CombinedScorePlayerTwo += SecondPlayerScore;
    }

    private void ResetPlayer()
    {
        _playerOne.SetPlayerToStartPos();
        _playerTwo.SetPlayerToStartPos();
        _playerOne.Score = 0;
        _playerTwo.Score = 0;

    }

    private void ResetGameStats()
    {
        GameTimer = SettingsManager.GameTimer;
        SettingsManager.HighScore = GetHighScore();
        IsPaused = false;
        if (CurrentLevelIndex >= 1)
        {

            if (MonoFactory.Instance != null)
            {
                MonoFactory.ReturnAllChildren();
            }

            if (LevelBuilder.Instance != null)
            {
                LevelBuilder.Instance.LevelIndex = CurrentLevelIndex - 1;

                LevelBuilder.Instance.BuildLevel();
            }
            if (!SettingsManager.OverWriteLevelSettings)
            {
                SettingsManager.Instance.SetSettings(CurrentLevelIndex);
            }
            if (_playerTwo != null && _playerOne != null)
            {
                ResetPlayer();
            }
        }
        CurrentLevelIndex++;
        LevelChanged = true;
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
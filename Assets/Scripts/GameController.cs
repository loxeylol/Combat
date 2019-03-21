using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField, Range(2, 20)] private int _maxScore;
    [SerializeField] private PlayerController _playerOne;
    [SerializeField] private PlayerController _playerTwo;
    private int _combinedScorePlayerOne, _combinedScorePlayerTwo;

    // --- Properties -------------------------------------------------------------------------------------------------
    private bool SceneChanged { get; set; }
    public int FirstPlayerScore => _playerOne.Score;
    public int SecondPlayerScore => _playerTwo.Score;
    public static GameController gameController;

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        if (gameController == null)
        {
            gameController = this;
        }
        else if (gameController != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        GetPlayerControllers();
    }

    private void Update()
    {
        CheckMaxScore();
    }


    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public void LoadNextLevel()
    {
        if (!SceneChanged)
        {
            return;
        }
        SceneChanged = false;
        SetCombinedScore();
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene < SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(currentScene += 1);
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void GetPlayerControllers()
    {
        if (SceneChanged)
        {
            return;
        }
        SceneChanged = true;
        _playerOne = GameObject.Find("Player1").GetComponent<PlayerController>();
        _playerTwo = GameObject.Find("Player2").GetComponent<PlayerController>();
    }
    private void CheckMaxScore()
    {

        if (SecondPlayerScore >= _maxScore || FirstPlayerScore >= _maxScore)
        {
            string a = FirstPlayerScore > SecondPlayerScore ? "PlayerOne Wins!" : "PlayerTwo Wins!";
            Debug.Log(a);
            LoadNextLevel();

        }
    }

    private void SetCombinedScore()
    {
        _combinedScorePlayerOne += FirstPlayerScore;
        _combinedScorePlayerTwo += SecondPlayerScore;
        ResetScore();
        Debug.Log(_combinedScorePlayerOne + "CombinedScorePlayerOne");

    }
    private void ResetScore()
    {
        _playerOne.Score = 0;
        _playerTwo.Score = 0;
    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
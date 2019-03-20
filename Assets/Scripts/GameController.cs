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
    public int FirstPlayerScore => _playerOne.Score;
    public int SecondPlayerScore => _playerTwo.Score;
    public static GameController gameController;

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
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
    private void Update()
    {
        Debug.Log("PlayerOneScore " + FirstPlayerScore + "maxscore" + _maxScore);
        CheckMaxScore();

    }


    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(currentScene += 1);
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    private void CheckMaxScore()
    {
        if (FirstPlayerScore >= _maxScore || SecondPlayerScore >= _maxScore)
        {
            string a = FirstPlayerScore > SecondPlayerScore ? "PlayerOne Wins!" : "PlayerTwo Wins!";
            Debug.Log(a);
            
        }
    }

    private void SetCombinedScore()
    {
        _combinedScorePlayerOne += FirstPlayerScore;
        _combinedScorePlayerTwo += SecondPlayerScore;


    }

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
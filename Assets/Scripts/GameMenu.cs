using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameMenu : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private GameObject _gameTimerTextObject;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private KeyCode _menuKey;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private Text _GameOverText;
    [SerializeField] private Text _playerOneScore;
    [SerializeField] private Text _playerTwoScore;
    private Text _gameTimerText;
    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _gameTimerText = _gameTimerTextObject.GetComponent<Text>();
        _gameTimerTextObject.SetActive(SettingsManager.IsThereTimeLimit);
        _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        GameController.Instance.GameOver += (winner) => OnGameOver(winner);
    }
    private void Update()
    {
        if (Input.GetKeyDown(_menuKey))
        {
            ToggleGameMenu();
        }
        if (_gameTimerTextObject.activeSelf)
        {
            _gameTimerText.text = "Time Remaining " + Math.Round(GameController.Instance.GameTimer);
        }
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void ToggleGameMenu()
    {
        _gameMenu.SetActive(!_gameMenu.activeSelf);
        GameController.Instance.PauseGame();
    }
    private void OnGameOver(PlayerController winner)
    {

        if (_GameOverText != null)
        {
            _GameOverText.gameObject.SetActive(true);
            _playerOneScore.gameObject.SetActive(true);
            _playerTwoScore.gameObject.SetActive(true);
            _playerOneScore.text = GameController.Instance.CombinedScorePlayerOne.ToString();
            _playerTwoScore.text = GameController.Instance.CombinedScorePlayerTwo.ToString();
            if (winner != null)
            {
                _GameOverText.text = winner.name + " has Won!";
            }
            else
            {
                _GameOverText.text = "tie!";
            }
            MonoFactory.ReturnAllChildren();
            ToggleGameMenu();
        }
    }
    private void OnBackToMenuButtonClicked()
    {
        GameController.Instance.GameOver -= (winner) => OnGameOver(winner);
        GameController.Instance.ResetCombinedPlayerScore();
        SettingsManager.HighScore = GameController.Instance.GetHighScore();
        GameController.Instance.CurrentLevelIndex = 0;
        MonoFactory.ReturnAllChildren();
        GameController.Instance.LoadLevelWithIndex(0);
    }
    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
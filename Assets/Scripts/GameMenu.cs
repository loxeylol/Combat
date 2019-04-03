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
    private Text _gameTimerText;
    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _gameTimerText = _gameTimerTextObject.GetComponent<Text>();
        _gameTimerTextObject.SetActive(SettingsManager.IsThereTimeLimit);
        _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
    }
    private void Update()
    {
        if (Input.GetKeyDown(_menuKey))
        {
            ToggleGameMenu();
        }
        if (_gameTimerTextObject.activeSelf)
        {
            _gameTimerText.text = "Time Remaining" + Math.Round(GameController.Instance.GameTimer);
        }
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void ToggleGameMenu()
    {
        _gameMenu.SetActive(!_gameMenu.activeSelf);
        GameController.Instance.PauseGame();
    }
    private void OnBackToMenuButtonClicked()
    {
        SettingsManager.HighScore = GameController.Instance.GetHighScore();
        //SettingsManager.SaveSettings();
        GameController.Instance.LoadLevelWithIndex(0);
    }
    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
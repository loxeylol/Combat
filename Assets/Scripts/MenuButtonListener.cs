﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuButtonListener : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] Button _startGameButton;
	// --- Properties -------------------------------------------------------------------------------------------------

	// --- Unity Functions --------------------------------------------------------------------------------------------
	private void Awake()
    {
        _startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

	// --- Protected/Private Methods ----------------------------------------------------------------------------------
    void OnStartGameButtonClicked()
    {
        GameController.Instance.LoadNextLevel();
    }
    
	// --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUi : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    private PlayerController _player;
    [SerializeField] private TextMeshPro _playerLabel;
    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _player = gameObject.GetComponent<PlayerController>();

        //_playerLabel = gameObject.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        _playerLabel.text = "" + _player.Score;
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
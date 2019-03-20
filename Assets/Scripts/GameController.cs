using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField, Range(2, 20)] private int _maxScore;
    [SerializeField] PlayerController _playerOne;
    [SerializeField] PlayerController _playerTwo;
    // --- Properties -------------------------------------------------------------------------------------------------
    public int FirstPlayerScore => _playerOne.Score;
    public int SecondPlayerScore => _playerTwo.Score;
    public static GameController gameController;

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        if(gameController == null)
        {
            gameController = this;
        }
        else if(gameController != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        Debug.Log("FirsplayerScore: " + FirstPlayerScore);
        Debug.Log("SecondPlayerScore: " + SecondPlayerScore);
       

    }
    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
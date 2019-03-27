using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neox;
using Neox.Helpers;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioSource _musicSource;
    // --- Properties -------------------------------------------------------------------------------------------------

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
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public void PlaySound(AudioClip clip)
    {
       
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
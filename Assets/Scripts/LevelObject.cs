using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neox.Helpers;


public class LevelObject : MonoBehaviour, IHittable
{

    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    private AudioSource _wallHitSound;
    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _wallHitSound = GetComponent<AudioSource>();
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    public void OnHit(Shootable hittedObject, Collision collision)
    {
        _wallHitSound.Play();
        if (!SettingsManager.BouncyBullets)
        {
            hittedObject.Explode();
        }
        hittedObject.Reflect(collision);
    }
	// --- Protected/Private Methods ----------------------------------------------------------------------------------
    
	// --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
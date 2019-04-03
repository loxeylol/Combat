using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PortalBehaviour : LevelObject
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private GameObject _otherPortal;
    // --- Properties -------------------------------------------------------------------------------------------------
    public override bool ExplodeShootable => false;
    public override bool ReflectShootable => false;
    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
		
    }
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = _otherPortal.transform.position;
    }
    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
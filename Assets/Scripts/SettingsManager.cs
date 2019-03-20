using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    // --- Enums ------------------------------------------------------------------------------------------------------
    public enum FireModes
    {
        Straight = 0,
        Ricochet = 1,
        Guided = 2,
        GuidedRicochet = 3
    }

    // --- Nested Classes ---------------------------------------------------------------------------------------------
    public class Settings
    {
        public bool _freePlayerRotation = true;
        public bool _friendlyFire = true;
        public bool _bouncyBullets = true;
        public bool _invisibleTankMode = false;
        public FireModes _fireModes = FireModes.Straight;
    }

    // --- Fields -----------------------------------------------------------------------------------------------------    
    private static Settings _settings;

    [SerializeField] private FireModes _fireModeEnum;
    // --- Properties -------------------------------------------------------------------------------------------------
    public static bool FreePlayerRotation
    {
        get { return _settings._freePlayerRotation; }
        set { _settings._freePlayerRotation = value; }
    }
    public static bool FriendlyFire
    {
        get { return _settings._friendlyFire; }
        set { _settings._friendlyFire = value; }
    }
    public static bool BouncyBullets
    {
        get { return _settings._bouncyBullets; }
        set { _settings._bouncyBullets = value; }
    }
    public static bool InvisibleTankMode
    {
        get { return _settings._invisibleTankMode; }
        set { _settings._invisibleTankMode = value; }
    }
    public static FireModes SelectedFireMode
    {
        get { return _settings._fireModes; }
        set { _settings._fireModes = Instance._fireModeEnum; }
    }





    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {


        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        _settings = new Settings();


    }
    private void Update()
    {
        Debug.Log(SelectedFireMode);
    }
    // --- Public/Internal Methods ------------------------------------------------------------------------------------


    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
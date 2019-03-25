using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireModes
{
    Straight = 0,
    Ricochet = 1,
    Guided = 2,
    GuidedRicochet = 3
}

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    // --- Enums ------------------------------------------------------------------------------------------------------


    // --- Nested Classes ---------------------------------------------------------------------------------------------
    [Serializable]
    public class Settings
    {
        public bool _freePlayerRotation = true;
        public bool _friendlyFire = true;
        public bool _invisibleTankMode = false;
        public bool _canBulletsBeDirected;
        public FireModes _fireModes = FireModes.Straight;
        [Range(2, 24)] public int _playerRotationSteps = 12;

    }

    // --- Fields -----------------------------------------------------------------------------------------------------    
    [SerializeField] private Settings _settings;

    // --- Properties -------------------------------------------------------------------------------------------------
    public static bool FreePlayerRotation
    {
        get { return Instance._settings._freePlayerRotation; }
        set { Instance._settings._freePlayerRotation = value; }
    }
    public static bool FriendlyFire
    {
        get { return Instance._settings._friendlyFire; }
        set { Instance._settings._friendlyFire = value; }
    }
    public static bool BouncyBullets
    {
        get { return SelectedFireMode == FireModes.Ricochet || SelectedFireMode == FireModes.GuidedRicochet; }
    }
    public static bool InvisibleTankMode
    {
        get { return Instance._settings._invisibleTankMode; }
        set { Instance._settings._invisibleTankMode = value; }
    }
    public static FireModes SelectedFireMode
    {
        get { return Instance._settings._fireModes; }
        set { Instance._settings._fireModes = value; }
    }
    public static bool CanBulletsBeDirected
    {
        get { return SelectedFireMode == FireModes.Guided || SelectedFireMode == FireModes.GuidedRicochet; }

    }
    public static int PlayerRotationSteps
    {
        get { return Instance._settings._playerRotationSteps; }
        set { Instance._settings._playerRotationSteps = value; }
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
    // --- Public/Internal Methods ------------------------------------------------------------------------------------


    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
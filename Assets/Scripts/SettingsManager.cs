using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public enum BulletType
{
    Regular = 0,
    Splash = 1,
    Multiple = 2
}

[Flags]
public enum FireModes
{
    Straight = 0,
    Guided = 1 << 0,
    Ricochet = 1 << 1,
}

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public const string DEFAULT_SETTING = "Default_settings";
    public const string USER_SETTINGS_FILENAME = "user_settings.json";

    private string UserSettingsFilepath => Path.Combine(Application.dataPath, USER_SETTINGS_FILENAME);

    // --- Enums ------------------------------------------------------------------------------------------------------


    // --- Nested Classes ---------------------------------------------------------------------------------------------
    [Serializable]
    public class Settings
    {
        public bool _freePlayerRotation = true;
        public bool _friendlyFire = true;
        public bool _invisibleTankMode = false;
        public bool _canBulletsBeDirected;
        public BulletType _bulletType = BulletType.Regular;
        [EnumMask(true)] public FireModes _fireModes = FireModes.Straight;
        [Range(2, 24)] public int _playerRotationSteps = 12;
        public int _maxScore = 10;
        public float _gameTimer = 60;
        public bool _isThereTimeLimit = false;
        public int _levelRange;
        public float _bulletLifeTime = 4;
        public bool _splitScreenMode; 
        public int _highscore;
    }

    // --- Fields -----------------------------------------------------------------------------------------------------    
    [SerializeField] private Settings _settings;
    private bool _overWriteLevelSettings;
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
        get { return SelectedFireMode.HasFlag(FireModes.Ricochet); }
    }
    public static bool InvisibleTankMode
    {
        get { return Instance._settings._invisibleTankMode; }
        set { Instance._settings._invisibleTankMode = value; }
    }
    public static BulletType BulletType
    {
        get { return Instance._settings._bulletType; }
        set { Instance._settings._bulletType = value; }
    }
    public static FireModes SelectedFireMode
    {
        get { return Instance._settings._fireModes; }
        set { Instance._settings._fireModes = value; }
    }
    public static bool CanBulletsBeDirected
    {
        get { return SelectedFireMode.HasFlag(FireModes.Guided); }

    }
    public static int PlayerRotationSteps
    {
        get { return Instance._settings._playerRotationSteps; }
        set { Instance._settings._playerRotationSteps = value; }
    }
    public static int MaxScore
    {
        get { return Instance._settings._maxScore; }
        set { Instance._settings._maxScore = value; }
    }
    public static float GameTimer
    {
        get { return Instance._settings._gameTimer; }
        set { Instance._settings._gameTimer = Mathf.Clamp(value, 0.3f, 2f) * 60; }
    }
    public static bool IsThereTimeLimit
    {
        get { return Instance._settings._isThereTimeLimit; }
        set { Instance._settings._isThereTimeLimit = value; }
    }
    public static int LevelRange
    {
        get { return Instance._settings._levelRange; }
        set { Instance._settings._levelRange = value; }
    }
    public static float BulletLifeTime
    {
        get { return Instance._settings._bulletLifeTime; }
        set { Instance._settings._bulletLifeTime = value; }
    }
    public static int HighScore
    {
        get { return Instance._settings._highscore; }
        set { Instance._settings._highscore = value; }
    }
    public static bool OverWriteLevelSettings
    {
        get { return Instance._overWriteLevelSettings; }
        set { Instance._overWriteLevelSettings = value; }
    }

    public static bool SplitScreenMode
    {
        get { return Instance._settings._splitScreenMode; }
        set { Instance._settings._splitScreenMode = value; }
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
        SetSettings();
    }
    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public static void SaveSettings()
    {
        Instance._SaveSettings();
    }
    public static Settings LoadData(string settings = DEFAULT_SETTING)
    {
        return Instance._LoadData(settings);
    }
    public Settings GetLevelSettings()
    {
        return new Settings();
    }
    public void SetSettings(int index = 0)
    {
        Instance._SetSettings(index);
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    private void _SaveSettings()
    {
        string settingsJson = JsonUtility.ToJson(_settings, true);
        File.WriteAllText(UserSettingsFilepath, settingsJson);
        Debug.Log(settingsJson);
    }

    private Settings _LoadData(string settings = DEFAULT_SETTING)
    {
        string settingsJson;
        if (OverWriteLevelSettings && File.Exists(UserSettingsFilepath))
        {
            settingsJson = File.ReadAllText(UserSettingsFilepath);
        }
        else
        {
            TextAsset textAsset = Resources.Load<TextAsset>(settings);

            if (textAsset == null)
            {
                textAsset = Resources.Load<TextAsset>(DEFAULT_SETTING);
            }

            settingsJson = textAsset.text;
            Resources.UnloadAsset(textAsset);
        }
       
        return JsonUtility.FromJson<Settings>(settingsJson);
    }
    private void _SetSettings(int index = 0)
    {
        _settings = LoadData("settings " + index);
    }


    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
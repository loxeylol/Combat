using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class PersistentData
{


    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------
    private class FilesToBeSaved
    {
        public SettingsManager.Settings settings;
        public int highscore;
    }
    // --- Fields -----------------------------------------------------------------------------------------------------
    private FilesToBeSaved _filesToBeSaved;

    private SettingsManager.Settings Settings
    {
        get { return _filesToBeSaved.settings; }
        set { _filesToBeSaved.settings = value; }
    }
    private int Highscore
    {
        get { return _filesToBeSaved.highscore; }
        set { _filesToBeSaved.highscore = value; }
    }

    // --- Properties -------------------------------------------------------------------------------------------------
    // --- Constructors -----------------------------------------------------------------------------------------------
    public PersistentData()
    {
        _filesToBeSaved = new FilesToBeSaved();


    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public void SetCurrentSettings(SettingsManager.Settings settings)
    {
        _SetCurrentSettings(settings);
    }
    public void SetCurrentHighscore(int highscore)
    {
        _SetCurrentHighScore(highscore);
    }
    public void SaveData()
    {
        _SaveData();
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------
    private void _SetCurrentSettings(SettingsManager.Settings settings)
    {
        Settings = settings;
    }
    private void _SetCurrentHighScore(int highscore)
    {
        Highscore = highscore;
    }
    private void _SaveData()
    {
        string settingsJson = JsonUtility.ToJson(_filesToBeSaved, true);
        string path = Path.Combine(Application.dataPath, "settings.json");
        File.WriteAllText(Path.Combine(Application.dataPath, "settings.json"), settingsJson);
        Debug.Log(settingsJson);
    }
    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class MenuUi : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private Text _title;
    [Header("Settings Content")]
    [SerializeField] private Dropdown _fireModeDropDown;
    [SerializeField] private Dropdown _bulletTypeDropDown;
    [SerializeField] private Toggle _toggleFreePlayerRotation;
    [SerializeField] private Toggle _toggleFriendlyFire;
    [SerializeField] private Toggle _toggleInvisibleTankMode;
    [SerializeField] private Toggle _toggleTimeLimit;
    [SerializeField] private Toggle _toggleOverrideLevelSettings;
    [SerializeField] private Slider _playerRotationStepsSlider;
    [SerializeField] private Slider _maxScoreSlider;
    [SerializeField] private Button _toggleSettingsButton;
    [SerializeField] private GameObject _settingsContent;
    [SerializeField] private GameObject _menuContent;
    [SerializeField] private GameObject[] _toggledUiElements;
    [SerializeField] private Slider _levelSelectSlider;
    [Header("Main Menu")]
    [SerializeField] private Button _backToMainMenuButton;
    [SerializeField] private Button _startGameButton;
    private Text _playerRotationStepSliderText, _maxScoreSliderText, _timeLimitSliderText, _levelSelectSliderText;
    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _playerRotationStepSliderText = _playerRotationStepsSlider.GetComponentInChildren<Text>();
        _maxScoreSliderText = _maxScoreSlider.GetComponentInChildren<Text>();
        _levelSelectSliderText = _levelSelectSlider.GetComponentInChildren<Text>();

        _levelSelectSlider.wholeNumbers = true;
        _levelSelectSlider.minValue = 1f;
        _levelSelectSlider.maxValue = LevelBuilder.Instance.LevelRange;
        _levelSelectSlider.value = 1f;
        SettingsManager.LevelRange = (int)_levelSelectSlider.maxValue;


        _toggleInvisibleTankMode.isOn = SettingsManager.InvisibleTankMode;
        _toggleFriendlyFire.isOn = SettingsManager.FriendlyFire;
        _toggleFreePlayerRotation.isOn = SettingsManager.FreePlayerRotation;
        _playerRotationStepsSlider.value = SettingsManager.PlayerRotationSteps;
        _maxScoreSlider.value = SettingsManager.MaxScore;
        _toggleTimeLimit.isOn = SettingsManager.IsThereTimeLimit;
        _toggleOverrideLevelSettings.isOn = SettingsManager.OverWriteLevelSettings;

        _levelSelectSliderText.text = _levelSelectSlider.value.ToString();
        _playerRotationStepSliderText.text = _playerRotationStepsSlider.value.ToString();
        _maxScoreSliderText.text = _maxScoreSlider.value.ToString();

        _bulletTypeDropDown.ClearOptions();
        _bulletTypeDropDown.AddOptions(Enum.GetNames(typeof(BulletType)).ToList());
        _bulletTypeDropDown.value = (int)SettingsManager.BulletType;

        _fireModeDropDown.ClearOptions();
        _fireModeDropDown.AddOptions(Enum.GetNames(typeof(FireModes)).ToList());
        _fireModeDropDown.value = (int)SettingsManager.SelectedFireMode;

        _toggleOverrideLevelSettings.onValueChanged.AddListener(state => SettingsManager.OverWriteLevelSettings = state);
        _toggleFriendlyFire.onValueChanged.AddListener(state => SettingsManager.FriendlyFire = state);
        _toggleFreePlayerRotation.onValueChanged.AddListener(state => SettingsManager.FreePlayerRotation = state);
        _toggleInvisibleTankMode.onValueChanged.AddListener(state => SettingsManager.InvisibleTankMode = state);
        _fireModeDropDown.onValueChanged.AddListener(state => SettingsManager.SelectedFireMode = (FireModes)state);
        _bulletTypeDropDown.onValueChanged.AddListener(state => SettingsManager.BulletType = (BulletType)state);
        _toggleTimeLimit.onValueChanged.AddListener(OnToggleTimeLimit);

        _toggledUiElements[0].SetActive(_toggleTimeLimit.isOn);
        _toggledUiElements[1].SetActive(!_toggleTimeLimit.isOn);

        _startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        _toggleSettingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _backToMainMenuButton.onClick.AddListener(OnSettingsSaveAndGoBackButtonPressed);

        _levelSelectSlider.onValueChanged.AddListener(OnLevelSelectSliderValueChanged);
        _playerRotationStepsSlider.onValueChanged.AddListener(OnPlayerRotationSliderValueChanged);
        _maxScoreSlider.onValueChanged.AddListener(OnMaxScoreSliderValueChanged);
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    public void OnSettingsSaveAndGoBackButtonPressed()
    {
        SettingsManager.SaveSettings();
        OnSettingsButtonClicked();
    }

    public void OnPlayerRotationSliderValueChanged(float a)
    {
        SettingsManager.PlayerRotationSteps = (int)a;
        _playerRotationStepSliderText.text = _playerRotationStepsSlider.value.ToString();
    }

    public void OnLevelSelectSliderValueChanged(float a)
    {
        _levelSelectSliderText.text = a.ToString();
        GameController.Instance.SelectedLevel = (int)a;
    }
    public void OnMaxScoreSliderValueChanged(float a)
    {
        SettingsManager.MaxScore = (int)a;
        _maxScoreSliderText.text = _maxScoreSlider.value.ToString();
    }
    public void OnToggleTimeLimit(bool isSet)
    {
        SettingsManager.IsThereTimeLimit = isSet;
        _toggledUiElements[0].SetActive(isSet);
        _toggledUiElements[1].SetActive(!isSet);
    }
    public void OnSettingsButtonClicked()
    {
        _settingsContent.SetActive(!_settingsContent.activeSelf);
        _menuContent.SetActive(!_menuContent.activeSelf);
        if (_menuContent.activeSelf)
        {
            _title.text = "Main Menu";
        }
        else
        {
            _title.text = "Settings";
        }
    }

    public void OnStartGameButtonClicked()
    {
        GameController.Instance.CurrentLevelIndex = (int)_levelSelectSlider.value;
        GameController.Instance.LoadLevelWithIndex(1);
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
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
    [SerializeField] private Dropdown _dropDown;
    [SerializeField] private Toggle _toggleFreePlayerRotation;
    [SerializeField] private Toggle _toggleFriendlyFire;
    [SerializeField] private Toggle _toggleInvisibleTankMode;
    [SerializeField] private Slider _playerRotationStepsSlider;
    [SerializeField] private Button _toggleMenuButton;
    [SerializeField] private GameObject _settingsMenu;
    // --- Properties -------------------------------------------------------------------------------------------------

    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _toggleInvisibleTankMode.isOn = SettingsManager.InvisibleTankMode;
        _toggleFriendlyFire.isOn = SettingsManager.FriendlyFire;
        _toggleFreePlayerRotation.isOn = SettingsManager.FreePlayerRotation;
        _playerRotationStepsSlider.value = SettingsManager.PlayerRotationSteps;

        _dropDown.ClearOptions();
        _dropDown.AddOptions(Enum.GetNames(typeof(FireModes)).ToList());
        _dropDown.value = (int)SettingsManager.SelectedFireMode;

        _toggleFriendlyFire.onValueChanged.AddListener(state => SettingsManager.FriendlyFire = state);
        _toggleFreePlayerRotation.onValueChanged.AddListener(state => SettingsManager.FreePlayerRotation = state);
        _toggleInvisibleTankMode.onValueChanged.AddListener(state => SettingsManager.InvisibleTankMode = state);
        _playerRotationStepsSlider.onValueChanged.AddListener(OnSliderValueChanged);
        _dropDown.onValueChanged.AddListener(state => SettingsManager.SelectedFireMode = (FireModes)state);

    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    public void OnSliderValueChanged(float a)
    {
        SettingsManager.PlayerRotationSteps = (int)a;
    }

    public void OnToggleMenuButtonClicked()
    {
        _settingsMenu.SetActive(!_settingsMenu.activeSelf);
    }
    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
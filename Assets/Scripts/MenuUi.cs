using System;
using System.Collections;
using System.Collections.Generic;
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
        _playerRotationStepsSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------
    public void OnDropDownSelect()
    {
        SettingsManager.SelectedFireMode = (SettingsManager.FireModes)_dropDown.value;
    }
    public void OnToggledListener()
    {

        SettingsManager.FreePlayerRotation = _toggleFreePlayerRotation.isOn;
        SettingsManager.FriendlyFire = _toggleFriendlyFire.isOn;
        SettingsManager.InvisibleTankMode = _toggleInvisibleTankMode.isOn;

    }
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomStepSlider : MonoBehaviour
{
    // --- Enums ------------------------------------------------------------------------------------------------------

    // --- Nested Classes ---------------------------------------------------------------------------------------------

    // --- Fields -----------------------------------------------------------------------------------------------------
    [SerializeField] private float _stepSize;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    private Text _sliderText;
    private Slider _slider;
    // --- Properties -------------------------------------------------------------------------------------------------
    public float SliderValue { get; set; }
    // --- Unity Functions --------------------------------------------------------------------------------------------
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.wholeNumbers = true;
        _slider.minValue = _minValue;
        _slider.maxValue = _maxValue;
        _slider.value = (SettingsManager.GameTimer / 60) + _stepSize;
        _sliderText = GetComponentInChildren<Text>();
        _sliderText.text = _slider.value * _stepSize + "m";
        _slider.onValueChanged.AddListener(ClampSliderValue);
    }

    public void ClampSliderValue(float a)
    {
        SliderValue = a * _stepSize;
        _sliderText.text = SliderValue.ToString() + "m";
        SettingsManager.GameTimer = SliderValue;
        Debug.Log(SettingsManager.GameTimer);
    }

    // --- Public/Internal Methods ------------------------------------------------------------------------------------

    // --- Protected/Private Methods ----------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------
}

// **************************************************************************************************************************************************
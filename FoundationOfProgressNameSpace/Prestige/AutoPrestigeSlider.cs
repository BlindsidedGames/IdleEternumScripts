using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;
using static Blindsided.SaveData.TextColourStrings;

namespace FoundationOfProgressNameSpace.Prestige
{
    public class AutoPrestigeSlider : MonoBehaviour
    {
        [SerializeField] private Slider autoPrestigeSlider;
        [SerializeField] private TMP_Text autoPrestigeValueText;
        private readonly double actualMin = 100;
        private readonly double actualMax = 5000;

        [SerializeField] private Button autoPrestigeToggle;
        [SerializeField] private TMP_Text autoPrestigeToggleText;


        private void Start()
        {
            // Convert them to logarithmic scale for the slider
            var sliderMin = (float)Math.Log10(actualMin + 1); // Adding 1 to avoid Log10(0)
            var sliderMax = (float)Math.Log10(actualMax);

// Set the slider's min and max values
            autoPrestigeSlider.minValue = sliderMin;
            autoPrestigeSlider.maxValue = sliderMax;
            autoPrestigeSlider.onValueChanged.AddListener(SetAmountToBreakFor);
            autoPrestigeSlider.value = (float)Math.Log10(AutoPrestigeSavedValue + 1);
            SetAutoPrestigeToggleText();
            autoPrestigeToggle.onClick.AddListener(ToggleAutoPrestige);
        }

        public void SetAmountToBreakFor(float amount)
        {
            AutoPrestigeSavedValue = (int)Math.Pow(10, autoPrestigeSlider.value) - 1;
            autoPrestigeValueText.text =
                $"<b>Auto-Prestige Threshold</b> | {ColourGreen}{AutoPrestigeSavedValue:N0}{EndColour}";
        }

        public void ToggleAutoPrestige()
        {
            AutoPrestigeEnabled = !AutoPrestigeEnabled;
            SetAutoPrestigeToggleText();
        }

        private void SetAutoPrestigeToggleText()
        {
            autoPrestigeToggleText.text = AutoPrestigeEnabled ? "On" : "Off";
        }
    }
}
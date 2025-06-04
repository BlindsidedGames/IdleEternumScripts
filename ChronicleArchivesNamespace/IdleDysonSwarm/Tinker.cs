using System;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ChronicleArchivesNamespace.IdleDysonSwarm.IdsStaticReferences;
using static Blindsided.SaveData.StaticReferences;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.TextColourStrings;

namespace ChronicleArchivesNamespace.IdleDysonSwarm
{
    public class Tinker : MonoBehaviour
    {
        public Button tinkerButton;
        public MPImage progressBar;
        public TMP_Text progressText;

        private readonly float _duration = 10f;


        private void OnEnable()
        {
            IdsEvents.UpdateUI += SetUI;
        }

        private void OnDisable()
        {
            IdsEvents.UpdateUI -= SetUI;
        }

        private void Start()
        {
            tinkerButton.onClick.AddListener(StartTinkering);
            SetUI();
        }

        private void StartTinkering()
        {
            if (TinkerActive) return;
            TinkerActive = true;
            SetTinkerButton();
        }

        public void Progress(float timeScale)
        {
            if (!TinkerActive) return;
            TinkerProgress += timeScale;
            if (TinkerProgress >= _duration)
            {
                TinkerActive = false;
                TinkerProgress = 0;
                Bots += 1;
                SetTinkerButton();
            }
        }

        private void SetUI()
        {
            progressBar.fillAmount = TinkerProgress / _duration;
            progressText.text = $"<b>Time Remaining:</b> {GetTimeRemaining()}";
        }

        private void SetTinkerButton()
        {
            tinkerButton.interactable = !TinkerActive;
        }

        private string GetTimeRemaining()
        {
            var progressRemaining = _duration - TinkerProgress;


            if (TimeScale == 0 || !TinkerActive) return $"{ColourRed}N/A{EndColour}";
            return
                $"{(UseScaledTimeForValues ? FormatTime(progressRemaining, true) : FormatTime(progressRemaining / Math.Abs(TimeScale), true))}";
        }
    }
}
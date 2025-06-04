using System;
using Blindsided.SaveData;
using CollapseOfTimeNamespace;
using MPUIKIT;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CollapseOfTimeNamespace.CollapseOfTimeStaticReferences;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.TextColourStrings;
using static Blindsided.SaveData.StaticReferences;


namespace UpgradeSystem
{
    public class CotUpgradeReferences : MonoBehaviour
    {
        [SerializeField] public string guid;
        public TMP_Text TitleText;
        public TMP_Text TimeRemainingText;
        public Button Activatebutton;
        public TMP_Text activatebuttonText;
        public MPImage FillBar;

        public TMP_Text DescriptionText;

        public CotUpgradeSaveData cotUpgradeSaveData;
        public UpgradeSo upgrade;

        private double _cachedUpgradeCost;

        private void CacheCost()
        {
            _cachedUpgradeCost = upgrade.GetCurrentCost();
        }

        private void Start()
        {
            SetUpgradeData();
            SetButtonState();
            Activatebutton.onClick.AddListener(OnbuttonPressed);
            CacheCost();
            UpdateTitleText();
            SetTextsAndProgress();
            DescriptionText.text = upgrade.description;
        }

        private void OnbuttonPressed()
        {
            cotUpgradeSaveData.IsActive = !cotUpgradeSaveData.IsActive;
            SetButtonState();
        }

        private void SetButtonState()
        {
            if (cotUpgradeSaveData.IsActive)
            {
                CollapseOfTimeProductionManager.Instance.AddCotUpgradeReference(this);
                activatebuttonText.text = "Deactivate";
            }
            else
            {
                CollapseOfTimeProductionManager.Instance.RemoveCotUpgradeReference(this);
                activatebuttonText.text = "Activate";
            }
        }

        public void IncrementTimeInvested(double time)
        {
            cotUpgradeSaveData.TimeInvested += time;
            if (cotUpgradeSaveData.TimeInvested >= _cachedUpgradeCost)
            {
                UpgradeManager.Instance.IncrementUpgradeLevel(upgrade);
                cotUpgradeSaveData.TimeInvested -= _cachedUpgradeCost;
                CacheCost();
                UpdateTitleText();
            }

            SetTextsAndProgress();
        }

        private void SetTextsAndProgress()
        {
            var timeRemaining = (_cachedUpgradeCost - cotUpgradeSaveData.TimeInvested) * activeResearches /
                                researchMultiplier;
            var showDecimal = timeRemaining / Math.Abs(TimeScale) <
                              60;
            TimeRemainingText.text =
                $"<b>Time Remaining</b> | {ColourGreen}{FormatTimeRemaining(timeRemaining, showDecimal)}{EndColour}";
            FillBar.fillAmount = 1 - (float)(cotUpgradeSaveData.TimeInvested / _cachedUpgradeCost);
        }

        private void UpdateTitleText()
        {
            TitleText.text = $"<b>{upgrade.upgradeName}</b> | Level {upgrade.GetCurrentLevel()}";
        }


        private void SetUpgradeData()
        {
            CotUpgrades.TryAdd(guid, new CotUpgradeSaveData());
            cotUpgradeSaveData = CotUpgrades[guid];
        }

        [Button]
        public void CreateGuid()
        {
            if (string.IsNullOrEmpty(guid))
                guid = Guid.NewGuid().ToString();
            else
                Debug.Log("Guid Exists");
        }
    }
}
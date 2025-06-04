using System;
using TimeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Blindsided.SaveData.TextColourStrings;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.StaticReferences;
using static UpgradeSystem.CurrencyManager;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;
using static Blindsided.SaveData.SaveData;

namespace UpgradeSystem
{
    public class UpgradeReferences : MonoBehaviour
    {
        public TMP_Text upgradeName;
        public TMP_Text level;
        public TMP_Text cost;
        public TMP_Text description;

        public Button buyButton;
        public TMP_Text buyButtonText;
        public UpgradeSo upgrade;

        public bool updateTimeScale;
        public bool continueInvoke;
        public bool dontHideMaxed;

        public bool Affordable => GetCurrencyAmount(upgrade.costCurrencyType).Item1 >= Cost();

        private void OnEnable()
        {
            InvokeRepeating(nameof(SetButtonActive), 0, 0.1f);
            buyButton.onClick.AddListener(PurchaseUpgrade);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(SetButtonActive));
            buyButton.onClick.RemoveListener(PurchaseUpgrade);
        }

        private void Start()
        {
            SetActive();
        }

        private void SetButtonActive()
        {
            buyButton.interactable = Affordable && !upgrade.IsMaxed;
            SetTexts();
            if (upgrade.IsMaxed && !continueInvoke) CancelInvoke(nameof(SetButtonActive));
        }

        public string AffordableString => Affordable ? ColourHighlight : ColourRed;

        private void PurchaseUpgrade()
        {
            var tempCost = Cost();
            UpgradeManager.Instance.AddLevelsToUpgrade(upgrade, PurchaseAmount());
            RemoveCurrencyByType(tempCost, upgrade.costCurrencyType);
            SetTexts();
            SetButtonActive();
            SetActive();
            //CategorySetter.instance.UpdateExpanders();
            if (updateTimeScale) TimeManager.timeManager.SetTimeScale();
        }

        public void AutoPurchaseUpgrade()
        {
            var tempCost = Cost();
            UpgradeManager.Instance.AddLevelsToUpgrade(upgrade, PurchaseAmount(true));
            RemoveCurrencyByType(tempCost, upgrade.costCurrencyType);
            SetTexts();
            SetButtonActive();
            SetActive();
        }

        public void SetTexts()
        {
            UpgradeLevels.TryGetValue(upgrade.guid, out var upgradeLevel);
            var (amountOfCurrency, currencyString) = GetCurrencyAmount(upgrade.costCurrencyType);
            level.text =
                $"<b>Level</b> | <color=#98C560>{upgradeLevel}</color> / {(upgrade.maxPurchases >= 0 ? upgrade.maxPurchases.ToString() : "∞")}";
            cost.text =
                $"<b>Cost</b> | {(upgrade.IsMaxed ? "Maxed" : $"{AffordableString}-{FormatNumber(amountOfCurrency)}{EndColour}/ {AffordableString}-{FormatNumber(Cost())}{EndColour} {currencyString}")}";
            upgradeName.text = $"<b>{upgrade.upgradeName}</b>";
            description.text = upgrade.description;
            buyButtonText.text = upgrade.IsMaxed ? "Maxed" : $"Buy ({PurchaseAmount()})";
        }

        public void SetActive()
        {
            gameObject.SetActive(!(upgrade.IsMaxed && HideMaxedResearches && !dontHideMaxed));
        }


        public double Cost()
        {
            UpgradeLevels.TryGetValue(upgrade.guid, out var level);
            return BuyXCost(PurchaseAmount(), upgrade.baseCost, upgrade.costMultiplier, level);
        }

        public int PurchaseAmount(bool auto = false)
        {
            if (auto)
            {
                UpgradeLevels.TryGetValue(upgrade.guid, out var cl);
                var ml = upgrade.maxPurchases >= 0 ? upgrade.maxPurchases : int.MaxValue;
                var ttp = Math.Max(1,
                    MaxAffordable(GetCurrencyAmount(upgrade.costCurrencyType).Item1, upgrade.baseCost,
                        upgrade.costMultiplier, cl));
                return Math.Min(ttp, ml - cl);
            }

            UpgradeLevels.TryGetValue(upgrade.guid, out var currentLevel);
            var maxLevel = upgrade.maxPurchases >= 0 ? upgrade.maxPurchases : int.MaxValue;
            var toTryPurchase = PurchaseMode switch
            {
                BuyMode.Buy1 => 1,
                BuyMode.Buy10 => RoundedBulkBuy ? 10 - currentLevel % 10 : 10,
                BuyMode.Buy50 => RoundedBulkBuy ? 50 - currentLevel % 50 : 50,
                BuyMode.Buy100 => RoundedBulkBuy ? 100 - currentLevel % 100 : 100,
                BuyMode.BuyMax => Math.Max(1,
                    MaxAffordable(GetCurrencyAmount(upgrade.costCurrencyType).Item1, upgrade.baseCost,
                        upgrade.costMultiplier, currentLevel)),
                _ => 1
            };
            return Math.Min(toTryPurchase, maxLevel - currentLevel);
        }
    }
}
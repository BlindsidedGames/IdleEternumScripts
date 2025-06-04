using System;
using System.Collections.Generic;
using Blindsided.SaveData;
using FoundationOfProgressNameSpace.BuildingScriptables;
using UnityEngine;
using UnlockNexus;
using UpgradeSystem;
using static Blindsided.SaveData.StaticReferences;
using static Blindsided.SaveData.SaveData;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.TextColourStrings;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;
using EventHandler = Blindsided.EventHandler;
using static Blindsided.Oracle;

namespace FoundationOfProgressNameSpace
{
    public class Building : MonoBehaviour, IUpgradable
    {
        #region NewUpgradeSystem

        [SerializeField] private List<string> tags = new() { "FoPBuilding" };

        public List<string> GetTags()
        {
            return tags;
        }

        public UpgradableStat GetStat(StatType statType)
        {
            return buildingData.UpgradableStats.GetValueOrDefault(statType);
        }

        public void Register()
        {
            UpgradeManager.Instance?.RegisterEntity(this);
        }

        public void Unregister()
        {
            UpgradeManager.Instance?.UnregisterEntity(this);
        }


        private void NewStartItems()
        {
        }

        #endregion


        public virtual FopBuilding FopBuildingData
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public virtual FopBuilding FopBuildingCreatedData
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public virtual FopBuildingStats FopBuildingStatsData
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public virtual double RequiredCurrency
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public virtual double ProducedCurrency
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public virtual bool AutoBuy => throw new NotImplementedException();

        public BuildingReferences buildingReferences;

        public BuildingData buildingData;
        public double BaseCost => buildingData.baseCost;
        public double CostExponent => buildingData.costExponent;

        public double ContinuumMultiplier => Ascended
            ? ContinuumProductionBuff *
              (1 + Math.Log10(1 + FopBuildingData.NumberPurchased))
            : 1;

        public double CachedProduction => buildingData.UpgradableStats[StatType.FopProduction].CachedValue;
        public double CachedCreation => buildingData.UpgradableStats[StatType.FopCreation].CachedValue;

        public bool Creates => buildingData.creates;

        public double ProductionMultiplier =>
            ResurgenceProductionMultiplier * ContinuumMultiplier;

        public double Production => CachedProduction * FopBuildingData.NumberOwned * ProductionMultiplier;
        public double Creation => CachedCreation * FopBuildingData.NumberOwned * ResurgenceProductionMultiplier;

        public bool Affordable => RequiredCurrency >= Cost();
        public string AffordableString => Affordable ? ColourHighlight : ColourRed;

        public UnlockManager.UnlockKeys buildingUnlockKey;

        public void SetAutomatedBuildingsActive()
        {
            buildingReferences.gameObject.SetActive(!(AutoBuy && HideAutomatedBuildings) &&
                                                    oracle.saveData.Unlocks[buildingUnlockKey]);
        }

        private void OnEnable()
        {
            FoundationOfProductionEvents.UpdateUI += UpdateUI;
            FoundationOfProductionEvents.AutomatedBuildingsHideShow += SetAutomatedBuildingsActive;
            EventHandler.UpdateTextsForTimeScaleEvent += SetBuildingNameText;
            InvokeRepeating(nameof(AutoPurchaseBuildings), 0, 0.1f);
            Register();
        }

        private void OnDisable()
        {
            FoundationOfProductionEvents.UpdateUI -= UpdateUI;
            FoundationOfProductionEvents.AutomatedBuildingsHideShow -= SetAutomatedBuildingsActive;
            EventHandler.UpdateTextsForTimeScaleEvent -= SetBuildingNameText;
            CancelInvoke(nameof(AutoPurchaseBuildings));
            Unregister();
        }

        private void Start()
        {
            buildingData.CacheStats();
            NewStartItems();
            SetBuildingNameText();
            if (!Creates) return;
            buildingReferences.buyButton.onClick.AddListener(PurchaseBuildings);
            UpdateUI();
        }

        private void SetBuildingNameText()
        {
            var shake = Math.Abs(TimeScale) > 0 ? "" : "<shake>";
            buildingReferences.buildingNameAnimator.SetText($"{shake}{buildingData.buildingName}");
        }

        public virtual void Produce(float elapsedTime)
        {
            var p = Production * elapsedTime;
            var c = Creation * elapsedTime;
            ProducedCurrency += p;
            FopBuildingData.Produced += p;
            FopBuildingStatsData.TotalProduction += p;
            if (!Creates) return;
            FopBuildingCreatedData.NumberCreated += c;
            FopBuildingData.Created += c;
            FopBuildingStatsData.TotalCreated += c;
        }

        public string ProductionString =>
            $"<b>Produces</b> | {ColourGreen}{FormatProduction(Production)}{EndColour} {ColourGrey}{buildingData.ProducedCurrencyString(Math.Abs(Production - 1) > 0.001)} /s{EndColour}";

        public string CreationString => Creates
            ? $"\n<b>Creates</b> | {ColourGreen}{FormatProduction(Creation)}{EndColour} {ColourGrey}{buildingData.ProducedBuildingString(Math.Abs(Creation - 1) > 0.001)} /s{EndColour}"
            : "";

        private string VoidLullString => $"<b>Produces</b> | {ColourRed}N/A{EndColour}" +
                                         $"\n<b>Creates</b> | {ColourRed}N/A{EndColour}";

        public string OwnedString =>
            $"{ColourGreen}{FormatNumber(FopBuildingData.NumberOwned)}{EndColour}" +
            (Creates ? $" (+{ColourGreenAlt}{FormatNumber(FopBuildingData.NumberPurchased)}{EndColour})" : "");

        public virtual void UpdateUI()
        {
            buildingReferences.owned.text = OwnedString;
            buildingReferences.production.text =
                $"{(TimeScale == 0 || Production == 0 ? VoidLullString : ProductionString + CreationString)}";
            buildingReferences.additionalInfo.text =
                (Ascended
                    ? $"<b>Continuum Multiplier</b> | {colorHighlight}{FormatNumber(ContinuumMultiplier)}{EndColour}\n\n"
                    : "") +
                $"<b>{buildingData.ProducedCurrencyString(FopBuildingCreatedData.Produced != 1)} Produced</b> | {FormatNumber(FopBuildingData.Produced)}" +
                $"{(Creates ? $"\n<b>{buildingData.ProducedBuildingString(FopBuildingCreatedData.Created != 1)} Created</b> | {FormatNumber(FopBuildingData.Created)}" : "")}" +
                $"<line-height=120%>\n</line-height><b>Lifetime {buildingData.ProducedCurrencyString(FopBuildingStatsData.TotalProduction != 1)} Produced</b> | {FormatNumber(FopBuildingStatsData.TotalProduction)}" +
                $"{(Creates ? $"\n<b>Lifetime {buildingData.ProducedBuildingString(FopBuildingStatsData.TotalCreated != 1)} Created</b> | {FormatNumber(FopBuildingStatsData.TotalCreated)}" : "")}" +
                $"<line-height=120%>{(Creates ? $"\n</line-height><b>Lifetime Purchased</b> | {FormatNumber(FopBuildingStatsData.TotalPurchased)}" : "</line-height>")}";


            var UpgradesAndMath = $"{colorHighlight}<b>Production</b>{EndColour}" +
                                  $"\n{buildingData.UpgradableStats[StatType.FopProduction].CachedBreakdown}";
            if (Creates)
                UpgradesAndMath += $"\n{colorHighlight}<b>Creation</b>{EndColour}" +
                                   $"\n{buildingData.UpgradableStats[StatType.FopCreation].CachedBreakdown}" +
                                   $"\n\n{colorHighlight}<b>Cost Multiplier</b>{EndColour}" +
                                   $"\n{buildingData.UpgradableStats[StatType.FopCostMultiplier].CachedBreakdown}";

            buildingReferences.upgradesMathBaseProduction.text = UpgradesAndMath;
            if (!Creates) return;
            buildingReferences.buyButtonText.text = $"Buy ({PurchaseAmount()})";
            buildingReferences.buyButton.interactable = RequiredCurrency >= Cost();
            buildingReferences.cost.text =
                $"<b>Cost</b> | {AffordableString}{FormatNumber(RequiredCurrency)}{EndColour}/ {AffordableString}{FormatNumber(Cost())}{EndColour} {ColourGrey}{buildingData.RequiredCurrencyString(true)}";
        }

        public void PurchaseBuildings()
        {
            var tempCost = Cost();
            if (RequiredCurrency < tempCost) return;
            FopBuildingStatsData.TotalPurchased += PurchaseAmount();
            FopBuildingData.NumberPurchased += PurchaseAmount();
            RequiredCurrency -= tempCost;
            UpdateUI();
        }

        public double Cost()
        {
            return BuyXCost(PurchaseAmount(), BaseCost, CostExponent, FopBuildingData.NumberPurchased,
                buildingData.UpgradableStats[StatType.FopCostMultiplier].CachedValue);
        }

        public int PurchaseAmount()
        {
            return PurchaseMode switch
            {
                BuyMode.Buy1 => 1,
                BuyMode.Buy10 => RoundedBulkBuy ? 10 - (int)Math.Floor(FopBuildingData.NumberPurchased % 10) : 10,
                BuyMode.Buy50 => RoundedBulkBuy ? 50 - (int)Math.Floor(FopBuildingData.NumberPurchased % 50) : 50,
                BuyMode.Buy100 => RoundedBulkBuy ? 100 - (int)Math.Floor(FopBuildingData.NumberPurchased % 100) : 100,
                BuyMode.BuyMax => Math.Max(1,
                    MaxAffordable(RequiredCurrency, BaseCost, CostExponent, FopBuildingData.NumberPurchased,
                        buildingData.UpgradableStats[StatType.FopCostMultiplier].CachedValue)),
                _ => 1
            };
        }

        public void AutoPurchaseBuildings()
        {
            if (RequiredCurrency < Cost() || !AutoBuy || !AutoBuyEnabled) return;
            var tempCost = Cost();
            double a = AutoBuyAmount;
            FopBuildingStatsData.TotalPurchased += a;
            FopBuildingData.NumberPurchased += a;
            RequiredCurrency -= tempCost;
            UpdateUI();
        }

        public int AutoBuyAmount => Math.Max(1,
            MaxAffordable(RequiredCurrency, BaseCost, CostExponent, FopBuildingData.NumberPurchased));
    }
}
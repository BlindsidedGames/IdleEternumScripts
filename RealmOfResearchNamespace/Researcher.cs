using System;
using Blindsided.SaveData;
using FoundationOfProgressNameSpace;
using RealmOfResearchNamespace.ResearchScriptables;
using UnityEngine;
using static Blindsided.SaveData.StaticReferences;
using static Blindsided.SaveData.SaveData;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.TextColourStrings;
using EventHandler = Blindsided.EventHandler;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace RealmOfResearchNamespace
{
    public class Researcher : MonoBehaviour
    {
        public virtual RorBuilding BuildingData
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public ResearcherType researcherType;
        public FopConsumptionManager fopConsumptionManager;

        public virtual RorBuilding BuildingCreatedData
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

        /*public virtual string ProducedCurrencyName => throw new NotImplementedException();
        public virtual string RequiredCurrencyName => throw new NotImplementedException();
        public virtual string ProducedBuildingName => throw new NotImplementedException();*/

        public BuildingReferences buildingReferences;
        //public BuildingReferences buildingCreatedReferences;

        public ResearchData buildingData;
        public double BaseCost => buildingData.baseCost;
        public double CostExponent => buildingData.costExponent;

        public double BaseProduction => buildingData.baseProduction;
        public double BaseCreation => buildingData.baseCreation;

        public bool Creates => buildingData.creates;

        public double ConsumptionBuff => 1 + fopConsumptionManager.GetResearcherMultiplier(researcherType);

        public double Production => BaseProduction * BuildingData.NumberOwned * ContinuumRealmOfResearchMultiplier *
                                    ConsumptionBuff;

        public double Creation => BaseCreation * BuildingData.NumberOwned * ContinuumRealmOfResearchMultiplier *
                                  ConsumptionBuff;

        public bool Affordable => RequiredCurrency >= Cost();
        public string AffordableString => Affordable ? ColourHighlight : ColourRed;


        private void OnEnable()
        {
            RealmOfResearchEvents.UpdateUI += UpdateUI;
            EventHandler.UpdateTextsForTimeScaleEvent += SetBuildingNameText;
            InvokeRepeating(nameof(AutoPurchaseBuildings), 0, 0.1f);
        }

        private void OnDisable()
        {
            RealmOfResearchEvents.UpdateUI -= UpdateUI;
            EventHandler.UpdateTextsForTimeScaleEvent -= SetBuildingNameText;
            CancelInvoke(nameof(AutoPurchaseBuildings));
        }

        private void Start()
        {
            SetBuildingNameText();
            UpdateUI();
            if (!Creates) return;
            buildingReferences.buyButton.onClick.AddListener(PurchaseBuildings);
        }

        private void SetBuildingNameText()
        {
            var shake = Math.Abs(TimeScale) > 0 ? "" : "<shake>";
            buildingReferences.buildingNameAnimator.SetText($"{shake}{buildingData.researcherName}");
        }

        public virtual void Produce(float elapsedTime)
        {
            if (Creates)
                BuildingCreatedData.NumberCreated += Creation * elapsedTime;
            else
                ProducedCurrency += Production * elapsedTime;
        }

        public string ProductionString =>
            $"<b>Produces</b> | {ColourGreen}-{FormatNumber(UseScaledTimeForValues ? Production : Production * Math.Abs(TimeScale))}{EndColour} {ColourGrey}{buildingData.ProducedCurrencyString(Math.Abs(Production - 1) > 0.001)} /s{EndColour}";

        public string CreationString => Creates
            ? $"<b>Creates</b> | {ColourGreen}-{FormatNumber(UseScaledTimeForValues ? Creation : Creation * Math.Abs(TimeScale))}{EndColour} {ColourGrey}{buildingData.ProducedBuildingString(Math.Abs(Creation - 1) > 0.001)} /s{EndColour}"
            : "";

        public string OwnedString =>
            $"{ColourGreen}-{FormatNumber(BuildingData.NumberOwned)}{EndColour}" +
            (Creates ? $" ({ColourGreenAlt}-{FormatNumber(BuildingData.NumberPurchased)}{EndColour})" : "");

        public virtual void UpdateUI()
        {
            buildingReferences.owned.text = OwnedString;
            buildingReferences.production.text = $"{(Creates ? CreationString : ProductionString)}";
            buildingReferences.extraInfoUnderCost.text =
                fopConsumptionManager.GetResearcherMultiplierString(researcherType, buildingData.creates);
            if (!Creates) return;
            buildingReferences.buyButtonText.text = $"Buy (-{PurchaseAmount()})";
            buildingReferences.buyButton.interactable = RequiredCurrency >= Cost();
            buildingReferences.cost.text =
                $"<b>Cost</b> | {AffordableString}-{FormatNumber(RequiredCurrency)}{EndColour}/ {AffordableString}-{FormatNumber(Cost())}{EndColour} {ColourGrey}{buildingData.RequiredCurrencyString(true)}";
        }

        public void PurchaseBuildings()
        {
            if (RequiredCurrency < Cost()) return;
            var tempCost = Cost();
            BuildingData.NumberPurchased += PurchaseAmount();
            RequiredCurrency -= tempCost;
            RealmOfResearchEvents.OnUpdateUI();
        }

        public double Cost()
        {
            return BuyXCost(PurchaseAmount(), BaseCost, CostExponent, BuildingData.NumberPurchased);
        }

        public int PurchaseAmount()
        {
            return PurchaseMode switch
            {
                BuyMode.Buy1 => 1,
                BuyMode.Buy10 => RoundedBulkBuy ? 10 - (int)Math.Floor(BuildingData.NumberPurchased % 10) : 10,
                BuyMode.Buy50 => RoundedBulkBuy ? 50 - (int)Math.Floor(BuildingData.NumberPurchased % 50) : 50,
                BuyMode.Buy100 => RoundedBulkBuy ? 100 - (int)Math.Floor(BuildingData.NumberPurchased % 100) : 100,
                BuyMode.BuyMax => Math.Max(1,
                    MaxAffordable(RequiredCurrency, BaseCost, CostExponent, BuildingData.NumberPurchased)),
                _ => 1
            };
        }

        public void AutoPurchaseBuildings()
        {
            if (RequiredCurrency < Cost() || !AutoBuy) return;
            var tempCost = Cost();
            BuildingData.NumberPurchased += AutoBuyAmount;
            RequiredCurrency -= tempCost;
            UpdateUI();
        }

        public int AutoBuyAmount => Math.Max(1,
            MaxAffordable(RequiredCurrency, BaseCost, CostExponent, BuildingData.NumberPurchased));
    }
}
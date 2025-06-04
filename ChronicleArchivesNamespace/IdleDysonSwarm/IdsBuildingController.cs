using System;
using System.Collections.Generic;
using ChronicleArchivesNamespace.IdleDysonSwarm.IdsScriptables;
using UnityEngine;
using UpgradeSystem;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.SaveData;
using static Blindsided.SaveData.StaticReferences;
using static ChronicleArchivesNamespace.IdleDysonSwarm.IdsStaticReferences;
using static Blindsided.SaveData.TextColourStrings;

namespace ChronicleArchivesNamespace.IdleDysonSwarm
{
    public class IdsBuildingController : MonoBehaviour, IUpgradable
    {
        public IdsBuildingData buildingData;
        public IdsFacilityReferences facilityReferences;

        public double Purchased
        {
            get => buildingData.BuildingSaveData.PurchaseData.Purchased;
            set => buildingData.BuildingSaveData.PurchaseData.Purchased = value;
        }

        private double Production => buildingData.UpgradableStats[StatType.IdsProduction].CachedValue *
                                     buildingData.BuildingSaveData.PurchaseData.Count;

        public bool producesBots;


        private void OnEnable()
        {
            IdsEvents.UpdateUI += UpdateTexts;
            Register();
        }

        private void OnDisable()
        {
            IdsEvents.UpdateUI -= UpdateTexts;
            Unregister();
        }

        private void Start()
        {
            buildingData.CacheStats();
            facilityReferences.buyButton.onClick.AddListener(Buy);
            facilityReferences.Name.text = buildingData.BuildingName;
            UpdateTexts();
            Register();
        }

        private void Update()
        {
            facilityReferences.buyButton.interactable = Affordable;
        }

        public void Produce(float timeScale)
        {
            if (producesBots)
                Bots += Production * timeScale;
            else
                buildingData.ProducedBuilding.BuildingSaveData.PurchaseData.Created += Production * timeScale;
        }

        #region Texts

        public bool Affordable => Cash >= Cost();
        public string AffordableString => Affordable ? ColourHighlight : ColourRed;

        private void UpdateTexts()
        {
            facilityReferences.production.text =
                $"<b>Producing</b> | {ColourGreen}{FormatProduction(Production)}{EndColour} {ColourGrey}/s";
            facilityReferences.cost.text =
                $"<b>Cost</b> | ${AffordableString}{FormatNumber(Cash)}{EndColour}/ {AffordableString}{FormatNumber(Cost())}{EndColour} {ColourGrey}";
            UpdateAmountText();
            facilityReferences.buyButtonText.text = $"Buy ({PurchaseAmount()})";
        }

        private void UpdateAmountText()
        {
            facilityReferences.owned.text =
                $"{ColourGreen}{FormatNumber(buildingData.BuildingSaveData.PurchaseData.Count)}{EndColour}(+{ColourGreenAlt}{FormatNumber(buildingData.BuildingSaveData.PurchaseData.Purchased)}{EndColour})";
            ;
        }

        #endregion

        #region Purchasing

        public void Buy()
        {
            var purchaseAmount = PurchaseAmount();
            if (Cash >= Cost())
            {
                Cash -= Cost();
                buildingData.BuildingSaveData.PurchaseData.Purchased += purchaseAmount;
                IdsEvents.OnUpdateUI();
            }
        }

        public double Cost()
        {
            return BuyXCost(PurchaseAmount(), buildingData.BaseCost, buildingData.CostExponent, Purchased);
        }

        public int PurchaseAmount()
        {
            return PurchaseMode switch
            {
                BuyMode.Buy1 => 1,
                BuyMode.Buy10 => RoundedBulkBuy ? 10 - (int)Math.Floor(Purchased % 10) : 10,
                BuyMode.Buy50 => RoundedBulkBuy ? 50 - (int)Math.Floor(Purchased % 50) : 50,
                BuyMode.Buy100 => RoundedBulkBuy ? 100 - (int)Math.Floor(Purchased % 100) : 100,
                BuyMode.BuyMax => Math.Max(1,
                    MaxAffordable(Cash, buildingData.BaseCost, buildingData.CostExponent, Purchased)),
                _ => 1
            };
        }

        #endregion


        public List<string> GetTags()
        {
            return buildingData.tags;
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
            UpgradeManager.Instance.UnregisterEntity(this);
        }
    }
}
using System;
using Blindsided.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlockNexus;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.TextColourStrings;
using EventHandler = Blindsided.EventHandler;
using static Blindsided.Oracle;

namespace FoundationOfProgressNameSpace.Prestige
{
    public class PrestigeManager : MonoBehaviour
    {
        private void OnEnable()
        {
            EventHandler.OnUnlockNexusEvent += ShowOrHideUpgrades;
        }

        private void OnDisable()
        {
            EventHandler.OnUnlockNexusEvent -= ShowOrHideUpgrades;
        }

        private void Start()
        {
            ResurgenceListeners();
            ContinuumListeners();
            ShowOrHideUpgrades();
            CalculateModifiers();
        }

        private void Update()
        {
            ResurgenceUnlockButtonEnablers();
            ContinuumUnlockButtonEnablers();
            SetResurgenceTexts();
            SetContinuumTexts();
            AutoPrestige();
        }

        //Prestige One
        public ResurgenceUnlockReferences additionalBuildings;
        public ResurgenceUnlockReferences autoBuyNebulaGenerator;
        public ResurgenceUnlockReferences autoBuyCelestialFoundry;
        public ResurgenceUnlockReferences autoBuyNovaSpire;
        public ResurgenceUnlockReferences autoBuyGalacticNexus;
        public ResurgenceUnlockReferences autoBuySingularityLoom;
        public ResurgenceUnlockReferences autoBuyEternityBeacon;

        public ResurgenceUnlockReferences autoBuyInfinityCrucible;

        private const double minCradles = 7e6;
        private const double prestigeExponent = 1.06;

        public double ResurgenceEnergyToGain => MaxAffordable(baseCost: minCradles, exponent: prestigeExponent,
            currentLevel: 0, currencyOwned: StarCradles);

        public double AdditionalCradlesRequired =>
            BuyXCost(1, minCradles, prestigeExponent, ResurgenceEnergyToGain) -
            (StarCradles - BuyXCost(ResurgenceEnergyToGain, minCradles, prestigeExponent, 0));


        public TMP_Text resurgenceInfoText;
        public TMP_Text resurgenceMultiplierText;
        public Button resurgenceButton;

        private double ResurgenceProductionCalculation => 1 + ResurgenceEnergy * 0.01;


        private void AutoPrestige()
        {
            if (ResurgenceEnergyToGain < AutoPrestigeSavedValue || !AutoPrestigeEnabled) return;
            Prestige();
        }

        private void Prestige()
        {
            Prestiged = true;
            TimesPrestiged++;
            ResurgenceEnergy += ResurgenceEnergyToGain;
            ResurgenceResetData = new ResurgenceResets();
            CalculateModifiers();
            FoundationOfProductionEvents.OnPrestigeOne();
        }

        private void SetResurgenceTexts()
        {
            resurgenceInfoText.text =
                $"<b>Resurgence Energy</b> | {ColourGreen}{FormatNumber(ResurgenceEnergy)}{EndColour}" +
                $"\n<b>Gain</b> | {ColourGreenAlt}{FormatNumber(ResurgenceEnergyToGain)}{EndColour}" +
                "\n<b>Next In</b> | " + ColourOrange + FormatNumber(AdditionalCradlesRequired) + EndColour +
                $" {ColourGrey}Star Cradles{EndColour}";
            resurgenceMultiplierText.text =
                $"<b>Production Multiplier</b> | {ColourGreen}{ResurgenceProductionMultiplier}";
        }

        private void ShowOrHideUpgrades()
        {
            var celestialFoundry = !Ascendancy.AutobuyCelestialFoundry &&
                                   oracle.saveData.Unlocks[UnlockManager.UnlockKeys.CelestialFoundry];
            var novaSpire = !Ascendancy.AutobuyNovaSpire && oracle.saveData.Unlocks[UnlockManager.UnlockKeys.NovaSpire];
            var galacticNexus = !Ascendancy.AutobuyGalacticNexus &&
                                oracle.saveData.Unlocks[UnlockManager.UnlockKeys.GalacticNexus];
            var singularityLoom = !Ascendancy.AutobuySingularityLoom &&
                                  oracle.saveData.Unlocks[UnlockManager.UnlockKeys.SingularityLoom];
            var eternityBeacon = !Ascendancy.AutobuyEternityBeacon &&
                                 oracle.saveData.Unlocks[UnlockManager.UnlockKeys.EternityBeacon];
            var infinityCrucible = !Ascendancy.AutobuyInfinityCrucible &&
                                   oracle.saveData.Unlocks[UnlockManager.UnlockKeys.InfinityCrucible];

            additionalBuildings.gameObject.SetActive(false);
            autoBuyNebulaGenerator.gameObject.SetActive(!Ascendancy.AutobuyNebulaGenerator);
            autoBuyCelestialFoundry.gameObject.SetActive(celestialFoundry);
            autoBuyNovaSpire.gameObject.SetActive(novaSpire);
            autoBuyGalacticNexus.gameObject.SetActive(galacticNexus);
            autoBuySingularityLoom.gameObject.SetActive(singularityLoom);
            autoBuyEternityBeacon.gameObject.SetActive(eternityBeacon);
            autoBuyInfinityCrucible.gameObject.SetActive(infinityCrucible);
            FoundationOfProductionEvents.OnAutomatedBuildingsHideShow();
        }

        private void ResurgenceListeners()
        {
            additionalBuildings.unlockButton.onClick.AddListener(() => UnlockAdditionalBuildings());
            autoBuyNebulaGenerator.unlockButton.onClick.AddListener(() => UnlockAutoBuyNebulaGenerator());
            autoBuyCelestialFoundry.unlockButton.onClick.AddListener(() => UnlockAutoBuyCelestialFoundry());
            autoBuyNovaSpire.unlockButton.onClick.AddListener(() => UnlockAutoBuyNovaSpire());
            autoBuyGalacticNexus.unlockButton.onClick.AddListener(() => UnlockAutoBuyGalacticNexus());
            autoBuySingularityLoom.unlockButton.onClick.AddListener(() => UnlockAutoBuySingularityLoom());
            autoBuyEternityBeacon.unlockButton.onClick.AddListener(() => UnlockAutoBuyEternityBeacon());
            autoBuyInfinityCrucible.unlockButton.onClick.AddListener(() => UnlockAutoBuyInfinityCrucible());
            resurgenceButton.onClick.AddListener(Prestige);
        }


        private void ResurgenceUnlockButtonEnablers()
        {
            resurgenceButton.interactable = ResurgenceEnergyToGain > 0;
            if (Ascendancy.AllOwned) return;
            additionalBuildings.unlockButton.interactable = ResurgenceEnergy >= additionalBuildings.cost;
            autoBuyNebulaGenerator.unlockButton.interactable = ResurgenceEnergy >= autoBuyNebulaGenerator.cost;
            autoBuyCelestialFoundry.unlockButton.interactable = ResurgenceEnergy >= autoBuyCelestialFoundry.cost;
            autoBuyNovaSpire.unlockButton.interactable = ResurgenceEnergy >= autoBuyNovaSpire.cost;
            autoBuyGalacticNexus.unlockButton.interactable = ResurgenceEnergy >= autoBuyGalacticNexus.cost;
            autoBuySingularityLoom.unlockButton.interactable = ResurgenceEnergy >= autoBuySingularityLoom.cost;
            autoBuyEternityBeacon.unlockButton.interactable = ResurgenceEnergy >= autoBuyEternityBeacon.cost;
            autoBuyInfinityCrucible.unlockButton.interactable = ResurgenceEnergy >= autoBuyInfinityCrucible.cost;
        }

        private void UnlockAdditionalBuildings()
        {
            if (ResurgenceEnergy < additionalBuildings.cost) return;
            AdditionalBuildings = true;
            ResurgenceEnergy -= additionalBuildings.cost;
            CalculateModifiers();
            ShowOrHideUpgrades();
        }

        private void UnlockAutoBuyInfinityCrucible()
        {
            if (ResurgenceEnergy < autoBuyInfinityCrucible.cost) return;
            Ascendancy.AutobuyInfinityCrucible = true;
            ResurgenceEnergy -= autoBuyInfinityCrucible.cost;
            CalculateModifiers();
            ShowOrHideUpgrades();
        }

        private void UnlockAutoBuyEternityBeacon()
        {
            if (ResurgenceEnergy < autoBuyEternityBeacon.cost) return;
            Ascendancy.AutobuyEternityBeacon = true;
            ResurgenceEnergy -= autoBuyEternityBeacon.cost;
            CalculateModifiers();
            ShowOrHideUpgrades();
        }

        private void UnlockAutoBuySingularityLoom()
        {
            if (ResurgenceEnergy < autoBuySingularityLoom.cost) return;
            Ascendancy.AutobuySingularityLoom = true;
            ResurgenceEnergy -= autoBuySingularityLoom.cost;
            CalculateModifiers();
            ShowOrHideUpgrades();
        }

        private void UnlockAutoBuyGalacticNexus()
        {
            if (ResurgenceEnergy < autoBuyGalacticNexus.cost) return;
            Ascendancy.AutobuyGalacticNexus = true;
            ResurgenceEnergy -= autoBuyGalacticNexus.cost;
            CalculateModifiers();
            ShowOrHideUpgrades();
        }

        private void UnlockAutoBuyNovaSpire()
        {
            if (ResurgenceEnergy < autoBuyNovaSpire.cost) return;
            Ascendancy.AutobuyNovaSpire = true;
            ResurgenceEnergy -= autoBuyNovaSpire.cost;
            CalculateModifiers();
            ShowOrHideUpgrades();
        }

        private void UnlockAutoBuyCelestialFoundry()
        {
            if (ResurgenceEnergy < autoBuyCelestialFoundry.cost) return;
            Ascendancy.AutobuyCelestialFoundry = true;
            ResurgenceEnergy -= autoBuyCelestialFoundry.cost;
            CalculateModifiers();
            ShowOrHideUpgrades();
        }

        private void UnlockAutoBuyNebulaGenerator()
        {
            if (ResurgenceEnergy < autoBuyNebulaGenerator.cost) return;
            Ascendancy.AutobuyNebulaGenerator = true;
            ResurgenceEnergy -= autoBuyNebulaGenerator.cost;
            CalculateModifiers();
            ShowOrHideUpgrades();
        }
        //Ascendancy

        private const double minShards = 100;
        private const double ascendancyExponent = 1.5;

        public double ContinuumShardsToGain => MaxAffordable(baseCost: minShards, exponent: ascendancyExponent,
            currentLevel: 0, currencyOwned: FoundationOfProductionStaticReferences.FopCurrencies.TemporalShards);

        public double AdditionalShardsRequired =>
            BuyXCost(1, minShards, ascendancyExponent, ContinuumShardsToGain) -
            (FoundationOfProductionStaticReferences.FopCurrencies.TemporalShards -
             BuyXCost(ContinuumShardsToGain, minShards, ascendancyExponent, 0));

        public TMP_Text ascendancyInfoText;
        public TMP_Text ascendancyExponentText;
        public TMP_Text realmOfResearchText;
        public Button ascendancyButton;

        private void SetContinuumTexts()
        {
            ascendancyInfoText.text =
                $"<b>Continuum Shards</b> | {ColourGreen}{FormatNumber(ContinuumShards)}{EndColour}" +
                $"\n<b>Gain</b> | {ColourGreenAlt}{FormatNumber(ContinuumShardsToGain)}{EndColour}" +
                "\n<b>Next In</b> | " + ColourGreen + FormatNumber(AdditionalShardsRequired) + EndColour +
                $" {ColourGrey}Temporal Shards{EndColour}";
            ascendancyExponentText.text =
                $"<b>Continuum Multiplier</b> | {ColourGreen}{FormatNumber(ContinuumProductionBuff)}";
            realmOfResearchText.text =
                $"<b>Realm of Research Multiplier</b> | {ColourGreen}{FormatNumber(ContinuumRealmOfResearchMultiplier)}";
        }

        private void ContinuumListeners()
        {
            ascendancyButton.onClick.AddListener(Ascend);
        }

        private void ContinuumUnlockButtonEnablers()
        {
            ascendancyButton.interactable = ContinuumShardsToGain > 0;
        }

        private double ContinuumProductionCalculation => 1 + Math.Log(ContinuumShards + 1, 2);
        private double ContinuumRealmOfResearchCalculation => 1 + ContinuumShards * 0.01;

        private void Ascend()
        {
            Ascended = true;
            TimesAscended++;
            ContinuumShards += ContinuumShardsToGain;
            Ascendancy.Prestige = new FopPrestige();
            CalculateModifiers();
            FoundationOfProductionEvents.OnPrestigeTwo();
        }

        public void CalculateModifiers()
        {
            ContinuumProductionBuff = ContinuumProductionCalculation;
            ContinuumRealmOfResearchMultiplier = ContinuumRealmOfResearchCalculation;
            ResurgenceProductionMultiplier = ResurgenceProductionCalculation;
        }
    }
}
using System;
using System.Collections.Generic;
using MPUIKIT;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;
using static ChronicleArchivesNamespace.IdleDysonSwarm.IdsStaticReferences;
using static Blindsided.SaveData.TextColourStrings;
using static Blindsided.Utilities.CalcUtils;

namespace ChronicleArchivesNamespace.IdleDysonSwarm
{
    public class IdsBotManager : SerializedMonoBehaviour, IUpgradable
    {
        public TMP_Text leftInfoString;
        public TMP_Text rightInfoString;
        public TMP_Text workerInfoString;
        public TMP_Text scienceInfoString;
        public Slider distributionSlider;
        public TMP_Text workerDistributionString;
        public TMP_Text scienceDistributionString;

        public TMP_Text meaningOfLifeText;
        public MPImage infinityProgressBar;
        public TMP_Text infinityProgressText;

        private double _workers;
        private double _scientists;


        private double _panelsPerSecond => _workers / 100;
        private double _activePanels => _panelsPerSecond * PanelLifetime;

        private double _infinityProgress => Math.Log(Bots, 2) / Math.Log(RequiredBotsForInfinity, 2);

        public List<string> tags = new() { "IdsBM" };
        public Dictionary<StatType, UpgradableStat> UpgradableStats = new();
        private float PanelLifetime => (float)UpgradableStats[StatType.IdsPanelLifetime].CachedValue;
        private double ResearchPerBot => UpgradableStats[StatType.IdsResearchPerBot].CachedValue;

        public void CacheStats()
        {
            foreach (var stat in UpgradableStats) stat.Value.CacheStat();
        }

        [Button]
        public void ApplyBaseStats()
        {
            if (!UpgradableStats.TryAdd(StatType.IdsPanelLifetime, new UpgradableStat { baseValue = 10 }))
                UpgradableStats[StatType.IdsPanelLifetime].baseValue = 10;

            if (!UpgradableStats.TryAdd(StatType.IdsResearchPerBot, new UpgradableStat { baseValue = 1 }))
                UpgradableStats[StatType.IdsResearchPerBot].baseValue = 1;
            if (!UpgradableStats.TryAdd(StatType.IdsCashMultiplier, new UpgradableStat { baseValue = 1 }))
                UpgradableStats[StatType.IdsCashMultiplier].baseValue = 1;
            if (!UpgradableStats.TryAdd(StatType.IdsScienceMultiplier, new UpgradableStat { baseValue = 1 }))
                UpgradableStats[StatType.IdsScienceMultiplier].baseValue = 1;
        }

        private void OnEnable()
        {
            IdsEvents.UpdateUI += SetUI;
            IdsEvents.Infinity += SetSliderDistribution;
            Register();
        }

        private void OnDisable()
        {
            IdsEvents.UpdateUI -= SetUI;
            IdsEvents.Infinity -= SetSliderDistribution;
            Unregister();
        }

        private void Start()
        {
            CacheStats();
            distributionSlider.onValueChanged.AddListener(UpdateDistributionSlider);
            SetSliderDistribution();
            SetUI();
        }

        private void SetSliderDistribution()
        {
            distributionSlider.value = BotDistribution;
        }

        private void SetUI()
        {
            UpdateDistribution();
            SetStrings();
            infinityProgressBar.fillAmount = (float)_infinityProgress;
        }

        private void SetStrings()
        {
            leftInfoString.text =
                $"<b>Panel Lifetime</b> | {ColourHighlight}{FormatProduction(PanelLifetime, true)}{EndColour} {ColourGrey}Seconds{EndColour}" +
                $"\n<b>Bots</b> | {ColourHighlight}{FormatNumber(Bots)}";
            rightInfoString.text = $"<b>Active Panels</b> | {ColourHighlight}{FormatNumber(_activePanels)}{EndColour}" +
                                   $"\n<b>Total Panels Decayed</b> | {ColourHighlight}{FormatNumber(PanelsDecayed)}";

            workerInfoString.text =
                $"<b>Workers</b> | {ColourGreen}{FormatNumber(_workers)}{EndColour} {ColourGrey}Producing {ColourHighlight}{FormatProduction(_panelsPerSecond)}{EndColour} Panels /s{EndColour}";
            scienceInfoString.text =
                $"<b>Scientists</b> | {ColourGreen}{FormatNumber(_scientists)}{EndColour} {ColourGrey}Producing {ColourHighlight}{FormatProduction(_scientists)}{EndColour} Science /s{EndColour}";
            infinityProgressText.text = $"<b>Progress</b> | {ColourGreen}{_infinityProgress:P2}{EndColour}";
            meaningOfLifeText.text = DisplayInfinityText(InfinityPoints);
        }

        private string DisplayInfinityText(int infinities)
        {
            var index = Mathf.Clamp(infinities, 0, secretReveals.Length - 1);
            return "<b>Meaning of Life</b> | " + secretReveals[index];
        }

        public void ProduceCashAndScience(float timeScale)
        {
            Cash += _activePanels * GetStat(StatType.IdsCashMultiplier).CachedValue * timeScale;
            Science += _scientists * ResearchPerBot * GetStat(StatType.IdsScienceMultiplier).CachedValue * timeScale;
            PanelsDecayed += _panelsPerSecond * timeScale;
        }

        public void UpdateDistributionSlider(float value)
        {
            BotDistribution = value;
        }

        public void UpdateDistribution()
        {
            var flooredBots = Math.Floor(Bots);
            _workers = Math.Floor((1 - BotDistribution) * flooredBots);
            _scientists = Math.Ceiling(BotDistribution * flooredBots);
            workerDistributionString.text = $"{1 - BotDistribution:P0}";
            scienceDistributionString.text = $"{BotDistribution:P0}";
        }

        private readonly string[] secretReveals =
        {
            "_____ _______ ___ ____________",
            "L____ _______ ___ ____________",
            "Lo___ _______ ___ ____________",
            "Lov__ _______ ___ ____________",
            "Love_ _______ ___ ____________",
            "Love, _______ ___ ____________",
            "Love, F______ ___ ____________",
            "Love, Fa_____ ___ ____________",
            "Love, Fam____ ___ ____________",
            "Love, Fami___ ___ ____________",
            "Love, Famil__ ___ ____________",
            "Love, Family_ ___ ____________",
            "Love, Family, ___ ____________",
            "Love, Family, a__ ____________",
            "Love, Family, an_ ____________",
            "Love, Family, and ____________",
            "Love, Family, and ___________s",
            "Love, Family, and __________ls",
            "Love, Family, and _________als",
            "Love, Family, and ________tals",
            "Love, Family, and _______ntals",
            "Love, Family, and ______entals",
            "Love, Family, and _____mentals",
            "Love, Family, and ____ementals",
            "Love, Family, and ___rementals",
            "Love, Family, and __crementals",
            "Love, Family, and _ncrementals",
            "Love, Family, and Incrementals"
        };

        public List<string> GetTags()
        {
            return tags;
        }

        public UpgradableStat GetStat(StatType statType)
        {
            return UpgradableStats.GetValueOrDefault(statType);
        }

        public void Register()
        {
            UpgradeManager.Instance.RegisterEntity(this);
        }

        public void Unregister()
        {
            UpgradeManager.Instance.UnregisterEntity(this);
        }
    }
}
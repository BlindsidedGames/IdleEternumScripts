using System;
using System.Collections.Generic;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CollapseOfTimeNamespace.CollapseOfTimeStaticReferences;
using static Blindsided.SaveData.TextColourStrings;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.StaticReferences;
using Random = System.Random;

namespace CollapseOfTimeNamespace
{
    public class EntropyNexus : MonoBehaviour
    {
        public TMP_Text nameText;
        public TMP_Text tierText;
        public TMP_Text fillbarText;
        public MPImage fillbar;
        public TMP_Text temporalWorkerText;
        public TMP_Text researchDistributionText;
        public TMP_Text entropyDistributionText;
        public Slider distributionSlider;
        public TMP_Text buttonText;
        public Button toggleButton;
        public List<EntropyTier> entropyTiers = new();
        private readonly Random random = new();

        public double UpgradeChance => GetCurrentTier().UpgradeChance;
        private double TotalBuff => Math.Log10(TemporalWorkers + 1);

        public double ResearchBuff
        {
            get => researchMultiplier;
            set => researchMultiplier = value;
        }

        public double entropyBuff;

        private void Start()
        {
            toggleButton.onClick.AddListener(Toggle);
            SetButtonText();
            distributionSlider.onValueChanged.AddListener(UpdateDistributionSlider);
            UpdateDistribution();
            SetSliderDistribution();
        }

        private void SetSliderDistribution()
        {
            distributionSlider.value = WorkerDistribution;
        }

        public void UpdateDistributionSlider(float value)
        {
            WorkerDistribution = value;
            UpdateDistribution();
        }

        public void UpdateDistribution()
        {
            var totalBuff = TotalBuff;
            ResearchBuff = (1 - WorkerDistribution) * totalBuff;
            entropyBuff = WorkerDistribution * totalBuff;
            researchDistributionText.text = $"<b>Research</b> | {ColourHighlight}{ResearchBuff:P0}";
            entropyDistributionText.text = $"<b>Entropy Nexus</b> | {ColourHighlight}{entropyBuff:P0}";
        }

        private EntropyTier GetCurrentTier()
        {
            return entropyTiers[EntropyTierSaveData];
        }


        public void Produce(float time)
        {
            CurrentEntropyProgress += time * entropyBuff;
            if (CurrentEntropyProgress >= GetCurrentTier().FillTime)
            {
                var timesToProduce = (int)(CurrentEntropyProgress / GetCurrentTier().FillTime);
                for (var i = 0; i < timesToProduce; i++)
                {
                    CurrentEntropyProgress -= GetCurrentTier().FillTime;
                    switch (UpgradeVsGrow)
                    {
                        case false: //Upgrade
                            var currentTierUpgradeChance = UpgradeChance;
                            if (currentTierUpgradeChance < 0) break;
                            var finalUpgradeChance = currentTierUpgradeChance * entropyBuff;
                            finalUpgradeChance = Math.Max(0.0, Math.Min(1.0, finalUpgradeChance));

                            if (random.NextDouble() < finalUpgradeChance)
                            {
                                // SUCCESS: Increment the tier level
                                EntropyTierSaveData++;
                                Console.WriteLine($"Upgrade Successful! Now at Tier {EntropyTierSaveData}");
                                // Optional: Reset progress after successful upgrade?
                                // CurrentEntropyProgress = 0;
                            }

                            break;
                        case true: //Grow
                            TemporalWorkers += GetCurrentTier().WorkersToProduce;
                            break;
                    }
                }

                UpdateDistribution();
            }
        }

        private void Toggle()
        {
            UpgradeVsGrow = !UpgradeVsGrow;
            SetButtonText();
        }

        private void SetButtonText()
        {
            buttonText.text = !UpgradeVsGrow ? "Upgrade" : "Grow";
        }

        public void SetTexts()
        {
            var currentTier = entropyTiers[EntropyTierSaveData];
            nameText.text = currentTier.Name;
            tierText.text = $"Tier {ColourGreen}{EntropyTierSaveData + 1}{EndColour}";
            fillbarText.text = GetFillBarString();
            temporalWorkerText.text = $"<b>Temporal Workers</b> | {FormatNumber(TemporalWorkers)}" +
                                      $"\n<b>Buff</b> | {ColourGrey}Log10({EndColour}Temporal Workers{ColourGrey}) = {ColourGreen}{FormatNumber(TotalBuff)}{EndColour} | {ColourGreenAlt}{TotalBuff:P0}{EndColour}";
            var isTooFast = GetCurrentTier().FillTime / entropyBuff / Math.Abs(TimeScale) < 0.2;
            fillbar.fillAmount = isTooFast ? 1 : (float)(CurrentEntropyProgress / GetCurrentTier().FillTime);
        }

        private string GetFillBarString()
        {
            var startString = !UpgradeVsGrow
                ? UpgradeChance < 0
                    ? "<b>Maxed</b> "
                    : $"<b>Upgrade Chance {GetCurrentTier().UpgradeChance * entropyBuff:P7} | "
                : $"<b>Forming {ColourGreen}{FormatNumber(GetCurrentTier().WorkersToProduce)}{EndColour} Temporal Workers{EndColour}</b> | ";
            return
                $"{startString}{ColourGreen}{FormatTimeRemaining((GetCurrentTier().FillTime - CurrentEntropyProgress) / entropyBuff, true, na: false)}{EndColour}";
        }
    }

    [Serializable]
    public class EntropyTier
    {
        public string Name;
        public double UpgradeChance;
        public double FillTime;
        public double WorkersToProduce;
    }
}
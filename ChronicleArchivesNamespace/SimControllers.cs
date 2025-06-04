using System;
using Blindsided.CardSelectionSystem;
using Blindsided.SaveData;
using MPUIKIT;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using static Blindsided.SaveData.StaticReferences;
using static ChronicleArchivesNamespace.ChronicleArchiveStaticReferences;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.TextColourStrings;
using static EnginesOfExpansionNamespace.EnginesOfExpansionStaticReferences;

namespace ChronicleArchivesNamespace
{
    public class SimControllers : MonoBehaviour
    {
        #region References

        public CardSelector cardSelector;
        public TMP_Text chronicleText;

        #region IDS

        [FoldoutGroup("Ids")] public MPImage IdsProgressBar;
        [FoldoutGroup("Ids")] public TMP_Text IdsTimeRemainingText;
        [FoldoutGroup("Ids")] public TMP_Text IdsFastestCompletionText;
        [FoldoutGroup("Ids")] public TMP_Text IdsGeneratingText;
        [FoldoutGroup("Ids")] public TMP_Text IdsCompletionsText;
        private int CurrentChronotonBoost => (int)(Chronotons >= 10 ? Math.Floor(Math.Log10(Chronotons) + 1) : 1);

        #endregion

        #endregion

        private void Update()
        {
            if (LayerTab == SaveData.Tab.ChronicleArchives)
            {
                if (TimeScale != 0)
                {
                    var speed = Math.Abs(TimeScale) * Time.deltaTime;
                    IdsMinigameProduce(speed);
                    chronicleText.text =
                        $"<b>Chronicles</b> | {ColourGrey} Available{EndColour} {ColourGreen}{FormatNumber(Chronicles, true)}{EndColour}";
                }

                cardSelector.UpdateCosts();
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            UpdateIdsUI();
        }


        #region IdsMethods

        private void IdsMinigameProduce(float timescale)
        {
            if (IdsCompletionCount < 1) return;
            IdsGeneratorProgress += timescale;
            if (IdsGeneratorProgress >= IdsFastestCompletionTime)
            {
                IdsGeneratorProgress = 0;
                Chronicles += IdsCompletionCount * CurrentChronotonBoost;
                Chronotons -= Chronotons / 10;
            }
        }

// Chronotons boost when you have 10 or more, consume 10% to gain a log10 boost based on total chronotons floored.
        private void UpdateIdsUI()
        {
            IdsProgressBar.fillAmount = IdsGeneratorProgress / IdsFastestCompletionTime;
            IdsTimeRemainingText.text =
                $"<b>Time Remaining</b> | {ColourGreen}{FormatTimeRemaining(IdsFastestCompletionTime - IdsGeneratorProgress, true, na: false)}{EndColour}";
            IdsFastestCompletionText.text =
                $"<b>Fastest Completion</b>{(!UseScaledTimeForValues ? $"{ColourGreen}*{EndColour}" : "")}{(OfflineTimeActive ? $"{ColourOrange}*{EndColour}" : "")}{(DevSpeed ? $"{ColourRed}*{EndColour}" : "")} | {ColourHighlight}{FormatTimeRemaining(IdsFastestCompletionTime, IdsFastestCompletionTime / TimeScale < 60, false)}{EndColour}" +
                $"\n<b>Chronoton Boost</b> | {ColourHighlight}{CurrentChronotonBoost:P0}{EndColour}";
            IdsGeneratingText.text =
                $"<b>Generating</b> | {ColourHighlight}{FormatNumber(IdsCompletionCount * CurrentChronotonBoost, true)}{EndColour}{ColourGrey} Chronicles per cycle{EndColour}";
            IdsCompletionsText.text = $"{ColourGreen}{FormatNumber(IdsCompletionCount, true)}{EndColour}";
        }

        #endregion
    }
}
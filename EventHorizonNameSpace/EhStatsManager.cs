using System.Text;
using Blindsided.SaveData;
using TMPro;
using UnityEngine;
using static Blindsided.SaveData.StaticReferences;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.TextColourStrings;


namespace EventHorizonNameSpace
{
    public class EhStatsManager : MonoBehaviour
    {
        public TMP_Text timeSpent;

        private void Update()
        {
            var timeScale = Mathf.Abs(TimeScale);
            switch (LayerTab)
            {
                case SaveData.Tab.Zero:
                    TimeSpentInRealms.EventHorizon += Time.deltaTime;
                    ScaledTimeSpentInRealms.EventHorizon += Time.deltaTime * timeScale;
                    UpdateTimeSpent();
                    break;
                case SaveData.Tab.FoundationOfProduction:
                    if (TimeScale != 0)
                    {
                        TimeSpentInRealms.FoundationOfProduction += Time.deltaTime;
                        ScaledTimeSpentInRealms.FoundationOfProduction += Time.deltaTime * timeScale;
                    }
                    else
                    {
                        TimeSpentInRealms.VoidLull += Time.deltaTime;
                        ScaledTimeSpentInRealms.VoidLull += Time.deltaTime * timeScale;
                    }

                    break;
                case SaveData.Tab.RealmOfResearch:
                    if (TimeScale != 0)
                    {
                        TimeSpentInRealms.RealmOfResearch += Time.deltaTime;
                        ScaledTimeSpentInRealms.RealmOfResearch += Time.deltaTime * timeScale;
                    }
                    else
                    {
                        TimeSpentInRealms.VoidLull += Time.deltaTime;
                        ScaledTimeSpentInRealms.VoidLull += Time.deltaTime * timeScale;
                    }

                    break;
                case SaveData.Tab.EnginesOfExpansion:
                    if (TimeScale != 0)
                    {
                        TimeSpentInRealms.EnginesOfExpansion += Time.deltaTime;
                        ScaledTimeSpentInRealms.EnginesOfExpansion += Time.deltaTime * timeScale;
                    }
                    else
                    {
                        TimeSpentInRealms.VoidLull += Time.deltaTime;
                        ScaledTimeSpentInRealms.VoidLull += Time.deltaTime * timeScale;
                    }

                    break;
                case SaveData.Tab.CollapseOfTime:
                    if (TimeScale != 0)
                    {
                        TimeSpentInRealms.CollapseOfTime += Time.deltaTime;
                        ScaledTimeSpentInRealms.CollapseOfTime += Time.deltaTime * timeScale;
                    }
                    else
                    {
                        TimeSpentInRealms.VoidLull += Time.deltaTime;
                        ScaledTimeSpentInRealms.VoidLull += Time.deltaTime * timeScale;
                    }

                    break;
                case SaveData.Tab.ChronicleArchives:
                    if (TimeScale != 0)
                    {
                        TimeSpentInRealms.ChronicleArchives += Time.deltaTime;
                        ScaledTimeSpentInRealms.ChronicleArchives += Time.deltaTime * timeScale;
                    }
                    else
                    {
                        TimeSpentInRealms.VoidLull += Time.deltaTime;
                        ScaledTimeSpentInRealms.VoidLull += Time.deltaTime * timeScale;
                    }

                    break;
                case SaveData.Tab.TemporalRifts:
                    if (TimeScale != 0)
                    {
                        TimeSpentInRealms.TemporalRifts += Time.deltaTime;
                        ScaledTimeSpentInRealms.TemporalRifts += Time.deltaTime * timeScale;
                    }
                    else
                    {
                        TimeSpentInRealms.VoidLull += Time.deltaTime;
                        ScaledTimeSpentInRealms.VoidLull += Time.deltaTime * timeScale;
                    }

                    break;
            }
        }

        public void UpdateTimeSpent()
        {
            var sb = new StringBuilder();

            // --- Total (always show) ---
            sb.Append("<b>Total</b>\n")
                .Append($"{ColourGreen}{FormatTime(TimeSpentInRealms.Total, shortForm: false)}{EndColour}\n")
                .Append(
                    $"{ColourOrange}{FormatTime(ScaledTimeSpentInRealms.Total, shortForm: false)}{EndColour}<line-height=120%>\n")
                .Append("</line-height>");

            // --- Event Horizon (always show) ---
            sb.Append("<b>Event Horizon</b>\n")
                .Append($"{ColourGreen}{FormatTime(TimeSpentInRealms.EventHorizon, shortForm: false)}{EndColour}\n")
                .Append($"{ColourOrange}\u221e{EndColour}<line-height=120%>\n")
                .Append("</line-height>");

            // --- Only show these if real time > 0 ---
            AddRealmSectionIfAnyTime(sb,
                "Foundation of Progress",
                TimeSpentInRealms.FoundationOfProduction,
                ScaledTimeSpentInRealms.FoundationOfProduction);

            AddRealmSectionIfAnyTime(sb,
                "Realm of Research",
                TimeSpentInRealms.RealmOfResearch,
                ScaledTimeSpentInRealms.RealmOfResearch);

            AddRealmSectionIfAnyTime(sb,
                "Engines of Expansion",
                TimeSpentInRealms.EnginesOfExpansion,
                ScaledTimeSpentInRealms.EnginesOfExpansion);

            AddRealmSectionIfAnyTime(sb,
                "Collapse of Time",
                TimeSpentInRealms.CollapseOfTime,
                ScaledTimeSpentInRealms.CollapseOfTime);

            AddRealmSectionIfAnyTime(sb,
                "Chronicle Archives",
                TimeSpentInRealms.ChronicleArchives,
                ScaledTimeSpentInRealms.ChronicleArchives);

            AddRealmSectionIfAnyTime(sb,
                "Temporal Rifts",
                TimeSpentInRealms.TemporalRifts,
                ScaledTimeSpentInRealms.TemporalRifts);

            // --- Void Lull (always show) ---
            sb.Append("<b>Void Lull</b>\n")
                .Append($"{ColourGreen}{FormatTime(TimeSpentInRealms.VoidLull, shortForm: false)}{EndColour}\n")
                .Append($"{ColourOrange}\u221e{EndColour}<line-height=120%>\n")
                .Append("</line-height>");

            // Legend (no extra newline)
            sb.Append($"{ColourGreen}Realtime{EndColour}, {ColourOrange}Scaled Time{EndColour}");

            timeSpent.text = sb.ToString();
        }

        /// <summary>
        ///     Appends a realm block only if the unscaled time > 0, with no leading blank line.
        /// </summary>
        private void AddRealmSectionIfAnyTime(
            StringBuilder sb,
            string title,
            double realTimeSeconds,
            double scaledTimeSeconds)
        {
            if (realTimeSeconds > 0)
                sb.Append($"<b>{title}</b>\n")
                    .Append($"{ColourGreen}{FormatTime(realTimeSeconds, shortForm: false)}{EndColour}\n")
                    .Append(
                        $"{ColourOrange}{FormatTime(scaledTimeSeconds, shortForm: false)}{EndColour}<line-height=120%>\n")
                    .Append("</line-height>");
        }
    }
}
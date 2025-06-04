using Sirenix.OdinInspector;
using UnityEngine;

namespace RealmOfResearchNamespace.ResearchScriptables
{
    [CreateAssetMenu(fileName = "ResearcherData", menuName = "Game/Researcher Data")]
    public class ResearchData : ScriptableObject
    {
        [FoldoutGroup("Strings")] public string researcherName;
        [FoldoutGroup("Cost Settings")] public double baseCost;
        [FoldoutGroup("Cost Settings")] public double costExponent;
        [FoldoutGroup("Production Settings")] public double baseProduction;
        [FoldoutGroup("Production Settings")] public double baseCreation;
        [FoldoutGroup("Production Settings")] public bool creates = true;

        public string ProducedCurrencyString(bool plural)
        {
            return plural ? pluralProducedCurrencyName : producedCurrencyName;
        }

        public string RequiredCurrencyString(bool plural)
        {
            return plural ? pluralRequiredCurrencyName : requiredCurrencyName;
        }

        public string ProducedBuildingString(bool plural)
        {
            return plural ? pluralProducedBuildingName : producedBuildingName;
        }


        [FoldoutGroup("Strings")] [Space(10)] public string producedCurrencyName;
        [FoldoutGroup("Strings")] public string requiredCurrencyName;
        [FoldoutGroup("Strings")] public string producedBuildingName;

        [FoldoutGroup("Strings")] [Space(10)] public string pluralProducedCurrencyName;
        [FoldoutGroup("Strings")] public string pluralRequiredCurrencyName;
        [FoldoutGroup("Strings")] public string pluralProducedBuildingName;

        public int sortOrder;
    }
}
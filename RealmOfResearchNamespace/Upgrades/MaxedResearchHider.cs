using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RealmOfResearchNamespace.RealmOfResearchStaticReferences;

namespace RealmOfResearchNamespace.Upgrades
{
    public class MaxedResearchHider : MonoBehaviour
    {
        public Button hiderButton;
        public TMP_Text hiderButtonText;

        private void Start()
        {
            hiderButton.onClick.AddListener(HiderButtonClickActions);
            SetText();
        }

        private void HiderButtonClickActions()
        {
            HideMaxedResearches = !HideMaxedResearches;
            SetText();
            RealmOfResearchEvents.OnSetMaxedResearches();
        }

        private void SetText()
        {
            hiderButtonText.text = $"Maxed Upgrades | {(!HideMaxedResearches ? "Shown" : "Hidden")}";
        }
    }
}
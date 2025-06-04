using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

namespace FoundationOfProgressNameSpace.UI
{
    public class AutomatedBuildingHider : MonoBehaviour
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
            HideAutomatedBuildings = !HideAutomatedBuildings;
            SetText();
            FoundationOfProductionEvents.OnAutomatedBuildingsHideShow();
        }

        private void SetText()
        {
            hiderButtonText.text = $"Automated Buildings | {(!HideAutomatedBuildings ? "Shown" : "Hidden")}";
        }
    }
}
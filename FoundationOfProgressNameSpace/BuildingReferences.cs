using Febucci.UI;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FoundationOfProgressNameSpace
{
    public class BuildingReferences : MonoBehaviour
    {
        public Button buyButton;
        public Button infoExpander;
        public MPImageBasic infoExpanderImage;
        public GameObject expandedInfo;

        [Space(10)] public TMP_Text buildingName;
        public TextAnimator_TMP buildingNameAnimator;
        public TMP_Text owned;
        public TMP_Text production;
        public TMP_Text cost;
        public TMP_Text buyButtonText;

        [Space(10)] public TMP_Text additionalInfo;
        public TMP_Text upgradesMathBaseProduction;
        public TMP_Text upgradesMathBaseCreation;
        public TMP_Text upgradesMathMultiplier;
        public TMP_Text upgradesMathCostMulti;
        public TMP_Text lore;

        public TMP_Text extraInfoUnderCost;
        public bool extraInfoUnderCostActive;

        private void Start()
        {
            infoExpander.onClick.AddListener(ExpandInfo);
            extraInfoUnderCost.gameObject.SetActive(extraInfoUnderCostActive);
        }

        private void ExpandInfo()
        {
            expandedInfo.SetActive(!expandedInfo.activeSelf);
            infoExpanderImage.FlipVertical = !expandedInfo.activeSelf;
        }
    }
}
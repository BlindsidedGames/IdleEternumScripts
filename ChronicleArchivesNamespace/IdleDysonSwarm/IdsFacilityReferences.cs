using Febucci.UI;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChronicleArchivesNamespace.IdleDysonSwarm
{
    public class IdsFacilityReferences : MonoBehaviour
    {
        public Button buyButton;
        public Button infoExpander;
        public MPImageBasic infoExpanderImage;
        public GameObject expandedInfo;

        [Space(10)] public TMP_Text Name;

        public TextAnimator_TMP NameAnimator;

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

        private void Start()
        {
            infoExpander.onClick.AddListener(ExpandInfo);
        }

        private void ExpandInfo()
        {
            expandedInfo.SetActive(!expandedInfo.activeSelf);
            infoExpanderImage.FlipVertical = !expandedInfo.activeSelf;
        }
    }
}
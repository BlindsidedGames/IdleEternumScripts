using Febucci.UI;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChronicleArchivesNamespace
{
    public class SimReferences : MonoBehaviour
    {
        public Button playButton;
        public Button infoExpander;
        public MPImageBasic infoExpanderImage;
        public GameObject expandedInfo;

        [Space(10)] public TMP_Text simName;
        public TextAnimator_TMP simNameAnimator;
        public TMP_Text owned;
        public TMP_Text production;
        public TMP_Text generating;
        public TMP_Text buyButtonText;

        [Space(10)] public TMP_Text additionalInfo;
        public TMP_Text upgradesMathBaseProduction;
        public TMP_Text upgradesMathBaseCreation;
        public TMP_Text upgradesMathMultiplier;
        public TMP_Text upgradesMathCostMulti;
        public TMP_Text lore;

        public MPImage progressBar1;
        public TMP_Text progressBar1Text;
        public MPImage progressBar2;
        public TMP_Text progressBar2Text;

        public GameObject progressBar2Parent;
        public GameObject costParent;
        public GameObject buyButtonParent;

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
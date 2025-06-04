using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Blindsided.Utilities.CalcUtils;

namespace FoundationOfProgressNameSpace.Prestige
{
    public class ResurgenceUnlockReferences : MonoBehaviour
    {
        public TMP_Text upgradeText;
        public Button unlockButton;
        public TMP_Text unlockButtonText;
        public double cost;

        private void Start()
        {
            unlockButtonText.text = $"{FormatNumber(cost)} RE";
        }
    }
}
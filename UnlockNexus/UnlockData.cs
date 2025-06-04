using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UpgradeSystem;

namespace UnlockNexus
{
    public class UnlockData
    {
        public UnlockManager.UnlockKeys ID;
        public bool Unlocked;
        public bool IsUpgrade;
        public bool HasUnlockCard;

        public bool TabHighlight;
        [ShowIf("TabHighlight")] public GameObject TabHighlightObject;

        [FoldoutGroup("Display Settings")] [ShowIf("HasUnlockCard")]
        public string DisplayName;

        [FoldoutGroup("Display Settings")] [ShowIf("HasUnlockCard")] [TextArea(5, 10)]
        public string Description;

        [FoldoutGroup("Display Settings")] [ShowIf("HasUnlockCard")] [TextArea(5, 10)]
        public string Condition;

        private UnlockCardReferences unlockCard;

        [FoldoutGroup("Unlock References")] [ShowIf("HasUnlockCard")]
        public Transform ParentForUnlock;

        [FoldoutGroup("Unlock References")] public List<GameObject> UnlockedObjects = new();
        [FoldoutGroup("Unlock References")] public List<GameObject> LockedObjects = new();

        [FoldoutGroup("Unlock References")] [ShowIf("IsUpgrade")]
        public List<UpgradeSo> UpgradesToEnable = new();

        public bool ProcessAnyway;

        public void Setup()
        {
            foreach (var item in UnlockedObjects) item.SetActive(Unlocked);
            foreach (var item in LockedObjects) item.SetActive(!Unlocked);
            if (UpgradesToEnable != null)
                foreach (var upgrade in UpgradesToEnable)
                    upgrade.isUnlocked = Unlocked;

            if (!Unlocked)
            {
                if (ParentForUnlock == null)
                    return;
                unlockCard = Object.Instantiate(UnlockManager.Instance.unlockCardPrefab, ParentForUnlock)
                    .GetComponent<UnlockCardReferences>();
                SetupUnlockCard();
            }
            else
            {
                if (unlockCard != null)
                    Object.Destroy(unlockCard.gameObject);
            }
        }

        public void SetupUnlockCard()
        {
            if (unlockCard == null) return;
            unlockCard.unlockName.text = DisplayName;
            unlockCard.unlockDescription.text = Description;
            unlockCard.unlockCondition.text = Condition;
        }

        public void Unlock()
        {
            if (TabHighlight && !Unlocked) TabHighlightObject.SetActive(true);
            Unlocked = true;
            Setup();
        }

        public void CheckChildUnlocks()
        {
            foreach (var unlock in NextUnlockIds) UnlockManager.Instance.CheckUnlock(unlock);
        }

        public List<UnlockManager.UnlockKeys> NextUnlockIds = new();
    }
}
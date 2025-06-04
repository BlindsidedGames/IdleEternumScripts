using System.Collections.Generic;
using MPUIKIT;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using UpgradeSystem;

namespace TemporalRiftNamespace
{
    public class EssenceConverter : SerializedMonoBehaviour, IUpgradable
    {
        [TabGroup("Selections")] public EssenceType essenceType;
        [TabGroup("SaveData")] public EssenceSaveData EssenceSaveData = new();

        [TabGroup("References")] public TMP_Text nameText;
        [TabGroup("References")] public TMP_Text conversionText;
        [TabGroup("References")] public TMP_Text gainText;
        [TabGroup("References")] public TMP_Text timerText;
        [TabGroup("References")] public Button convertButton;
        [TabGroup("References")] public TMP_Text convertButtonText;
        [TabGroup("References")] public MPImage conversionProgressBar;

        #region UpgradeSystem

        [TabGroup("UpgradeData")] public List<string> tags;
        [TabGroup("UpgradeData")] public Dictionary<StatType, UpgradableStat> UpgradableStats = new();

        public List<string> GetTags()
        {
            return tags;
        }

        public UpgradableStat GetStat(StatType statType)
        {
            return UpgradableStats.GetValueOrDefault(statType);
        }

        public void Register()
        {
            UpgradeManager.Instance?.RegisterEntity(this);
        }

        public void Unregister()
        {
            UpgradeManager.Instance.UnregisterEntity(this);
        }

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        #endregion
    }

    public class EssenceSaveData
    {
        public float CurrentCostPercent = 10f;
        public float Timer = 0;
        public bool IsConverting = false;
        public double AmountGaining = 0;
    }

    public enum EssenceType
    {
        None,
        StellarParticles,
        EntropyFragments,
        Chronotons,
        TemporalWorkers,
        Chronicles
    }
}
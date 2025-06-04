using System.Collections.Generic;

namespace UpgradeSystem
{
    public interface IUpgradable
    {
        List<string> GetTags();
        UpgradableStat GetStat(StatType statType);
        void Register();
        void Unregister();
    }
}
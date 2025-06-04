using System;
using static Blindsided.Oracle;
using static ChronicleArchivesNamespace.IdleDysonSwarm.IdsStaticReferences;
using static TemporalRiftNamespace.TemporalRiftsStaticReferences;

namespace UpgradeSystem
{
    public static class CurrencyManager
    {
        private static double EntropyFragments
        {
            get => oracle.saveData.RealmOfResearchSaveDataData.Currencies.EntropyFragments;
            set => oracle.saveData.RealmOfResearchSaveDataData.Currencies.EntropyFragments = value;
        }

        public static bool CheckAffordability(double cost, CurrencyType currencyType, bool tryPurchase = false)
        {
            switch (currencyType)
            {
                case CurrencyType.EntropyFragments:
                    if (EntropyFragments < cost) return false;
                    if (tryPurchase) EntropyFragments -= cost;
                    return true;
                case CurrencyType.IdsResearch:
                    if (Science < cost) return false;
                    if (tryPurchase) Science -= cost;
                    return true;
                case CurrencyType.EternumEssence:
                    if (EternumEssence < cost) return false;
                    if (tryPurchase) EternumEssence -= cost;
                    return true;
                case CurrencyType.StabilizedMatrixFragments:
                    if (StabilizedMatrixFragments < cost) return false;
                    if (tryPurchase) StabilizedMatrixFragments -= cost;
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void RemoveCurrencyByType(double amount, CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.EntropyFragments:
                    EntropyFragments -= amount;
                    break;
                case CurrencyType.IdsResearch:
                    Science -= amount;
                    break;
                case CurrencyType.EternumEssence:
                    EternumEssence -= amount;
                    break;
                case CurrencyType.StabilizedMatrixFragments:
                    StabilizedMatrixFragments -= amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static (double, string) GetCurrencyAmount(CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.EntropyFragments:
                    return (EntropyFragments, "Entropy Fragments");
                case CurrencyType.IdsResearch:
                    return (Science, "Science");
                case CurrencyType.EternumEssence:
                    return (EternumEssence, "Eternum Essence");
                case CurrencyType.StabilizedMatrixFragments:
                    return (StabilizedMatrixFragments, "Stabilized Matrix Fragments");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum CurrencyType
    {
        EntropyFragments,
        IdsResearch,
        EternumEssence,
        StabilizedMatrixFragments
    }
}
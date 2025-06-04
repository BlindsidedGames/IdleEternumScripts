using Sirenix.OdinInspector;

namespace Blindsided.SaveData
{
    [HideReferenceObjectPicker]
    public class EnginesOfExpansionSaveData
    {
        public EoeResetOnPrestige ResetOnPrestige = new();
        public bool EnergyAmplifierAutoActivate;
        public AutoButtonCycleState AutoButtonCycleState;
        public bool TemporalAcceleratorTarget; //true = TimeCore, false = ChronotonDrill
        public bool PerfectResonanceScore;
    }

    public enum AutoButtonCycleState
    {
        ChargeDisabled,
        AutoDisabled,
        AutoEnabled
    }

    [HideReferenceObjectPicker]
    public class EoeResetOnPrestige
    {
        public double TimeCoreLevel;
        public double TimeCoreProgress;
        public double ChronotonDrillLevel;
        public double Chronotons;
        public double ChronotonDrillProgress;

        public bool EnergyAmplifierUnlocked;
        public bool EnergyAmplifierBuffActive;
        public double EnergyAmplifierBuffRemainingTime;
        public double EnergyAmplifierCharge;

        public bool TemnporalAcceleratorUnlocked;
        public double TemporalAcceleratorCharge;

        public bool ResonanceChamberUnlocked;
        public bool ResonanceChamberBuffActive;
        public double ResonanceBuffRemainingTime;
        public double ResonanceCharge;
        public double ResonanceMultiplier;
    }
}
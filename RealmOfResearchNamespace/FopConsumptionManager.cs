using System;
using UnityEngine;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.TextColourStrings;

namespace RealmOfResearchNamespace
{
    public class FopConsumptionManager : MonoBehaviour
    {
        public double percentageDrainPerSecond = 0.01;
        private double StellarParticlesConsumption => FopCurrencies.StellarParticles * percentageDrainPerSecond;
        public double NebulaDustConsumption => FopCurrencies.NebulaDust * percentageDrainPerSecond;
        public double ForgeEssenceConsumption => FopCurrencies.ForgeEssence * percentageDrainPerSecond;
        public double NovaCoresConsumption => FopCurrencies.NovaCores * percentageDrainPerSecond;
        public double QuantumSeedsConsumption => FopCurrencies.QuantumSeeds * percentageDrainPerSecond;

        public double VoidScribeMultiplier => Math.Log10(StellarParticlesConsumption + 1);
        public double FractureLoomMultiplier => Math.Log10(NebulaDustConsumption + 1);
        public double DarkArchitectMultiplier => Math.Log10(ForgeEssenceConsumption + 1);
        public double CollapseEngineMultiplier => Math.Log10(NovaCoresConsumption + 1);
        public double SungularityVaultMultiplier => Math.Log10(QuantumSeedsConsumption + 1);


        public void Drain(float deltaTime)
        {
            FopCurrencies.StellarParticles -= StellarParticlesConsumption * deltaTime;
            FopCurrencies.NebulaDust -= NebulaDustConsumption * deltaTime;
            FopCurrencies.ForgeEssence -= ForgeEssenceConsumption * deltaTime;
            FopCurrencies.NovaCores -= NovaCoresConsumption * deltaTime;
            FopCurrencies.QuantumSeeds -= QuantumSeedsConsumption * deltaTime;
        }

        public double GetResearcherMultiplier(ResearcherType researcherType)
        {
            return researcherType switch
            {
                ResearcherType.VoidScribe => VoidScribeMultiplier,
                ResearcherType.FractureLoom => FractureLoomMultiplier,
                ResearcherType.DarkArchitect => DarkArchitectMultiplier,
                ResearcherType.CollapseEngine => CollapseEngineMultiplier,
                ResearcherType.SungularityVault => SungularityVaultMultiplier,
                _ => 1.0
            };
        }

        public string GetResearcherMultiplierString(ResearcherType researcherType, bool creates)
        {
            var productionVsCreation = creates ? "Creation" : "production";
            return researcherType switch
            {
                ResearcherType.VoidScribe =>
                    $"{ColourGrey}Consumes {ColourRed}{FormatNumber(StellarParticlesConsumption)}{EndColour} {ColourOrange}Stellar Particles{EndColour} /Second\nBoosting {productionVsCreation} by {ColourGreen}{VoidScribeMultiplier:P0}{EndColour}",
                ResearcherType.FractureLoom =>
                    $"{ColourGrey}Consumes {ColourRed}{FormatNumber(NebulaDustConsumption)}{EndColour} {ColourOrange}Nebula Dust{EndColour} /Second\nBoosting {productionVsCreation} by {ColourGreen}{FractureLoomMultiplier:P0}{EndColour}",
                ResearcherType.DarkArchitect =>
                    $"{ColourGrey}Consumes {ColourRed}{FormatNumber(ForgeEssenceConsumption)}{EndColour} {ColourOrange}Forge Essence{EndColour} /Second\nBoosting {productionVsCreation} by {ColourGreen}{DarkArchitectMultiplier:P0}{EndColour}",
                ResearcherType.CollapseEngine =>
                    $"{ColourGrey}Consumes {ColourRed}{FormatNumber(NovaCoresConsumption)}{EndColour} {ColourOrange}Nova Cores{EndColour} /Second\nBoosting {productionVsCreation} by {ColourGreen}{CollapseEngineMultiplier:P0}{EndColour}",
                ResearcherType.SungularityVault =>
                    $"{ColourGrey}Consumes {ColourRed}{FormatNumber(QuantumSeedsConsumption)}{EndColour} {ColourOrange}Quantum Seeds{EndColour} /Second\nBoosting {productionVsCreation} by {ColourGreen}{SungularityVaultMultiplier:P0}{EndColour}",

                _ => "1.00"
            };
        }
    }

    public enum ResearcherType
    {
        VoidScribe,
        FractureLoom,
        DarkArchitect,
        CollapseEngine,
        SungularityVault
    }
}
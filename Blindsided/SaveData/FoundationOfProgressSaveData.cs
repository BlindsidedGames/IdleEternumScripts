using Sirenix.OdinInspector;

namespace Blindsided.SaveData
{
    [HideReferenceObjectPicker]
    public class FoundationOfProgressSaveData
    {
        public FopAscendancy Ascendancy = new();
        public FopStats Stats = new();
        public bool Prestiged;
        public long TimesPrestiged;
        public bool Ascended;
        public long TimesAscended;
        public long TotalResets => TimesPrestiged + TimesAscended;
        public bool BuildingFoldoutPreference = true;
        public bool HideAutomatedBuildings;
        public float AutoPrestigeSavedValue = 400;
        public bool AutoPrestigeEnabled;
        public bool AutoBuyEnabled = true;
    }

    [HideReferenceObjectPicker]
    public class FopPrestige
    {
        public ResurgenceResets ResurgenceResets = new();
        public double ResurgenceEnergy;
        public double ProductionMultiplier;
    }

    [HideReferenceObjectPicker]
    public class ResurgenceResets
    {
        public FopCurrencies Currencies = new();
        public FopBuildings FopBuildings = new();
    }

    [HideReferenceObjectPicker]
    public class FopAscendancy
    {
        public FopPrestige Prestige = new();

        public double ContinuumShards;
        public double ProductionExponent;
        public double RealmOfResearchMultiplier;

        public bool AutobuyNebulaGenerator;
        public bool AutobuyCelestialFoundry;
        public bool AutobuyNovaSpire;
        public bool AutobuyGalacticNexus;
        public bool AutobuySingularityLoom;
        public bool AutobuyEternityBeacon;
        public bool AutobuyInfinityCrucible;

        public bool AdditionalBuildings;

        public bool AllOwned => AutobuyNebulaGenerator && AutobuyCelestialFoundry && AutobuyNovaSpire &&
                                AutobuyGalacticNexus && AutobuySingularityLoom && AutobuyEternityBeacon &&
                                AutobuyInfinityCrucible && AdditionalBuildings;
    }

    [HideReferenceObjectPicker]
    public class FopCurrencies
    {
        public double StellarParticles;
        public double NebulaDust;
        public double ForgeEssence;
        public double NovaCores;
        public double QuantumSeeds;
        public double TranscendentFilaments;
        public double EterniumSparks;
        public double TemporalShards;
    }

    [HideReferenceObjectPicker]
    public class FopBuildings
    {
        public FopBuilding StarCradle = new() { NumberCreated = 1 };
        public FopBuilding NebulaGenerator = new();
        public FopBuilding CelestialFoundry = new();
        public FopBuilding NovaSpire = new();
        public FopBuilding GalacticNexus = new();
        public FopBuilding SingularityLoom = new();
        public FopBuilding EternityBeacon = new();
        public FopBuilding InfinityCrucible = new();
    }

    [HideReferenceObjectPicker]
    public class FopStats
    {
        public FopBuildingStats StarCradleStats = new();
        public FopBuildingStats NebulaGeneratorStats = new();
        public FopBuildingStats CelestialFoundryStats = new();
        public FopBuildingStats NovaSpireStats = new();
        public FopBuildingStats GalacticNexusStats = new();
        public FopBuildingStats SingularityLoomStats = new();
        public FopBuildingStats EternityBeaconStats = new();
        public FopBuildingStats InfinityCrucibleStats = new();
    }


    [HideReferenceObjectPicker]
    public class FopBuilding
    {
        public double NumberOwned => NumberCreated + NumberPurchased;
        public double NumberCreated;
        public double NumberPurchased = 0;

        public double Produced;
        public double Created;
    }

    [HideReferenceObjectPicker]
    public class FopBuildingStats
    {
        public double TotalPurchased;
        public double TotalProduction;
        public double TotalCreated;
    }

    public enum BuildingType
    {
        StarCradle,
        NebulaGenerator,
        CelestialFoundry,
        NovaSpire,
        GalacticNexus,
        SingularityLoom,
        EternityBeacon,
        InfinityCrucible
    }
}
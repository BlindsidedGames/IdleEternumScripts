using System.Collections.Generic;
using Sirenix.OdinInspector;
using UpgradeSystem;

// Included as most derived classes use it
// Included as most derived classes use it
// Common utility
// Common utility
// Common utility for accessing saved values
// Common utility

// Common for this namespace


namespace EnginesOfExpansionNamespace.Engines
{
    /// <summary>
    ///     Abstract base class for game facilities within the Engines of Expansion system.
    ///     Handles common IUpgradable implementation and lifecycle methods.
    ///     Derived classes must implement specific logic for setup, production/action, and UI updates.
    /// </summary>
    public abstract class UpgradableFacility : SerializedMonoBehaviour, IUpgradable
    {
        // --- Shared IUpgradable Implementation ---

        // Derived classes should initialize this list, potentially via [SerializeField]
        // or directly in their specific SetUpVariables implementation.
        protected abstract List<string> FacilityTags { get; }

        public Dictionary<StatType, UpgradableStat> UpgradableStats = new();

        // Abstract method to be implemented by derived classes
        // to populate the UpgradableStats dictionary with their specific stats and base values.
        protected abstract void SetUpSpecificVariables();

        public List<string> GetTags()
        {
            return FacilityTags;
        }

        public UpgradableStat GetStat(StatType statType)
        {
            // Returns the stat if found, or null/default if not.
            // Consider adding error handling or returning a default UpgradableStat if necessary.
            return UpgradableStats.GetValueOrDefault(statType);
        }

        public void Register()
        {
            // Assumes UpgradeManager is a Singleton accessible via Instance
            UpgradeManager.Instance?.RegisterEntity(this);
        }

        public void Unregister()
        {
            // Assumes UpgradeManager is a Singleton accessible via Instance
            UpgradeManager.Instance?.UnregisterEntity(this);
        }

        // --- Unity Lifecycle Methods ---

        protected virtual void OnEnable()
        {
            // Ensure stats are set up before registering
            SetUpSpecificVariables();
            Register();
        }

        protected virtual void OnDisable()
        {
            Unregister();
        }

        // Optional: Derived classes might override Start for specific initializations (like button listeners)
        // protected virtual void Start() { }

        // --- Abstract Methods for Derived Class Implementation ---

        /// <summary>
        ///     Implement the core production, charging, or action logic per game tick (deltaTime).
        /// </summary>
        /// <param name="deltaTime">Time elapsed since the last frame/tick.</param>
        public abstract void Produce(float deltaTime);

        /// <summary>
        ///     Implement the logic to update all UI elements associated with this facility.
        ///     Typically called after state changes (e.g., purchases, production cycles, buffs).
        /// </summary>
        public abstract void UpdateUI();

        // --- Common Helper Properties (Optional - Add if truly universal) ---

        // Example: Affordability check might be common, but resources and costs differ.
        // Could be made abstract or virtual if a common pattern exists.
        // public abstract bool Affordable { get; }
        // public string AffordableString => Affordable ? ColourHighlight : ColourRed;
    }
}
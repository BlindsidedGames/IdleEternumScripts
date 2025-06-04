using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using static Blindsided.SaveData.TextColourStrings;
using static Blindsided.Utilities.CalcUtils;

namespace UpgradeSystem
{
    public class UpgradableStat
    {
        public double baseValue;
        [FoldoutGroup("Applied Upgrades")] public List<Modifier> additiveModifiers = new();
        [FoldoutGroup("Applied Upgrades")] public List<Modifier> multiplicativeModifiers = new();

        [FoldoutGroup("Cached Value")] [SerializeField]
        private double _cachedValue;

        [FoldoutGroup("Cached Value")] [SerializeField]
        public bool Floored;

        [FoldoutGroup("Cached Value")] [SerializeField]
        private string _cachedBreakdown;

        [SerializeField] private bool _isDirty = true;
        private bool _isDirtyBreakdown = true;

        // Add a flag to flip multiplicative color logic if needed.
        public bool flipMultiplicativeColor = false;

        public double CachedValue
        {
            get
            {
                if (_isDirty)
                {
                    _cachedValue = GetValue();
                    _isDirty = false;
                }

                return Floored ? Math.Floor(_cachedValue) : _cachedValue;
            }
        }

        public string CachedBreakdown
        {
            get
            {
                if (_isDirtyBreakdown)
                {
                    _cachedBreakdown = GetBreakdown();
                    _isDirtyBreakdown = false;
                }

                return _cachedBreakdown;
            }
        }

        public double GetValue()
        {
            var additiveSum = additiveModifiers?.Sum(m => m.value) ?? 0;
            var multiplicativeProduct = multiplicativeModifiers?.Aggregate(1.0, (acc, m) => acc * m.value) ?? 1.0;
            return (baseValue + additiveSum) * multiplicativeProduct;
        }

        public void AddAdditiveModifier(Modifier modifier)
        {
            if (additiveModifiers == null)
                additiveModifiers = new List<Modifier>();
            additiveModifiers.Add(modifier);
        }

        public void AddMultiplicativeModifier(Modifier modifier)
        {
            if (multiplicativeModifiers == null)
                multiplicativeModifiers = new List<Modifier>();
            multiplicativeModifiers.Add(modifier);
        }

        public void RemoveModifier(Modifier modifier)
        {
            if (additiveModifiers?.Remove(modifier) == true) return;
            if (multiplicativeModifiers != null)
                multiplicativeModifiers.Remove(modifier);
        }

        // Helper to decide color based on modifier value.
        private string GetModifierColor(double value, bool isMultiplicative)
        {
            if (isMultiplicative)
            {
                // Normal: > 1 positive, < 1 negative.
                // Flip if needed.
                if (!flipMultiplicativeColor)
                    return value > 1 ? ColourGreen : value < 1 ? ColourRed : ColourWhite;
                return value > 1 ? ColourRed : value < 1 ? ColourGreen : ColourWhite;
            }

            // For additive: positive is green, negative is red.
            return value > 0 ? ColourGreen : value < 0 ? ColourRed : ColourWhite;
        }

        public string GetBreakdown()
        {
            var breakdown = $"{ColourWhite}<b>Base</b>{EndColour}: {baseValue}";

            if (additiveModifiers != null && additiveModifiers.Any())
            {
                breakdown += $"\n{ColourWhite}<b>Additive bonuses</b>{EndColour}\n";
                foreach (var mod in additiveModifiers)
                {
                    var modColor = GetModifierColor(mod.value, false);
                    // Prepend a '+' if the value is positive.
                    var sign = mod.value > 0 ? "+" : "";
                    breakdown +=
                        $"{sign} {modColor}<b>{FormatNumber(mod.value)}</b>{EndColour} from <b>{mod.sourceUpgrade.upgradeName}</b>\n";
                }
            }

            if (multiplicativeModifiers != null && multiplicativeModifiers.Any())
            {
                breakdown += $"\n{ColourWhite}<b>Multiplicative bonuses</b>{EndColour}\n";
                foreach (var mod in multiplicativeModifiers)
                {
                    var modColor = GetModifierColor(mod.value, true);
                    breakdown +=
                        $"* {modColor}<b>{FormatNumber(mod.value)}</b>{EndColour} from <b>{mod.sourceUpgrade.upgradeName}</b>\n";
                }
            }

            breakdown +=
                $"\n{ColourWhite}<b>Total</b>{EndColour}\n{ColourGreen}{FormatNumber(CachedValue)}{EndColour}\nAKA {ColourGreenAlt}{CachedValue:N2}{EndColour}";
            return breakdown;
        }

        public void CacheStat()
        {
            _isDirty = true;
            _isDirtyBreakdown = true;
        }
    }

    public class Modifier
    {
        public double value;
        public int level;
        public UpgradeSo sourceUpgrade;
    }
}
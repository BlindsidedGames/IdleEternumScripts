using System;
using ChronicleArchivesNamespace;
using Sirenix.OdinInspector;

namespace Blindsided.CardSelectionSystem
{
    [Serializable]
    public class Card
    {
        public CardReferences cardReferences; // References to card assets
        [MinValue(1)] public int Rarity; // Rarity must be at least 1
        [MinValue(0)] public float Weight; // Weight cannot be negative
    }
}
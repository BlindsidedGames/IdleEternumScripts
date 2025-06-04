using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace Blindsided.CardSelectionSystem
{
    [Serializable]
    public class CardCollection
    {
        [Required] public string collectionName; // Name of the collection (e.g., "Starter Deck")
        public Button purchaseButton;
        public TMP_Text purchaseCostText;
        public double purchaseCost;

        [TableList] public Card[] Cards; // Array of cards in this collection

        public CardCollection(Card[] cards)
        {
            Cards = cards;
        }
    }
}
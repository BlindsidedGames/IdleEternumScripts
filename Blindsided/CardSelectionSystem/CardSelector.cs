using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using static Blindsided.Utilities.CalcUtils;
using static Blindsided.SaveData.SaveData;
using static Blindsided.SaveData.StaticReferences;
using static ChronicleArchivesNamespace.ChronicleArchiveStaticReferences;
using static Blindsided.SaveData.TextColourStrings;

namespace Blindsided.CardSelectionSystem
{
    public class CardSelector : MonoBehaviour
    {
        public List<CardCollection> cardCollections = new(); // List of all card collections

        // Dictionary to store precomputed data for each collection
        private readonly Dictionary<string, (Card[], float[])> collectionData = new();

        private void OnEnable()
        {
            foreach (var collection in cardCollections)
                collection.purchaseButton.onClick.AddListener(() => PurchaseCards(collection));
        }

        private void OnDisable()
        {
            foreach (var collection in cardCollections) collection.purchaseButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            InitializeCollections();
            UpdateCosts();
        }


        private void PurchaseCards(CardCollection collection)
        {
            StartCoroutine(nameof(PurchaseMassCards), collection);
        }

        private const int BruteForceThreshold = 100_000; // tune to taste

        private IEnumerator PurchaseMassCards(CardCollection collection)
        {
            var numToPurchase = PurchaseAmount(collection.purchaseCost);
            Chronicles -= (long)Cost(collection.purchaseCost);
            UpdateCosts();

            if (numToPurchase > BruteForceThreshold)
            {
                GrantByWeight(collection, numToPurchase);
                yield break; // one-frame
            }

            GrantByLoop(collection, numToPurchase); // ≤100k → still one-frame
            yield break;
        }

/*───────────────────────────────────────────────────────────────────────────
 * 1)  FAST BULK PATH  – gives each card its weighted share in one pass
 *───────────────────────────────────────────────────────────────────────────*/
        private void GrantByWeight(CardCollection collection, int amount)
        {
            // Grab the card list & cumulative-weights array that your Select-method uses
            if (!collectionData.TryGetValue(collection.collectionName, out var entry))
                return;

            var (cards, cumWeights) = entry;
            var cardCount = cards.Length;
            double totalW = cumWeights[cardCount]; // cumWeights[^1] in C# 8+

            /* ----- Compute integer quantities (floor) --------------------------- */
            var grants = new int[cardCount];
            var granted = 0;

            for (var i = 0; i < cardCount; i++)
            {
                double w = cumWeights[i + 1] - cumWeights[i];
                var copies = (int)(amount * w / totalW); // floor
                grants[i] = copies;
                granted += copies;
            }

            /* ----- Hand out the rounding remainder randomly --------------------- */
            var remainder = amount - granted;
            for (var i = 0; i < remainder; i++)
            {
                var idx = Random.Range(0, cardCount);
                grants[idx]++;
            }

            /* ----- Award the piles ---------------------------------------------- */
            for (var i = 0; i < cardCount; i++)
                if (grants[i] > 0)
                    cards[i].cardReferences.EarnCards(grants[i]);
        }

/*───────────────────────────────────────────────────────────────────────────
 * 2)  BRUTE-FORCE PATH  – the dictionary+loop we wrote previously
 *───────────────────────────────────────────────────────────────────────────*/
        private void GrantByLoop(CardCollection collection, int numToPurchase)
        {
            var piles = new Dictionary<Card, int>(Mathf.Min(numToPurchase, 256));

            for (var i = 0; i < numToPurchase; i++)
            {
                var card = SelectCardFromCollection(collection.collectionName);
                if (card == null) continue;

                if (piles.TryGetValue(card, out var cnt))
                    piles[card] = cnt + 1;
                else
                    piles.Add(card, 1);
            }

            foreach (var kvp in piles)
            {
                var amount = kvp.Value;

                // Optional micro-rounding to shrink very large stacks
                if (amount >= 1_000_000) amount = amount / 100 * 100;
                else if (amount >= 100_000) amount = amount / 25 * 25;
                else if (amount >= 10_000) amount = amount / 5 * 5;

                kvp.Key.cardReferences.EarnCards(amount);
            }
        }


        public void UpdateCosts()
        {
            foreach (var collection in cardCollections)
            {
                var cost = Cost(collection.purchaseCost);
                collection.purchaseCostText.text =
                    $"<b>Purchase</b> | {ColourGreen}{FormatNumber(PurchaseAmount(collection.purchaseCost), true)}{EndColour} Costing {ColourHighlight}{FormatNumber(cost)}{EndColour} Chronicles";
                collection.purchaseButton.interactable = Chronicles >= cost;
            }
        }

        public double Cost(double baseCost)
        {
            return PurchaseAmount(baseCost) * baseCost;
        }

        public int PurchaseAmount(double collectionCost)
        {
            return PurchaseMode switch
            {
                BuyMode.Buy1 => 1,
                BuyMode.Buy10 => 10,
                BuyMode.Buy50 => 50,
                BuyMode.Buy100 => 100,
                BuyMode.BuyMax => (int)Math.Max(1, Chronicles / collectionCost),
                _ => 1
            };
        }

        [Button("Initialize Collections")]
        private void InitializeCollections()
        {
            collectionData.Clear();
            foreach (var collection in cardCollections)
            {
                if (collection.Cards == null || collection.Cards.Length == 0)
                {
                    Debug.LogWarning($"Collection '{collection.collectionName}' has no cards.");
                    continue;
                }

                // Sort cards by rarity
                var sortedCards = collection.Cards.OrderBy(c => c.Rarity).ToArray();
                // Precompute cumulative weights
                var cumWeights = new float[sortedCards.Length + 1];
                cumWeights[0] = 0;
                for (var i = 0; i < sortedCards.Length; i++)
                {
                    if (sortedCards[i].Weight < 0) sortedCards[i].Weight = 0; // Ensure non-negative weights
                    cumWeights[i + 1] = cumWeights[i] + sortedCards[i].Weight;
                }

                collectionData[collection.collectionName] = (sortedCards, cumWeights);
            }
        }

        // Binary search to find the lower bound of the rarity range
        private int LowerBound(Card[] cards, int targetRarity)
        {
            var left = 0;
            var right = cards.Length;
            while (left < right)
            {
                var mid = left + (right - left) / 2;
                if (cards[mid].Rarity < targetRarity)
                    left = mid + 1;
                else
                    right = mid;
            }

            return left;
        }

        // Binary search to find the upper bound of the rarity range
        private int UpperBound(Card[] cards, int targetRarity)
        {
            var left = 0;
            var right = cards.Length;
            while (left < right)
            {
                var mid = left + (right - left) / 2;
                if (cards[mid].Rarity <= targetRarity)
                    left = mid + 1;
                else
                    right = mid;
            }

            return left;
        }

        [Button]
        public void TestDrawCard()
        {
            var card = SelectCardFromCollection("Set One", 1, 3);
            if (card != null)
            {
                Debug.Log($"Selected card: {card.cardReferences.cardName} (Rarity: {card.Rarity})");
                card.cardReferences.EarnCards();
            }
        }

        // Select a card from the specified collection within the rarity range
        public Card SelectCardFromCollection(string collectionName, int minRarity = 0, int maxRarity = 99)
        {
            if (!collectionData.ContainsKey(collectionName))
            {
                Debug.LogWarning($"Collection '{collectionName}' not found.");
                return null;
            }

            var (cards, cumWeights) = collectionData[collectionName];
            var start = LowerBound(cards, minRarity);
            var end = UpperBound(cards, maxRarity) - 1;

            if (start > end || start >= cards.Length)
            {
                Debug.LogWarning($"No cards found in '{collectionName}' within rarity {minRarity}-{maxRarity}.");
                return null;
            }

            var totalWeight = cumWeights[end + 1] - cumWeights[start];
            if (totalWeight <= 0)
            {
                Debug.LogWarning(
                    $"Total weight is zero or negative in '{collectionName}' for rarity {minRarity}-{maxRarity}.");
                return null;
            }

            var r = Random.Range(0f, totalWeight);
            var target = cumWeights[start] + r;

            var left = start;
            var right = end + 1;
            while (left < right)
            {
                var mid = left + (right - left) / 2;
                if (cumWeights[mid] <= target)
                    left = mid + 1;
                else
                    right = mid;
            }

            var selectedIndex = left - 1;
            return cards[selectedIndex];
        }
    }
}
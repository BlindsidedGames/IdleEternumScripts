using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.EssentialKit;
using static Blindsided.SaveData.SaveData;
using static Blindsided.SaveData.StaticReferences;

public class LayerSwitcher : MonoBehaviour
{
    [SerializeField] protected Button zero;
    public GameObject[] zeroObjects;

    #region Negative

    [SerializeField] protected Button minusOne;
    public GameObject[] minusOneObjects;

    [SerializeField] protected Button minusFive;
    public GameObject[] minusFiveObjects;

    [SerializeField] protected Button minusTwenty;
    public GameObject[] minusTwentyObjects;

    #endregion

    #region Positive

    [SerializeField] protected Button one;
    public GameObject[] oneObjects;

    [SerializeField] protected Button five;
    public GameObject[] fiveObjects;

    [SerializeField] protected Button twenty;
    public GameObject[] twentyObjects;

    #endregion

    public GameObject[] toDisable;


    [Button("0 Event Horizon")]
    public void SetLayerZero()
    {
        SetTab(Tab.Zero, false);
    }

    [Button("1 Foundation of Production")]
    public void SetLayerOne()
    {
        SetTab(Tab.FoundationOfProduction, false);
    }

    [Button("2 Engines of Expansion")]
    public void SetLayerFive()
    {
        SetTab(Tab.EnginesOfExpansion, false);
    }

    [Button("3 Chronicle Archive")]
    public void SetLayerTwenty()
    {
        SetTab(Tab.ChronicleArchives, false);
    }

    [Button("-1 Realm of Research")]
    public void SetLayerMinusOne()
    {
        SetTab(Tab.RealmOfResearch, false);
    }

    [Button("-2 The Collapse of TIme")]
    public void SetLayerMinusFive()
    {
        SetTab(Tab.CollapseOfTime, false);
    }

    [Button("-3 Temporal Rifts")]
    public void SetLayerMinusTwenty()
    {
        SetTab(Tab.TemporalRifts, false);
    }

    private void Start()
    {
        zero.onClick.AddListener(() => SetTab(Tab.Zero));
        //positive
        one.onClick.AddListener(() => SetTab(Tab.FoundationOfProduction));
        five.onClick.AddListener(() => SetTab(Tab.EnginesOfExpansion));
        twenty.onClick.AddListener(() => SetTab(Tab.ChronicleArchives));
        //negative
        minusOne.onClick.AddListener(() => SetTab(Tab.RealmOfResearch));
        minusFive.onClick.AddListener(() => SetTab(Tab.CollapseOfTime));
        minusTwenty.onClick.AddListener(() => SetTab(Tab.TemporalRifts));
        SetSavedTab(LayerTab);
    }

    private void EnableAllButtons()
    {
        zero.interactable = true;
        //positive
        one.interactable = true;
        five.interactable = true;
        twenty.interactable = true;
        //negative
        minusOne.interactable = true;
        minusFive.interactable = true;
        minusTwenty.interactable = true;
    }

    public void DisableAll()
    {
        foreach (var item in toDisable) item.SetActive(false);
    }

    public void SetTab(Tab t, bool save = true)
    {
        DisableAll();
        EnableAllButtons();
        switch (t)
        {
            case Tab.Zero:
                foreach (var item in zeroObjects) item.SetActive(true);
                zero.interactable = false;
                break;
            //positive
            case Tab.FoundationOfProduction:
                foreach (var item in oneObjects) item.SetActive(true);
                one.interactable = false;
                break;
            case Tab.EnginesOfExpansion:
                foreach (var item in fiveObjects) item.SetActive(true);
                five.interactable = false;
                break;
            case Tab.ChronicleArchives:
                foreach (var item in twentyObjects) item.SetActive(true);
                twenty.interactable = false;
                break;
            //negative
            case Tab.RealmOfResearch:
                foreach (var item in minusOneObjects) item.SetActive(true);
                minusOne.interactable = false;
                break;
            case Tab.CollapseOfTime:
                foreach (var item in minusFiveObjects) item.SetActive(true);
                if (RateMyApp.IsAllowedToRate()) RateMyApp.AskForReviewNow();

                minusFive.interactable = false;
                break;
            case Tab.TemporalRifts:
                foreach (var item in minusTwentyObjects) item.SetActive(true);
                minusTwenty.interactable = false;
                break;
            default:
                Debug.Log("NoTab");
                break;
        }

        if (save)
            switch (t)
            {
                case Tab.Zero:
                    LayerTab = Tab.Zero;
                    break;
                //positive
                case Tab.FoundationOfProduction:
                    LayerTab = Tab.FoundationOfProduction;
                    break;
                case Tab.EnginesOfExpansion:
                    LayerTab = Tab.EnginesOfExpansion;
                    break;
                case Tab.ChronicleArchives:
                    LayerTab = Tab.ChronicleArchives;
                    break;
                //negative
                case Tab.RealmOfResearch:
                    LayerTab = Tab.RealmOfResearch;
                    break;
                case Tab.CollapseOfTime:
                    LayerTab = Tab.CollapseOfTime;
                    break;
                case Tab.TemporalRifts:
                    LayerTab = Tab.TemporalRifts;
                    break;
                default:
                    Debug.Log("NoTab");
                    break;
            }

        OverlordEvents.OnUpdateTimescale();
    }

    private void SetSavedTab(Tab t)
    {
        switch (t)
        {
            case Tab.Zero:
                zero.onClick.Invoke();
                break;
            //positive
            case Tab.FoundationOfProduction:
                one.onClick.Invoke();
                break;
            case Tab.EnginesOfExpansion:
                five.onClick.Invoke();
                break;
            case Tab.ChronicleArchives:
                twenty.onClick.Invoke();
                break;
            //negative
            case Tab.RealmOfResearch:
                minusOne.onClick.Invoke();
                break;
            case Tab.CollapseOfTime:
                minusFive.onClick.Invoke();
                break;
            case Tab.TemporalRifts:
                minusTwenty.onClick.Invoke();
                break;
            default:
                Debug.Log("NoTab");
                break;
        }
    }
}
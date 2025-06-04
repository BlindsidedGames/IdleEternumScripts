using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Blindsided.SaveData.StaticReferences;

public class ValueDisplayTimescaleSetting : MonoBehaviour
{
    public Button timescaleButton;
    public TMP_Text timescaleText;

    private void Start()
    {
        timescaleButton.onClick.AddListener(ChangeTimescale);
        SetText();
    }

    private void ChangeTimescale()
    {
        UseScaledTimeForValues = !UseScaledTimeForValues;
        SetText();
    }

    private void SetText()
    {
        timescaleText.text = !UseScaledTimeForValues ? "Realtime" : "Timescale";
    }
}
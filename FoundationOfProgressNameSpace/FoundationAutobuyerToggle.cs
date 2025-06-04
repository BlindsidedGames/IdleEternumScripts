using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static FoundationOfProgressNameSpace.FoundationOfProductionStaticReferences;

public class FoundationAutobuyerToggle : MonoBehaviour
{
    public Button autobuyerButton;
    public TMP_Text autobuyerButtonText;

    private void Start()
    {
        autobuyerButton.onClick.AddListener(SetButton);
        LoadState();
    }

    private void LoadState()
    {
        switch (AutoBuyEnabled)
        {
            case true:
                autobuyerButtonText.text = "Active";
                break;
            case false:
                autobuyerButtonText.text = "Inactive";
                break;
        }
    }

    private void SetButton()
    {
        AutoBuyEnabled = !AutoBuyEnabled;
        LoadState();
    }
}
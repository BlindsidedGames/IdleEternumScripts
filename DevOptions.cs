using Sirenix.OdinInspector;
using TimeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Blindsided.SaveData.StaticReferences;

public class DevOptions : MonoBehaviour
{
    public Button devSpeedButton;
    public TMP_Text devSpeedText;

    public Button setMaxForwardsButton;

    public Button setMaxBackwardsButton;
    public GoalController goalController;

    private void Start()
    {
        devSpeedButton.onClick.AddListener(ChangeDevSpeed);
        SetDevSpeedText();
        setMaxForwardsButton.onClick.AddListener(SetMaxForwards);
        setMaxBackwardsButton.onClick.AddListener(SetMaxBackwards);
    }

    [Button]
    private void SetMaxBackwards()
    {
        CurrentTime = TimeManager.timeManager.MaxBackwardTime;
    }

    [Button]
    private void SetMaxForwards()
    {
        CurrentTime = TimeManager.timeManager.MaxForwardTime;
    }

    [Button]
    private void ChangeDevSpeed()
    {
        DevSpeed = !DevSpeed;
        SetDevSpeedText();
        goalController.ShowCurrentGoal();
    }

    private void SetDevSpeedText()
    {
        devSpeedText.text = DevSpeed ? "Active" : "Inactive";
    }
}
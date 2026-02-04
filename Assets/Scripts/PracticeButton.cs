using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PracticeButton : MonoBehaviour
{
    [SerializeField] StatType statType;
    Button practiceButton;
    [SerializeField] TMP_Text energyCostText;

    void Start()
    {
        practiceButton = GetComponent<Button>();

        Debug.Assert(practiceButton != null, "PracticeButton: practiceButton is not assigned.");
        Debug.Assert(energyCostText != null, "PracticeButton: energyCostText is not assigned.");

        energyCostText.text = MainSceneController.Instance.GetPracticeCost(statType).ToString();

        practiceButton.onClick.AddListener(Practice);
        GameManager.Instance.SubscribeOnChanged(StatType.Energy, judgeButtonInteractable);
    }

    void Practice()
    {
        int cost = MainSceneController.Instance.GetPracticeCost(statType);
        int upValue = MainSceneController.Instance.GetPracticeCostUpValue(statType);

        GameManager.Instance.AddStat(StatType.Energy, -cost);
        GameManager.Instance.AddStat(statType, upValue);
    }

    void judgeButtonInteractable(int energy)
    {
        if (energy < MainSceneController.Instance.GetPracticeCost(statType))
        {
            practiceButton.interactable = false;
        }
        else
        {
            practiceButton.interactable = true;
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.UnsubscribeOnChanged(StatType.Energy, judgeButtonInteractable);
    }
}
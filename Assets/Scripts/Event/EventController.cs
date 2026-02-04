using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
    [SerializeField] GameObject eventWindow;
    [SerializeField] GameObject playerCharacter;
    [SerializeField] Button setActiveEventButton;

    [Header("순서 맞춰주세요")]
    [SerializeField] Button[] eventButtons;
    [SerializeField] TMP_Text[] eventNameTexts;
    [SerializeField] TMP_Text[] eventCostTexts;

    DialogueManager dialogueManager;

    void Start()
    {
        GameManager.Instance.onDayChanged += ActiveEventButton;
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        setActiveEventButton.onClick.AddListener(OpenEvents);
        List<Event> e = EventManager.Instance.GetEvents();
        for (int i = 0; i < 3; i++)
        {
            if(e.Count >= 3) SetEventUI(i, e[i]);
            EventManager.Instance.OnEventSets[i] += SetEventUI;
            GameManager.Instance.SubscribeOnChanged(StatType.Energy, judge);
        }
        eventWindow.SetActive(false);
    }

    void OnDestroy()
    {
        GameManager.Instance.onDayChanged -= ActiveEventButton;
        for (int i = 0; i < 3; i++)
        {
            GameManager.Instance.UnsubscribeOnChanged(StatType.Energy, judge);
            EventManager.Instance.OnEventSets[i] -= SetEventUI;
        }
    }

    void SetEventUI(int i, Event e)
    {
        eventButtons[i].onClick.RemoveAllListeners();
        eventButtons[i].onClick.AddListener(() =>
        {
            playerCharacter.SetActive(true);
            EventManager.Instance.SetRandom3Events();
            GameManager.Instance.AddStat(StatType.Energy, -e.energyCost);
            EventManager.Instance.eventCount++;
            if (EventManager.Instance.eventCount >= EventManager.Instance.maxEventCount)
                setActiveEventButton.interactable = false;
            dialogueManager.StartDialogue(e.eventName);
            eventWindow.SetActive(false);
        });

        eventNameTexts[i].text = e.eventName.Replace("_", " ");
        eventCostTexts[i].text = e.energyCost.ToString();
    }

    void judge(int v)
    {
        List<Event> e = EventManager.Instance.GetEvents();
        for (int i = 0; i<3; i++)
        {
            if (e[i].energyCost > GameManager.Instance.GetStat(StatType.Energy))
            {
                eventButtons[i].interactable = false;
            }
            else
            {
                eventButtons[i].interactable = true;
            }
        }
    }

    void OpenEvents()
    {
        playerCharacter.SetActive(false);
        eventWindow.SetActive(true);
    }

    void ActiveEventButton()
    {
        setActiveEventButton.interactable = true;
    }
    
    public void CloseEvents()
    {
        playerCharacter.SetActive(true);
        eventWindow.SetActive(false);
    }
}

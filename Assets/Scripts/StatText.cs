using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatText : MonoBehaviour
{
    [SerializeField] StatType statType;
    [SerializeField] TMP_Text statText;

    GameManager gameManager;

    void Start()
    {
        StartCoroutine(Init());        
    }

    IEnumerator Init()
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        SetText(GameManager.Instance.GetStat(statType));
        gameManager = GameManager.Instance;
        gameManager.SubscribeOnChanged(statType, SetText);
    }

    void OnDestroy()
    {
        gameManager.UnsubscribeOnChanged(statType, SetText);
    }
    
    void SetText(int value)
    {
        statText.text = value.ToString();
        if (statType == StatType.Money)
            statText.text += "원";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StatBar : MonoBehaviour
{
    [SerializeField] StatType statType;
    Slider statSlider;
    [SerializeField] TMP_Text statText;
    [SerializeField] TMP_Text valueText;

    void Start()
    {
        StartCoroutine(Init());
    }
    
    IEnumerator Init()
    {
        yield return null;
        yield return null;
        yield return null;

        statSlider = GetComponent<Slider>();
        Debug.Assert(statSlider != null, $"{statType} StatBar: statSlider is not assigned.");
        Debug.Assert(statText != null, $"{statType} StatBar: statText is not assigned.");
        
        statText.text = $"{GameManager.Instance.GetStatKoreanName(statType)}";

        statSlider.maxValue = GameManager.Instance.GetMaxStat(statType);

        SetUI(GameManager.Instance.GetStat(statType));
        GameManager.Instance.SubscribeOnChanged(statType, SetUI);
    }

    void SetUI(int statValue)
    {
        statSlider.value = statValue;
        if(valueText != null) valueText.text = $"{statValue}/{statSlider.maxValue}";
    }

    void OnDestroy()
    {
        GameManager.Instance.UnsubscribeOnChanged(statType, SetUI);
    }
}

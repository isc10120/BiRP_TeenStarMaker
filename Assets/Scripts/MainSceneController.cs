using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    [Header("Practice Cost Settings")]
    [SerializeField] int songWritePracCost;
    [SerializeField] int kpopPracCost;
    [SerializeField] int trotPracCost;

    [Header("Practice Up Value Settings")]
    [SerializeField] int songWritePracUpValue;
    [SerializeField] int kpopPracUpValue;
    [SerializeField] int trotPracUpValue;

    [Header("GameObject Reference")]
    [SerializeField] TMP_Text dateText;
    [SerializeField] Button SleepButton;
    [SerializeField] TMP_Text userNameText;
    [SerializeField] GameObject FadeInImg;

    private Dictionary<StatType, int> practiceCostDict = new Dictionary<StatType, int>();
    private Dictionary<StatType, int> practiceUpValueDict = new Dictionary<StatType, int>();

    GameManager gameManager;

    void Init()
    {
        gameManager = GameManager.Instance;

        practiceCostDict.Add(StatType.SingerSongwriter, songWritePracCost);
        practiceCostDict.Add(StatType.KPop, kpopPracCost);
        practiceCostDict.Add(StatType.Trot, trotPracCost);
        practiceUpValueDict.Add(StatType.SingerSongwriter, songWritePracUpValue);
        practiceUpValueDict.Add(StatType.KPop, kpopPracUpValue);
        practiceUpValueDict.Add(StatType.Trot, trotPracUpValue);

        gameManager.onDayChanged += SetDateText;
        userNameText.text = gameManager.GetName();
        SleepButton.onClick.AddListener(GoNextDay);

        GameManager.Instance.GoNextDay();
    }

    void GoNextDay()
    {
        FadeInImg.SetActive(true);
        StartCoroutine(WaitFadeTime());
    }

    IEnumerator WaitFadeTime()
    {
        yield return new WaitForSeconds(1);
        gameManager.GoNextDay();
    }

    void OnDestroy()
    {
        gameManager.onDayChanged -= SetDateText;
    }
    
    void SetDateText()
    {
        dateText.text = $"{gameManager.GetMonth():00}월 {gameManager.GetDay():00}일";
    }

    public int GetPracticeCost(StatType type)
    {
        return practiceCostDict[type];
    }

    public int GetPracticeCostUpValue(StatType type)
    {
        return practiceUpValueDict[type];
    }

    public static MainSceneController Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

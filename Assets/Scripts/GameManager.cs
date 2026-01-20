using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class GameData
{
    public Dictionary<StatType, int> statValueDict = new Dictionary<StatType, int>();
    public Dictionary<PartsType, string> wornPartsDict = new Dictionary<PartsType, string>();
    // public Dictionary<string, bool> isPartsOpened = new Dictionary<string, bool>();
    public BodySize bodySize = BodySize.Small;
    public int day = 0;
    public int month = 0;
    public string name = "뿌까";
}

public enum BodySize
{
    Small,
    Medium,
    Large
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] int energyRecover = 100;
    [SerializeField] int maxDay = 6;
    [SerializeField] private Stat[] stats;
    [SerializeField] private CharacterParts[] characterParts;
    public CharacterParts defaultClothes_Top;
    public CharacterParts defaultClothes_Bottom;
    public CharacterParts defaultClothes_Shoes;
    public CharacterParts defaultClothes_Hair;
    // 한벌옷(세트)는 디폴트 id = "default"이며 스프라이트 없음

    private GameData gameData = new GameData();
    private Dictionary<StatType, Stat> statsDict = new Dictionary<StatType, Stat>();
    private Dictionary<string, CharacterParts> partsDict = new Dictionary<string, CharacterParts>();
    private Dictionary<StatType, Action<int>> onChangedActionDict = new Dictionary<StatType, Action<int>>();
    private Dictionary<PartsType, Action<String>> onChangedPartsActionDict = new Dictionary<PartsType, Action<string>>();
    public Action onBodySizeChanged;
    public Action onDayChanged;

    public void SetName(string name)
    {
        gameData.name = name;
    }

    public string GetName()
    {
        return gameData.name;
    }
    
    void GoToEnding()
    {
        Debug.Log(GetStat(StatType.Trot));
        Debug.Log(GetStat(StatType.KPop));
        Debug.Log(GetStat(StatType.SingerSongwriter));
        Debug.Log(GetStat(StatType.Popularity));
        Debug.Log(GetStat(StatType.Visual));
        Debug.Log(GetStat(StatType.Trot));

        if (GetStat(StatType.Trot) >= 80 && GetStat(StatType.Popularity) >= 50 && GetStat(StatType.Visual) >= 50)
        {
            SceneManager.LoadScene("Scene - EndTrot");
        }
        else if (GetStat(StatType.KPop) >= 80 && GetStat(StatType.Popularity) >= 55 && GetStat(StatType.Visual) >= 80)
        {
            SceneManager.LoadScene("Scene - EndKpop");
        }
        else if (GetStat(StatType.SingerSongwriter) >= 80 && GetStat(StatType.Popularity) >= 30)
        {
            SceneManager.LoadScene("Scene - EndSingwrite");
        }
        else
        {
            SceneManager.LoadScene("Scene - EndFail");
        }
    }

    public void GoNextDay()
    {
        gameData.day = UnityEngine.Random.Range(1, 32); // 1 이상, 32 미만 → 1~31
        gameData.month++;
        if (gameData.month > maxDay)
        {
            GoToEnding();
            return;
        }
        onDayChanged?.Invoke();
        // SetStat(StatType.Energy, GetMaxStat(StatType.Energy));
        AddStat(StatType.Energy, energyRecover);
    }

    public void SetBodySize(BodySize size)
    {
        gameData.bodySize = size;
        onBodySizeChanged?.Invoke();
    }

    public BodySize GetBodySize()
    {
        return gameData.bodySize;
    }

    public int GetDay()
    {
        return gameData.day;
    }
    public int GetMonth()
    {
        return gameData.month;
    }

    /// <summary>
    /// 현재 체형에 따른 스프라이트를 반환함
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Sprite GetPartsSprite(string id)
    {
        switch (gameData.bodySize)
        {
            case BodySize.Small:
                return partsDict[id].sprite_Small;
            case BodySize.Medium:
                return partsDict[id].sprite_Medium;
            case BodySize.Large:
                return partsDict[id].sprite_Large;
            default:
                return partsDict[id].sprite_Small;  // 컴파일러 오류 방지
        }
    }

    public int GetPartsCost(string id)
    {
        return partsDict[id].cost;
    }

    public List<string> GetAllPartsIdsByType(PartsType partsType)
    {
        List<string> partsIds = new List<string>();

        foreach (var part in characterParts)
        {
            if (part.partsType == partsType)
            {
                partsIds.Add(part.id);
            }
        }
        return partsIds;
    }

    public string GetWornPartsId(PartsType partsType)
    {
        return gameData.wornPartsDict[partsType];
    }

    public void ChangeParts(PartsType partsType, string id)
    {
        gameData.wornPartsDict[partsType] = id;
        
        switch (partsType)
        {
            case PartsType.Clothes_Top:
            case PartsType.Clothes_Bottom:
                if (gameData.wornPartsDict[PartsType.Clothes_Set] != "default")
                    ChangeParts(PartsType.Clothes_Set, "default");
                break;
            case PartsType.Clothes_Set:
                if (id == "default")
                    break;
                gameData.wornPartsDict[PartsType.Clothes_Top] = defaultClothes_Top.id;
                gameData.wornPartsDict[PartsType.Clothes_Bottom] = defaultClothes_Bottom.id;
                onChangedPartsActionDict[PartsType.Clothes_Top]?.Invoke(defaultClothes_Top.id);
                onChangedPartsActionDict[PartsType.Clothes_Bottom]?.Invoke(defaultClothes_Bottom.id);
                break;
        }

        if (onChangedPartsActionDict.ContainsKey(partsType))
        {
            onChangedPartsActionDict[partsType]?.Invoke(id);
        }
    }

    public int GetMaxStat(StatType statType)
    {
        return statsDict[statType].MaxValue;
    }

    public string GetStatKoreanName(StatType statType)
    {
        return statsDict[statType].koreanName;
    }

    public Sprite GetStatIcon(StatType statType)
    {
        return statsDict[statType].icon;
    }

    public int GetStat(StatType statType)
    {
        return gameData.statValueDict[statType];
    }

    public void AddStat(StatType statType, int value)
    {
        SetStat(statType, gameData.statValueDict[statType] + value);
    }

    public void SetStat(StatType statType, int value)
    {
        if (value < 0)
        {
            value = 0;
        }
        else if (value > statsDict[statType].MaxValue)
        {
            value = statsDict[statType].MaxValue;
        }

        gameData.statValueDict[statType] = value;

        if (onChangedActionDict.ContainsKey(statType))
        {
            onChangedActionDict[statType]?.Invoke(gameData.statValueDict[statType]);
        }

        // 체형변화
        if (statType == StatType.Visual)
        {
            int v = GetStat(StatType.Visual);
            if (v >= 60)
            {
                SetBodySize(BodySize.Medium);
            }
            else if (v >= 30)
            {
                SetBodySize(BodySize.Large);
            }
            else
            {
                SetBodySize(BodySize.Small);
            }
        }
    }

    public void SubscribeOnChanged(StatType statType, Action<int> action)
    {
        if (onChangedActionDict.ContainsKey(statType))
        {
            onChangedActionDict[statType] += action;
        }
        else
        {
            onChangedActionDict[statType] = action;
        }
    }

    public void UnsubscribeOnChanged(StatType statType, Action<int> action)
    {
        if (onChangedActionDict.ContainsKey(statType))
        {
            onChangedActionDict[statType] -= action;
        }
    }

    public void SubscribeOnChangedParts(PartsType partsType, Action<string> action)
    {
        if (onChangedPartsActionDict.ContainsKey(partsType))
        {
            onChangedPartsActionDict[partsType] += action;
        }
        else
        {
            onChangedPartsActionDict[partsType] = action;
        }
    }

    public void UnsubscribeOnChangedParts(PartsType partsType, Action<string> action)
    {
        if (onChangedPartsActionDict.ContainsKey(partsType))
        {
            onChangedPartsActionDict[partsType] -= action;
        }
    }

    public void Initialize()
    {
        gameData = new GameData();

        foreach (Stat stat in stats)
        {
            if (stat == null) continue;
            statsDict[stat.type] = stat;
            gameData.statValueDict[stat.type] = stat.BaseValue;
            Debug.Log($"Initialized {stat.type} with base value {stat.BaseValue}");
        }

        foreach (CharacterParts parts in characterParts)
        {
            if (parts == null) continue;
            partsDict[parts.id] = parts;
        }

        gameData.wornPartsDict[PartsType.Clothes_Top] = defaultClothes_Top.id;
        gameData.wornPartsDict[PartsType.Clothes_Bottom] = defaultClothes_Bottom.id;
        gameData.wornPartsDict[PartsType.Clothes_Shoes] = defaultClothes_Shoes.id;
        gameData.wornPartsDict[PartsType.Hair] = defaultClothes_Hair.id;
        gameData.wornPartsDict[PartsType.Clothes_Set] = "default";
        gameData.wornPartsDict[PartsType.Body] = "body";
    }


    void Awake()
    {
        base.Awake();
        Initialize();
    }

}

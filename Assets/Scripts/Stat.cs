using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public enum StatType
{
    Trot,
    SingerSongwriter,
    KPop,
    Energy,
    Popularity,
    Visual,
    Money
}

[CreateAssetMenu(fileName = "New Stat", menuName = "Stat")]
public class Stat : ScriptableObject
{
    public StatType type;
    public string koreanName;
    public int BaseValue = 0;
    public int MaxValue = 0;
    public Sprite icon;
}

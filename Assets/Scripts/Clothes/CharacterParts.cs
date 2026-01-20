using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartsType
{
    Hair,
    Face,
    Body,
    Clothes_Top,
    Clothes_Bottom,
    Clothes_Shoes,
    Clothes_Set,
}

[CreateAssetMenu(fileName = "New CharacterParts", menuName = "CharacterParts")]
public class CharacterParts : ScriptableObject
{
    public string id;
    public PartsType partsType;
    public Sprite sprite_Small;
    public Sprite sprite_Medium;
    public Sprite sprite_Large;
    public int cost;
}

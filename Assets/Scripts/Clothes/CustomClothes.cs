using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomClothes : MonoBehaviour
{
    [SerializeField] GameObject clothesCellPrefab;

    public GameObject GetCellPrefab()
    {
        return clothesCellPrefab;
    }
}

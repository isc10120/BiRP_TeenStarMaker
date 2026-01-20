using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] Image hair;
    [SerializeField] Image face;
    [SerializeField] Image body;
    [SerializeField] Image clothes_Top;
    [SerializeField] Image clothes_Bottom;
    [SerializeField] Image clothes_Shoes;
    [SerializeField] Image clothes_Set;

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


        gameManager = GameManager.Instance;

        SetHair(gameManager.defaultClothes_Hair.id);
        SetClothes_Top(gameManager.defaultClothes_Top.id);
        SetClothes_Bottom(gameManager.defaultClothes_Bottom.id);
        SetClothes_Shoes(gameManager.defaultClothes_Shoes.id);
        clothes_Set.gameObject.SetActive(false);

        gameManager.SubscribeOnChangedParts(PartsType.Hair, SetHair);
        // gameManager.SubscribeOnChangedParts(PartsType.Face, SetFace);
        // gameManager.SubscribeOnChangedParts(PartsType.Body, SetBody);
        gameManager.SubscribeOnChangedParts(PartsType.Clothes_Top, SetClothes_Top);
        gameManager.SubscribeOnChangedParts(PartsType.Clothes_Bottom, SetClothes_Bottom);
        gameManager.SubscribeOnChangedParts(PartsType.Clothes_Shoes, SetClothes_Shoes);
        gameManager.SubscribeOnChangedParts(PartsType.Clothes_Set, SetClothes_Set);
        gameManager.onBodySizeChanged += SetBody;
    }

    void OnDestroy()
    {
        gameManager.UnsubscribeOnChangedParts(PartsType.Hair, SetHair);
        // gameManager.UnsubscribeOnChangedParts(PartsType.Face, SetFace);
        // gameManager.UnsubscribeOnChangedParts(PartsType.Body, SetBody);
        gameManager.UnsubscribeOnChangedParts(PartsType.Clothes_Top, SetClothes_Top);
        gameManager.UnsubscribeOnChangedParts(PartsType.Clothes_Bottom, SetClothes_Bottom);
        gameManager.UnsubscribeOnChangedParts(PartsType.Clothes_Shoes, SetClothes_Shoes);
        gameManager.UnsubscribeOnChangedParts(PartsType.Clothes_Set, SetClothes_Set);
        gameManager.onBodySizeChanged -= SetBody;
    }

    void SetHair(string id)
    {
        SetParts(hair, id);
    }

    // void SetFace(string id)
    // {
    //     SetParts(face, id);
    // }

    void SetBody()
    {
        SetParts(body, "body");
        SetHair(gameManager.GetWornPartsId(PartsType.Hair));
        SetClothes_Top(gameManager.GetWornPartsId(PartsType.Clothes_Top));
        SetClothes_Bottom(gameManager.GetWornPartsId(PartsType.Clothes_Bottom));
        SetClothes_Shoes(gameManager.GetWornPartsId(PartsType.Clothes_Shoes));
        SetClothes_Set(gameManager.GetWornPartsId(PartsType.Clothes_Set));
    }

    void SetClothes_Top(string id)
    {
        SetParts(clothes_Top, id);
    }

    void SetClothes_Bottom(string id)
    {
        SetParts(clothes_Bottom, id);
    }

    void SetClothes_Shoes(string id)
    {
        SetParts(clothes_Shoes, id);
    }

    void SetClothes_Set(string id)
    {
        if (id == "default")
        {
            clothes_Set.gameObject.SetActive(false);
            clothes_Top.gameObject.SetActive(true);
            clothes_Bottom.gameObject.SetActive(true);
        }
        else
        {
            SetParts(clothes_Set, id);
            clothes_Set.gameObject.SetActive(true);
            clothes_Top.gameObject.SetActive(false);
            clothes_Bottom.gameObject.SetActive(false);
        }
    }

    void SetParts(Image image, string id)
    {
        image.sprite = gameManager.GetPartsSprite(id);
    }

}

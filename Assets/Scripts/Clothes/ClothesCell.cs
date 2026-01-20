using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothesCell : MonoBehaviour
{
    [SerializeField] string partsId;
    [SerializeField] PartsType partsType;
    [SerializeField] Button selectButton;
    [SerializeField] Button buyButton;
    [SerializeField] GameObject isSelectedImg;
    [SerializeField] TMP_Text costText;
    // [SerializeField] Image partsImage;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;

        selectButton.onClick.AddListener(SelectParts);
        gameManager.SubscribeOnChangedParts(partsType, judgeIsSelected);
        judgeIsSelected(gameManager.GetWornPartsId(partsType));

        costText.text = gameManager.GetPartsCost(partsId).ToString();
        // partsImage.sprite = gameManager.GetPartsSprite(partsId);
        // gameManager.onBodySizeChanged += SetSprite;
        gameManager.SubscribeOnChanged(StatType.Money, JudgeCanBuy);

        buyButton.onClick.AddListener(BuyParts);

        if (partsId == gameManager.defaultClothes_Bottom.id
        || partsId == gameManager.defaultClothes_Top.id
        || partsId == gameManager.defaultClothes_Shoes.id
        || partsId == gameManager.defaultClothes_Hair.id)
        {
            buyButton.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        gameManager.UnsubscribeOnChangedParts(partsType, judgeIsSelected);
        // gameManager.onBodySizeChanged -= SetSprite;
        gameManager.UnsubscribeOnChanged(StatType.Money, JudgeCanBuy);
    }

    void BuyParts()
    {
        gameManager.AddStat(StatType.Money, -gameManager.GetPartsCost(partsId));
        buyButton.gameObject.SetActive(false);
        costText.gameObject.SetActive(false);
    }

    // void SetSprite()
    // {
    //     partsImage.sprite = gameManager.GetPartsSprite(partsId);
    // }

    void SelectParts()
    {
        gameManager.ChangeParts(partsType, partsId);
    }

    void judgeIsSelected(string selectedPartsid)
    {
        if (selectedPartsid == partsId)
        {
            isSelectedImg.SetActive(true);
            selectButton.interactable = false;

            if ((partsType == PartsType.Clothes_Top || partsType == PartsType.Clothes_Bottom)
            && gameManager.GetWornPartsId(PartsType.Clothes_Set) != "default")
            {
                isSelectedImg.SetActive(false);
                selectButton.interactable = true;
            }
        }
        else
        {
            isSelectedImg.SetActive(false);
            selectButton.interactable = true;
        }

    }

    void JudgeCanBuy(int money)
    {
        if (money < gameManager.GetPartsCost(partsId))
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

}
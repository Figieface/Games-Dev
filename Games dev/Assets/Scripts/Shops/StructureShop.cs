using System;
using TMPro;
using UnityEngine;

public class StructureShop : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] public GameObject shopUI;
    public static int currency;

    private void Start()
    {
        currency = SetStartCurrency();
    }

    private int SetStartCurrency()
    {
        return 500; //for if we want to change this in future
    }

    private void Update() //buying and selling are done in placementstate and removingstate respectively
    {
        currencyText.text = $"Gold: {currency}";
    }

    public void ToggleShopUI()
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }
}

using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class TurretUIHudComponent : MonoBehaviour
{
    [SerializeField]
    private string product = "";

    public void Refresh()
    {
        int value = Systems.purchasesController.GetProductValue(product);
        TextMeshProUGUI coinsText = transform.Find("coinsText").GetComponent<TextMeshProUGUI>();
        Button button = transform.Find("Button - Turret").GetComponent<Button>();

        coinsText.SetText(value.ToString());
        button.interactable = value <= Systems.currencyModel.GetCoins();
    }
}

// Assets/Scripts/Exhibition/UI/InventorySlotUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Button button;

    private ItemData boundItem;
    private InventoryController controller;

    public void Bind(ItemData item, int count, InventoryController controller)
    {
        //boundItem = item;
        //this.controller = controller;
        //iconImage.sprite = item.Icon;
        //countText.text = count.ToString();
        //button.onClick.RemoveAllListeners();
        //button.onClick.AddListener(() => controller.OnItemSelected(boundItem));
    }

    public void UpdateCount(int count) => countText.text = count.ToString();
}
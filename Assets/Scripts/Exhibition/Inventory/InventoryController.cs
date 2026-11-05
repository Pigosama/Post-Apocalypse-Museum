// Assets/Scripts/Exhibition/Inventory/InventoryController.cs
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public void OnItemSelected(ItemData item)
    {
        if (!InventoryManager.Instance.HasItem(item))
        {
            Debug.Log("在庫がありません");
            return;
        }

        switch (item.Category)
        {
            case ItemCategory.Placable:
                var furniture = (PlaceableItemData)item;
                // wall || (floor & equipment)
                IInputMode mode = furniture.Layer == FurnitureLayer.Wall
                    ? new WallPlacementInput(furniture)
                    : new FloorPlacementInput(furniture);
                InputManager.Instance.SetMode(mode);
                break;

            case ItemCategory.Consumable:
                InputManager.Instance.SetMode(new ConsumableUseInput((ConsumableItemData)item));
                break;
        }

        //UIManager.Instance.CloseInventoryPanel(); // インベントリを閉じてマップ操作に戻る
    }
}
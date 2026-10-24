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
            case ItemCategory.Furniture:
                var furniture = (PlaceableItemData)item;
                /*IInputMode mode = furniture.Layer == FurnitureLayer.Floor
                    ? new FloorPlacementInput(furniture)
                    : new WallPlacementInput(furniture);*/
                //InputManager.Instance.SetMode(mode);
                break;

            case ItemCategory.Equipment:
                //InputManager.Instance.SetMode(new EquipmentPlacementInput((EquipmentData)item));
                break;

            case ItemCategory.Consumable:
                //InputManager.Instance.SetMode(new ConsumableUseInput((ConsumableItemData)item));
                break;
        }

        //UIManager.Instance.CloseInventoryPanel(); // インベントリを閉じてマップ操作に戻る
    }
}
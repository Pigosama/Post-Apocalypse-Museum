/*using UnityEngine;
using UnityEngine.UI;

public class InventoryItemHandler : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private int slotIndex;
    private InventorySlot currentSlot;

    [SerializeField] private GameObject itemImage;
    [SerializeField] private GameObject itemCount;
    

    public void InitializeItem(InventoryManager manager, int i, InventorySlot slot)
    {
        inventoryManager = manager;
        slotIndex = i;
        currentSlot = slot;

        itemImage.GetComponent<Button>().onClick.AddListener(OnSelected);

        DisplaySlot();
    }

    private void OnSelected()
    {
        inventoryManager.SelectSlot(slotIndex);
    }

    private void DisplaySlot()
    {
        GetComponent<Image>().sprite = currentSlot.itemData.ItemIcon;
    }
    
}*/

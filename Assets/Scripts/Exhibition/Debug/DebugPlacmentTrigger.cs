// Assets/Scripts/Exhibition/Debug/DebugPlacementTrigger.cs
using UnityEngine;

public class DebugPlacementTrigger : MonoBehaviour
{
    [SerializeField] private PlaceableItemData testWallItem;
    [SerializeField] private PlaceableItemData testFloorItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            InputManager.Instance.SetMode(new WallPlacementInput(testWallItem));

        if (Input.GetKeyDown(KeyCode.Alpha2))
            InputManager.Instance.SetMode(new FloorPlacementInput(testFloorItem));
    }

    void Start()
    {
        InventoryManager.Instance.AddItem(testWallItem, 10);
        InventoryManager.Instance.AddItem(testFloorItem, 10);
    }
}
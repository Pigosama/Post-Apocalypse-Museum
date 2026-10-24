using UnityEngine;
using System.Collections.Generic;

public class PlacedObject : MonoBehaviour
{
    [SerializeField] private PlaceableItemData itemData;
    public PlaceableItemData ItemData => itemData;

    public int RoomId {get; private set;}
    public List<Vector2Int> OccupiedCells { get; private set; } // 壁用
    public List<int> OccupiedXs { get; private set; }           // 床/設備用

    public void InitializeWall(PlaceableItemData data, int roomId, List<Vector2Int> cells)
    {
        itemData = data;
        RoomId = roomId;
        OccupiedCells = cells;
        SetupVisuals();
    }
    public void InitializeFloor(PlaceableItemData data, int roomId, List<int> xs)
    {
        itemData = data;
        RoomId = roomId;
        OccupiedXs = xs;
        SetupVisuals();
    }
    private void SetupVisuals()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && itemData.ItemSprite != null)
        {
            sr.sprite = itemData.ItemSprite;
        }
    }


}

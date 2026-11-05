using System.Collections.Generic;
using UnityEngine;

public enum FurnitureLayer { Floor, Wall, Equipment }

[CreateAssetMenu(fileName = "PlaceableItemData", menuName = "Scriptable Objects/PlaceableItemData")]
public class PlaceableItemData : ItemData
{
    [Header("Placement")]
    [SerializeField] private FurnitureLayer layer;
    [SerializeField] private int floorWidth = 1;
    [SerializeField] private List<Vector2Int> wallFootprint = new() { Vector2Int.zero };
    [SerializeField] private GameObject placePrefab;
    [SerializeField] private Sprite itemSprite;

    public override ItemCategory Category => ItemCategory.Placable;
    public FurnitureLayer Layer => layer;
    public int FloorWidth => floorWidth;
    public List<Vector2Int> WallFootprint => wallFootprint;
    public Sprite ItemSprite => itemSprite;
    public GameObject PlacePrefab => placePrefab;


    public List<Vector3Int> GetFootprintAsVector3Int()
    {
        List<Vector3Int> footprint = new();

        if (layer != FurnitureLayer.Wall)
        {
            for (int x = 0; x < floorWidth; x++)
            {
                footprint.Add(new Vector3Int(x, 0, 0));
            }
        }
        else
        {
            foreach (Vector2Int offset in wallFootprint)
            {
                footprint.Add(new Vector3Int(offset.x, offset.y, 0));
            }
        }

        return footprint;
    }
}
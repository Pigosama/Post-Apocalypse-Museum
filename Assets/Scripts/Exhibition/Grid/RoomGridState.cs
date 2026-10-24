using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil.Cil;

public class RoomGridState
{
    public int RoomId { get; }

    private readonly Dictionary<Vector2Int, PlacedObject> wallOccupancy = new();
    private readonly Dictionary<int, PlacedObject> equipmentOccupancy = new();
    private readonly Dictionary<int, PlacedObject> floorOccupancy = new();

    public RoomGridState(int roomId) => RoomId = roomId;

    // Wall (2D)
    public bool CanPlaceWall(List<Vector2Int> cells) 
        => cells.TrueForAll(c => !wallOccupancy.ContainsKey(c));
    public void PlaceWall(List<Vector2Int> cells, PlacedObject obj)
    {
        foreach (var c in cells) wallOccupancy[c] = obj;
    }
    public void RemoveWall(List<Vector2Int> cells)
    {
        foreach (var c in cells) wallOccupancy.Remove(c);
    }

    public bool TryGetWallObject(Vector2Int cellPos, out PlacedObject obj)
        => wallOccupancy.TryGetValue(cellPos, out obj);

    // Equipment (1D) — Floorと全く同じ構造だが別レイヤーなので辞書も別
    public bool CanPlaceEquipment(List<int> xs)
        => xs.TrueForAll(x => !equipmentOccupancy.ContainsKey(x));
    public void PlaceEquipment(List<int> xs, PlacedObject obj)
    {
        foreach (var x in xs) equipmentOccupancy[x] = obj;
    }
    public void RemoveEquipment(List<int> xs)
    {
        foreach (var x in xs) equipmentOccupancy.Remove(x);
    }
    public bool TryGetEquipmentObject(int x, out PlacedObject equipment)
        => equipmentOccupancy.TryGetValue(x, out equipment);

    // Floor (1D)
    public bool CanPlaceFloor(List<int> xs)
        => xs.TrueForAll(x => !floorOccupancy.ContainsKey(x));
    public void PlaceFloor(List<int> xs, PlacedObject obj)
    {
        foreach (var x in xs) floorOccupancy[x] = obj;
    }
    public void RemoveFloor(List<int> xs)
    {
        foreach (var x in xs) floorOccupancy.Remove(x);
    }
    public bool TryGetFloorObject(int x, out PlacedObject obj)
        => floorOccupancy.TryGetValue(x, out obj);
}
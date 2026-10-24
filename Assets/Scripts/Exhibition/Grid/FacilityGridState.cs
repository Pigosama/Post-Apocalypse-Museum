// Assets/Scripts/Exhibition/Grid/FacilityGridState.cs
using System.Collections.Generic;
using UnityEngine;

public class FacilityGridState : MonoBehaviour
{
    public static FacilityGridState Instance { get; private set; }
    void Awake() => Instance = this;

    [SerializeField] private Grid grid; // 施設グリッドのGridコンポーネント(Cell Size設定に従う)

    public enum TileType { Empty, Corridor, Room, Stairs, Elevator }

    private readonly Dictionary<Vector2Int, TileType> tileMap = new();
    private readonly Dictionary<Vector2Int, RoomInstance> roomTileMap = new();

    public TileType GetTileType(Vector2Int pos)
        => tileMap.TryGetValue(pos, out var t) ? t : TileType.Empty;

    public RoomInstance GetRoomAt(Vector2Int pos)
        => roomTileMap.TryGetValue(pos, out var room) ? room : null;

    public bool TryGetRoomPosition(Vector3 worldPos, out RoomPosition roomPos, out RoomInstance room)
    {
        Vector2Int facilityGridPos = WorldToGrid(worldPos);
        room = GetRoomAt(facilityGridPos);

        if (room == null)
        {
            roomPos = default;
            return false;
        }

        int localX = facilityGridPos.x - room.Origin.x;
        roomPos = new RoomPosition(room.RoomId, localX);
        return true;
    }

    public bool CanPlaceRoom(List<Vector2Int> footprint)
    {
        foreach (var cell in footprint)
            if (GetTileType(cell) != TileType.Empty) return false;
        return true;
    }

    public void PlaceRoom(List<Vector2Int> footprint, RoomInstance room)
    {
        foreach (var cell in footprint)
        {
            tileMap[cell] = TileType.Room;
            roomTileMap[cell] = room;
        }
    }

    /// <summary>
    /// グリッド座標(整数)→ワールド座標。GridのCell Size設定に従う。
    /// 部屋の原点は「セルの左下角」を採用(中心ではない)
    /// </summary>
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return grid.CellToWorld(new Vector3Int(gridPos.x, gridPos.y, 0));
    }

    /// <summary>
    /// ワールド座標→グリッド座標(逆算、クリック判定等で使用)
    /// </summary>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3Int cell = grid.WorldToCell(worldPos);
        return new Vector2Int(cell.x, cell.y);
    }

    /// <summary>
    /// 1セルぶんのワールドサイズ(部屋のlocalScale調整に使用)
    /// </summary>
    public Vector3 CellSize => grid.cellSize;
}
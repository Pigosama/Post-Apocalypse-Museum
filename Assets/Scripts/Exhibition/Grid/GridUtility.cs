// Assets/Scripts/Grid/GridUtility.cs
using System.Collections.Generic;
using UnityEngine;

/*public static class GridUtility
{
    public const float CellSize = 1f; // タイル1マスのワールド上のサイズ

    // 単一セル変換(ホバー中のカーソル位置など、1点だけ知りたい場合)
    public static Vector2Int WorldToGridSingle(Vector2 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / CellSize),
            Mathf.FloorToInt(worldPos.y / CellSize)
        );
    }

    // 複数セル変換(家具のサイズを考慮して占有マスのリストを返す)
    public static List<Vector2Int> WorldToGrid(Vector2 worldPos, Vector2Int size)
    {
        Vector2Int origin = WorldToGridSingle(worldPos);
        var cells = new List<Vector2Int>(size.x * size.y);

        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                cells.Add(new Vector2Int(origin.x + x, origin.y + y));

        return cells;
    }

    public static Vector2 GridToWorld(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x * CellSize, gridPos.y * CellSize);
    }
}*/
// Assets/Scripts/Grid/GridConvert.cs
using UnityEngine;

public static class GridConvert
{
    public static Vector3Int ToVector3Int(Vector2Int v) => new Vector3Int(v.x, v.y, 0);
    public static Vector2Int ToVector2Int(Vector3Int v) => new Vector2Int(v.x, v.y);
}
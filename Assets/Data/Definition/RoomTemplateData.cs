// Assets/Scripts/Data/Definition/RoomTemplateData.cs
using System.Collections.Generic;
using UnityEngine;

public enum RoomCategory { Corridor, Exhibition, ServiceElevator }

[CreateAssetMenu(fileName = "RoomTemplateData", menuName = "Scriptable Objects/RoomTemplateData")]
public class RoomTemplateData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string templateId;
    [SerializeField] private string displayName;
    [SerializeField] private RoomCategory category;

    [Header("Shape")]
    [SerializeField] private Vector2Int size = new Vector2Int(6, 6); // 幅 x 高さ(マス)

    [Header("Visual / Prefab")]
    [SerializeField] private GameObject roomPrefab;

    [Header("Exhibition Only")]
    [Tooltip("展示室として使う場合のキャパシティ。廊下/エレベータでは無視される")]
    [SerializeField] private int capacity;
    [Tooltip("アップグレード先テンプレート。最終形態ならnull")]
    [SerializeField] private RoomTemplateData upgradeTarget;

    public string TemplateId => templateId;
    public string DisplayName => displayName;
    public RoomCategory Category => category;
    public Vector2Int Size => size;
    public GameObject RoomPrefab => roomPrefab;
    public int Capacity => capacity;
    public RoomTemplateData UpgradeTarget => upgradeTarget;

    /// <summary>
    /// 指定した原点(左下マス)にこのテンプレートを置いた場合の占有マス一覧
    /// </summary>
    public List<Vector2Int> GetFootprint(Vector2Int origin)
    {
        var cells = new List<Vector2Int>(size.x * size.y);
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                cells.Add(origin + new Vector2Int(x, y));
        return cells;
    }
}
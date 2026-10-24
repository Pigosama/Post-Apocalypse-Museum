using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/EquipmentData")]
public class EquipmentData : ItemData
{
    public override ItemCategory Category => ItemCategory.Equipment;
    public string equipmentId;
    public string equipmentName;
    public int floorWidth; // 占有幅(床家具と同じ1D想定でよいか要確認)

    public List<EquipmentLevelData> levels; // レベルごとの性能・見た目
}

[System.Serializable]
public class EquipmentLevelData
{
    public int level;
    public Sprite visual;      // 見た目
    public int maxHp;          // 性能(バリケードならHP)
    public int attackPower;    // 性能(タレットなら攻撃力)
    public GameObject prefab;
}
// Assets/Scripts/Data/Definition/ConsumableItemData.cs
using UnityEngine;

public enum ConsumableEffectType { RestoreMentalState, MemoryWipe }

[CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Scriptable Objects/ConsumableItemData")]
public class ConsumableItemData : ItemData
{
    [Header("Effect")]
    [SerializeField] private ConsumableEffectType effectType;
    [SerializeField] private int effectRadius = 2;   // 散布の効果範囲(Xマス)
    [SerializeField] private float effectPower = 30f; // 回復量など

    public override ItemCategory Category => ItemCategory.Consumable;
    public ConsumableEffectType EffectType => effectType;
    public int EffectRadius => effectRadius;
    public float EffectPower => effectPower;
}
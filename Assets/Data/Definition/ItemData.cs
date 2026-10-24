// Assets/Scripts/Data/Definition/ItemData.cs
using UnityEngine;

public enum ItemCategory { Furniture, Equipment, Consumable }

public abstract class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string itemId;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private int maxStackSize = 99;

    public string ItemId => itemId;
    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;
    public int MaxStackSize => maxStackSize;

    public abstract ItemCategory Category { get; }
}
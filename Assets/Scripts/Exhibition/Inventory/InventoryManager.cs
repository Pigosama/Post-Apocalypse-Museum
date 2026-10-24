// Assets/Scripts/Exhibition/Inventory/InventoryManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    void Awake() => Instance = this;

    private readonly Dictionary<ItemData, int> items = new();

    public event Action<ItemData, int> OnItemCountChanged; // UI更新用

    public void AddItem(ItemData item, int amount = 1)
    {
        items.TryGetValue(item, out int current);
        int newAmount = Mathf.Min(current + amount, item.MaxStackSize);
        items[item] = newAmount;
        OnItemCountChanged?.Invoke(item, newAmount);
    }

    public bool HasItem(ItemData item, int amount = 1)
        => items.TryGetValue(item, out int current) && current >= amount;

    public bool TryConsumeItem(ItemData item, int amount = 1)
    {
        if (!HasItem(item, amount)) return false;

        items[item] -= amount;
        OnItemCountChanged?.Invoke(item, items[item]);
        return true;
    }

    public int GetCount(ItemData item) => items.TryGetValue(item, out int c) ? c : 0;

    public IEnumerable<KeyValuePair<ItemData, int>> AllItems => items;
}
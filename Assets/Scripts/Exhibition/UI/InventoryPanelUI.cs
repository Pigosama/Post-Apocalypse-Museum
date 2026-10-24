// Assets/Scripts/Exhibition/UI/InventoryPanelUI.cs
using UnityEngine;
using System.Collections.Generic;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField] private InventoryController controller;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;

    private readonly Dictionary<ItemData, InventorySlotUI> activeSlots = new();

    void OnEnable() => RefreshAll();

    void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnItemCountChanged -= HandleCountChanged;
    }

    void Start()
    {
        InventoryManager.Instance.OnItemCountChanged += HandleCountChanged;
    }

    private void RefreshAll()
    {
        foreach (Transform child in slotContainer) Destroy(child.gameObject);
        activeSlots.Clear();

        foreach (var kvp in InventoryManager.Instance.AllItems)
        {
            var slotGO = Instantiate(slotPrefab, slotContainer);
            var slot = slotGO.GetComponent<InventorySlotUI>();
            slot.Bind(kvp.Key, kvp.Value, controller);
            activeSlots[kvp.Key] = slot;
        }
    }

    private void HandleCountChanged(ItemData item, int newCount)
    {
        if (activeSlots.TryGetValue(item, out var slot))
            slot.UpdateCount(newCount);
        else
            RefreshAll(); // 新規アイテムが追加された場合は全体再構築
    }
}
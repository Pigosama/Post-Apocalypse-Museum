// Assets/Scripts/Exhibition/Interaction/LayerSelectionManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class LayerSelectionManager : MonoBehaviour
{
    public static LayerSelectionManager Instance { get; private set; }
    void Awake() => Instance = this;

    // 選択優先順位（高い順）
    private static readonly EditLayer[] PriorityOrder =
    {
        EditLayer.Entity,
        EditLayer.Equipment,
        EditLayer.Floor,
        EditLayer.Wall
    };
    // 選択可能なレイヤーの集合
    private readonly HashSet<EditLayer> enabledLayers = new()
    {
        EditLayer.Wall,
        EditLayer.Floor,
        EditLayer.Equipment,
        EditLayer.Entity
    };
    // レイヤーの有効/無効が変更された時に呼ばれるイベント
    public event Action<EditLayer, bool> OnLayerChanged;

    public bool IsLayerEnabled(EditLayer layer) => enabledLayers.Contains(layer);

    // 有効なレイヤーを優先順位順に列挙する
    public IEnumerable<EditLayer> GetEnabledLayersByPriority()
    {
        foreach (EditLayer layer in PriorityOrder)
        {
            if (enabledLayers.Contains(layer))
                yield return layer;
        }
    }

    public void SetLayer(EditLayer layer, bool isEnabled)
    {
        if (!Enum.IsDefined(typeof(EditLayer), layer))
            throw new ArgumentOutOfRangeException(nameof(layer), layer, "Unknown edit layer.");

        bool changed = isEnabled
            ? enabledLayers.Add(layer)
            : enabledLayers.Remove(layer);

        if (changed)
            OnLayerChanged?.Invoke(layer, isEnabled);
    }
}
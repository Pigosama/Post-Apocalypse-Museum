// Assets/Scripts/Exhibition/Interaction/LayerSelectionManager.cs
using System;
using UnityEngine;

public class LayerSelectionManager : MonoBehaviour
{
    public static LayerSelectionManager Instance { get; private set; }
    void Awake() => Instance = this;

    public EditLayer CurrentLayer { get; private set; } = EditLayer.Wall;   // レイヤー初期化
    public event Action<EditLayer> OnLayerChanged;

    public void SetLayer(EditLayer layer)
    {
        if (layer == CurrentLayer) return;
        CurrentLayer = layer;
        SelectionManager.Instance.ClearSelection(); // レイヤーを切り替えたら選択解除
        OnLayerChanged?.Invoke(layer);
    }
}
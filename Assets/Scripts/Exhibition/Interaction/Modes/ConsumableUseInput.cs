// Assets/Scripts/Exhibition/Interaction/Modes/ConsumableUseInput.cs
using UnityEngine;

public class ConsumableUseInput : IInputMode
{
    private readonly ConsumableItemData itemData;
    public bool BlocksSimulationInput => true;

    public ConsumableUseInput(ConsumableItemData data) => itemData = data;

    public void OnEnter() => UIManager.Instance.ShowConsumableCursorHint(itemData);
    public void OnExit() => UIManager.Instance.HideConsumableCursorHint();
    public void OnTick() { }
    public void OnPointerMove(Vector2 worldPos, GameObject hover) { }

    public void OnPrimaryAction(Vector2 worldPos, GameObject hit)
    {
        // クリックした対象からroomIdとXを取得する想定(部屋の特定方法は要検討、後述)
        if (!FacilityGridState.Instance.TryGetRoomPosition(worldPos, out RoomPosition clickedPos, out _)) return;

        if (!InventoryManager.Instance.TryConsumeItem(itemData)) return;

        var registry = RoomManager.Instance.GetOrCreateEntityRegistry(clickedPos.RoomId);
        var targets = registry.GetInRange(clickedPos.X, itemData.EffectRadius);

        foreach (var target in targets)
        {
            if (target.TryGetComponent<IVisitorEntity>(out var visitor))
                ApplyEffect(visitor);
        }

        InputManager.Instance.ReturnToDefaultMode(); // 1回使い切り。連続散布したいなら外す
    }

    public void OnSecondaryAction(Vector2 worldPos, GameObject hit)
        => InputManager.Instance.ReturnToDefaultMode();

    private void ApplyEffect(IVisitorEntity visitor)
    {
        switch (itemData.EffectType)
        {
            case ConsumableEffectType.RestoreMentalState:
                //visitor.RestoreMentalState(itemData.EffectPower);
                break;
            case ConsumableEffectType.MemoryWipe:
                //visitor.WipeMemory(); // パニック状態や精神状態異常のリセット等、詳細仕様は別途
                break;
        }
    }

    private bool TryGetRoomPosition(Vector2 worldPos, GameObject hit, out RoomPosition pos)
    {
        // TODO: 現状「クリック位置→どの部屋か」を判定する仕組みがまだ無いため、
        // 部屋の当たり判定(部屋ごとのCollider等)を別途用意する必要がある
        pos = default;
        return false;
    }
}
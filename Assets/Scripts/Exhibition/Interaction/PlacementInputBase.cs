// PlacementInputBase.cs
using UnityEngine;

public abstract class PlacementInputBase : IInputMode
{
    protected readonly PlaceableItemData itemData;
    public bool BlocksSimulationInput => true;

    protected PlacementInputBase(PlaceableItemData data) => itemData = data;

    public abstract void OnEnter();
    public abstract void OnTick();
    public abstract void OnPrimaryAction(Vector2 worldPos, GameObject hit);

    // OnExitは全モード共通なので、ここに実装を書いて子クラス側では書かなくてよい
    public virtual void OnExit()
    {
        PlacementManager.Instance.StopPlacing();
        PlacementManager.Instance.CancelMoveIfPending();
    }

    public void OnSecondaryAction(Vector2 worldPos, GameObject hit)
        => InputManager.Instance.ReturnToDefaultMode();
    public void OnPointerMove(Vector2 worldPos, GameObject hover) { }
}


// WallPlacementInput.cs
public class WallPlacementInput : PlacementInputBase
{
    public WallPlacementInput(PlaceableItemData data) : base(data) { }
    public override void OnEnter() => PlacementManager.Instance.StartPlacingWall(itemData);
    public override void OnTick() => PlacementManager.Instance.TickWallPlacement();
    public override void OnPrimaryAction(Vector2 worldPos, GameObject hit)
        => PlacementManager.Instance.TryPlaceWallAtCurrentCell();
}
public class FloorPlacementInput : PlacementInputBase
{
    public FloorPlacementInput(PlaceableItemData data) : base(data) { }
    public override void OnEnter() => PlacementManager.Instance.StartPlacingFloor(itemData);
    public override void OnTick() => PlacementManager.Instance.TickFloorPlacement();
    public override void OnPrimaryAction(Vector2 worldPos, GameObject hit)
        => PlacementManager.Instance.TryPlaceAtCurrentX();
}
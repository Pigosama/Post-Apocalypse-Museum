using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public interface IInputMode
{
    void OnEnter();                       // モードに入った時の初期化(ハイライト表示など)
    void OnExit();                        // モードを抜ける時のクリーンアップ
    void OnTick();                        // 毎フレーム呼ばれる（必要なら）
    void OnPrimaryAction(Vector2 worldPos, GameObject hitObject); // 左クリック相当
    void OnSecondaryAction(Vector2 worldPos, GameObject hitObject); // 右クリック(キャンセル等)
    void OnPointerMove(Vector2 worldPos, GameObject hoverObject);  // ホバー中のプレビュー表示
    bool BlocksSimulationInput { get; }   // このモード中はUI以外への入力を専有するか
}

public class NormalModeInput : IInputMode
{
    public bool BlocksSimulationInput => false;

    public void OnEnter() { /* 通常カーソルに戻す */ }
    public void OnExit() { }
    public void OnTick() { }

    public void OnPrimaryAction(Vector2 worldPos, GameObject hit)
    {
        var l = LayerSelectionManager.Instance.CurrentLayer;
        switch (l)
        {
            case EditLayer.Entity:
                SelectionManager.Instance.Select(hit);
                break;
            default:
                SelectPlacedObjectAt(worldPos, l);
                break;
        }
    }
    public void OnSecondaryAction(Vector2 worldPos, GameObject hit) { }
    public void OnPointerMove(Vector2 worldPos, GameObject hover) { }

    private void SelectPlacedObjectAt(Vector2 worldPos, EditLayer layer)
    {
        if(!FacilityGridState.Instance.TryGetRoomPosition(worldPos, out RoomPosition roomPos, out RoomInstance room))
        {
            SelectionManager.Instance.ClearSelection();
            return;
        }

        Vector2Int gridPos = FacilityGridState.Instance.WorldToGrid(worldPos);
        var state = room.GridState;
        // マップ上のオブジェクトを探す
        PlacedObject found = layer switch
        {
            EditLayer.Wall      => state.TryGetWallObject(gridPos, out var w) ? w : null,
            EditLayer.Floor     => state.TryGetFloorObject(roomPos.X, out var f) ? f : null,
            EditLayer.Equipment => state.TryGetEquipmentObject(roomPos.X, out var e) ? e : null,
            _ => null,
        };

        if(found != null)
            SelectionManager.Instance.SelectPlacedObject(found, room);
        else
            SelectionManager.Instance.ClearSelection();
    }
}

/*public class FurniturePlacementInput : IInputMode
{
    private readonly PlacementManager placementManager;
    private readonly PlaceableItemData furnitureToPlace;
    //private GameObject previewGhost;

    public bool BlocksSimulationInput => true;

    public FurniturePlacementInput(PlacementManager manager, PlaceableItemData data){
        placementManager = manager;
        furnitureToPlace = data;
    }

    public void OnEnter() => placementManager.StartPlacing(furnitureToPlace);
    public void OnExit() => placementManager.StopPlacing();
    public void OnTick() => placementManager.TickPlacement();

    public void OnPrimaryAction(Vector2 worldPos, GameObject hit) => placementManager.TryPlaceAtCurrentPosition();
    public void OnSecondaryAction(Vector2 worldPos, GameObject hit)
        => InputManager.Instance.ReturnToDefaultMode(); // 右クリックでキャンセル

    public void OnPointerMove(Vector2 worldPos, GameObject hover) { }
}*/

/*public class FurnitureRemovalInput : IInputMode
{
    private readonly PlacementManager placementManager;
    private readonly FurnitureLayer layer;

    public bool BlocksSimulationInput => true;

    public FurnitureRemovalInput(PlacementManager manager, FurnitureLayer layer)
    {
        placementManager = manager;
        this.layer = layer;
    }

    public void OnEnter()
    {
        // 特に初期化不要。カーソルを撤去用アイコンに変える等はここで
        //UIManager.Instance.ShowRemovalCursorHint();
    }
    public void OnExit()
    {
        //UIManager.Instance.HideRemovalCursorHint();
        //placementManager.ClearPreview();
    }
    public void OnTick(){}
    public void OnPrimaryAction(Vector2 worldPos, GameObject hit)
    {
        placementManager.TryRemoveAtCurrentPosition();
        // 連続撤去を許可するかはお好みで。許可するならここでReturnToDefaultModeを呼ばない
    }
    public void OnSecondaryAction(Vector2 worldPos, GameObject hit)
        => InputManager.Instance.ReturnToDefaultMode();
    public void OnPointerMove(Vector2 worldPos, GameObject hover) { }
}*/

public class AnomalySuppressionInput : IInputMode
{
    public bool BlocksSimulationInput => true;

    public void OnEnter() => UIManager.Instance.ShowSuppressionCursorHint();
    public void OnExit() => UIManager.Instance.HideSuppressionCursorHint();
    public void OnTick() { }
    public void OnPointerMove(Vector2 worldPos, GameObject hover) { }

    public void OnPrimaryAction(Vector2 worldPos, GameObject hit)
    {
        if (hit != null && hit.TryGetComponent<IAnomalyEntity>(out var anomaly)
            && anomaly.State == AnomalyState.Breached)
        {
            // ここでスタッフへタスクを発行(直接殴るのではなく、タスクキューに積む方がRimWorld的)
            // StaffManager.Instance.QueueTask(new SuppressAnomalyTask(anomaly));
            InputManager.Instance.ReturnToDefaultMode();
        }
    }

    public void OnSecondaryAction(Vector2 worldPos, GameObject hit)
        => InputManager.Instance.ReturnToDefaultMode();
}


public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask worldInteractableLayer;

    private IInputMode currentMode;
    private IInputMode defaultMode; // NormalModeInput

    void Awake() => Instance = this;

    void Start()
    {
        defaultMode = new NormalModeInput();
        SetMode(defaultMode);
    }

    public void SetMode(IInputMode newMode)
    {
        currentMode?.OnExit();
        currentMode = newMode;
        currentMode.OnEnter();
    }

    public void ReturnToDefaultMode() => SetMode(defaultMode);

    void Update()
    {
        // UI上のクリックはブロック（EventSystemで判定)
        if (IsPointerOverUI()) return;

        Vector2 worldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        GameObject hit = RaycastWorld(worldPos);

        currentMode.OnPointerMove(worldPos, hit);
        currentMode.OnTick();

        if (Mouse.current.leftButton.wasPressedThisFrame)
            currentMode.OnPrimaryAction(worldPos, hit);

        if (Mouse.current.rightButton.wasPressedThisFrame)
            currentMode.OnSecondaryAction(worldPos, hit);

        // Escで常にデフォルトモードへ(共通ショートカット)
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            ReturnToDefaultMode();
    }

    private GameObject RaycastWorld(Vector2 worldPos)
    {
        var hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f, worldInteractableLayer);
        return hit.collider ? hit.collider.gameObject : null;
    }

    private bool IsPointerOverUI() =>
        EventSystem.current.IsPointerOverGameObject();
}
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance { get; private set; }
    
    [Header("References")]
    private MainManager mainManager;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform floorObjectsParent;
    [SerializeField] private Transform wallObjectsParent;
    [SerializeField] private Tilemap previewTilemap;
    [SerializeField] TileBase previewTileRed;
    [SerializeField] TileBase previewTileGreen;

    
    [Header("Placement")]
    //[SerializeField] private LayerMask placedObjectLayer;
    private PlaceableItemData selectedItem;
    private GameObject ghostObject;
    private Vector2Int currentCellPosition;
    private RoomPosition currentRoomPosition; //1Dエンティティの座標
    private RoomInstance currentRoom;
    private bool isOverValidRoom;
    private List<Vector2Int> currentOccupyingCells;
    private List<int> currentFloorXs = new();
    private bool currentFloorValid;

    private bool isMoving;
    private PlaceableItemData moveItemData;

    private List<Vector2Int> GetOccupiedCells(Vector2Int origin, List<Vector2Int> footprint)
    {
        List<Vector2Int> cells = new();

        foreach (Vector2Int offset in footprint)
        {
            cells.Add(origin + offset);
        }

        return cells;
    }
    
    private Vector3Int ConvertToVector3Int(Vector2Int v2)
    {
        return new Vector3Int(v2.x, v2.y, 0);
    }
    

    private void Awake()
    {
        //isPlacing = mainController.IsOnPlacingMode;   // need a control from the other script
        mainManager = GetComponent<MainManager>();
        Instance = this;
    }
    private void Start()
    {
        //InitializeGhost();
        
    }
    private void InitializeGhost()
    {
        if (selectedItem == null)
            return;
        
        if (ghostObject != null)
            Destroy(ghostObject);

        ghostObject = new GameObject("GhostObject");
        SpriteRenderer sr = ghostObject.AddComponent<SpriteRenderer>();
        sr.sprite = selectedItem.ItemSprite;
        sr.color = new Color(1f, 1f, 1f, 0.5f);
        ghostObject.transform.SetParent(transform);
    }
   
    public void StopPlacing()
    {
        selectedItem = null;

        if (previewTilemap != null)
            previewTilemap.ClearAllTiles();

        if (ghostObject != null)
            Destroy(ghostObject);
        //isPlacing = false;
    }

    // RUNTIME
    public void TickWallPlacement()
    {
        if (selectedItem == null)
            return;
        
        DetectCurrentCell();
        currentOccupyingCells = GetOccupiedCells(currentCellPosition, selectedItem.WallFootprint);

        UpdateGhostPosition();
        UpdatePreviewTile();
    }
    public void TickFloorPlacement()
    {
        if (selectedItem == null) return;

        DetectCurrentCell();
        if (previewTilemap != null) previewTilemap.ClearAllTiles();

        if (!isOverValidRoom)
        {
            if (ghostObject != null) ghostObject.SetActive(false);
            return;
        }
        if (ghostObject != null) ghostObject.SetActive(true);

        int width = selectedItem.FloorWidth;
        int startX = Mathf.Clamp(currentRoomPosition.X, 0, currentRoom.Template.Size.x - width);

        currentFloorXs = new List<int>(width);
        for (int i = 0; i < width; i++) currentFloorXs.Add(startX + i);

        currentFloorValid = currentRoom.GridState.CanPlaceFloor(currentFloorXs);

        if (previewTilemap != null)
        {
            TileBase tile = currentFloorValid ? previewTileGreen : previewTileRed;
            int floorY = currentRoom.Origin.y;
            foreach (int x in currentFloorXs)
                previewTilemap.SetTile(new Vector3Int(currentRoom.Origin.x + x, floorY, 0), tile);
        }
    }

    /*public void TryRemoveAtCurrentPosition()
    {
        if (IsPointerOverUI())
            return;

        TryRemoveAt(layer: FurnitureLayer.Wall);
    }*/
    public void RemoveAndReturnToInventory(PlacedObject obj, RoomInstance room)
    {
        RemoveFromGrid(obj, room);
        InventoryManager.Instance.AddItem(obj.ItemData);
        Destroy(obj.gameObject);
        OnItemRemoved?.Invoke(obj.ItemData);
    }
    public void BeginMove(PlacedObject obj, RoomInstance room)
    {
        moveItemData = obj.ItemData;
        RemoveFromGrid(obj, room);
        Destroy(obj.gameObject);
        isMoving = true;

        IInputMode mode = moveItemData.Layer == FurnitureLayer.Floor
            ? new FloorPlacementInput(moveItemData)
            : new WallPlacementInput(moveItemData);
        InputManager.Instance.SetMode(mode);
    }
    public void CancelMoveIfPending()
    {
        if(!isMoving) return;
        InventoryManager.Instance.AddItem(moveItemData);
        isMoving = false;
        moveItemData = null;
    }

    public event System.Action<PlaceableItemData> OnItemPlaced;
    public event System.Action<PlaceableItemData> OnItemRemoved;


    // DETECTION
    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
    private void DetectCurrentCell()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -mainCamera.transform.position.z;
        Vector3 mouseWorldPos =
            mainCamera.ScreenToWorldPoint(mouseScreenPos);

        isOverValidRoom = FacilityGridState.Instance.TryGetRoomPosition(
            mouseWorldPos, out currentRoomPosition, out currentRoom);

        var cellpos = new Vector2Int(grid.WorldToCell(mouseWorldPos).x, grid.WorldToCell(mouseWorldPos).y);
        currentCellPosition = cellpos;
    }

    // INPUT
    public void StartPlacingWall(PlaceableItemData item)
    {
        selectedItem = item;
        InitializeGhost();
    }
    public void StartPlacingFloor(PlaceableItemData item)
    {
        selectedItem = item;
        InitializeGhost();
    }

    public void TryPlaceWallAtCurrentCell()
    {
        TryPlaceOnWall();
    }
    public void TryPlaceAtCurrentX()
    {
        if(!isOverValidRoom || !currentFloorValid)  return;
        TryPlaceOnFloor();
    }

    // GHOST
    private void UpdateGhostPosition()
    {
        if (ghostObject == null)
            return;
        
        //Vector3 worldPos = grid.GetCellCenterWorld(currentCellPosition);
        //ghostObject.transform.position = worldPos;
    }
    private void UpdatePreviewTile()
    {
        if(previewTilemap == null)
            return;
        previewTilemap.ClearAllTiles();

        var occupiedCells = GetOccupiedCells(currentCellPosition, selectedItem.WallFootprint);

        //bool canPlace = CanPlaceAtPosition(currentCellPosition, occupiedCells);
        bool canPlace = CanPlaceOnWall(occupiedCells);
        TileBase tile = canPlace ? previewTileGreen : previewTileRed;

        foreach (var cell in occupiedCells)
        {
            var cell3D = new Vector3Int(cell.x, cell.y, 0);
            previewTilemap.SetTile(cell3D, tile);
        }
    }

    // LOGICS
    // check
    private void TryPlaceOnWall()
    {
        if (!CanPlaceOnWall(currentOccupyingCells))
        {
            Debug.Log("Cannot place object at current position!");
            return;
        }

        PlaceObject();
    }
    private void TryPlaceOnFloor()
    {
        if (!isMoving && !InventoryManager.Instance.TryConsumeItem(selectedItem)) return;
        isMoving = false;

        int floorY = currentRoom.Origin.y;
        Vector3 worldPos = grid.GetCellCenterWorld(new Vector3Int(currentRoom.Origin.x + currentFloorXs[0], floorY, 0));

        GameObject newObj = Instantiate(selectedItem.PlacePrefab, worldPos, Quaternion.identity, floorObjectsParent);
        PlacedObject placedObj = newObj.GetComponent<PlacedObject>();
        if (placedObj == null) { Destroy(newObj); return; }

        placedObj.InitializeFloor(selectedItem, currentRoom.RoomId, new List<int>(currentFloorXs));
        currentRoom.GridState.PlaceFloor(currentFloorXs, placedObj);

        OnItemPlaced?.Invoke(selectedItem);
    }
    private bool CanPlaceOnWall(List<Vector2Int> cells)
    {
        // 部屋の中をクリックしているか
        if(!isOverValidRoom){
            //Debug.Log("Cannot place object outside of a valid room!");
            return false;
        }
        
        // 部屋からはみ出てないか
        foreach (var c in cells)
        {
            if(FacilityGridState.Instance.GetRoomAt(c) != currentRoom)
                return false;
            //cells.Add(c);
        }
        // 場所が空いているか
        return currentRoom.GridState.CanPlaceWall(cells);
        
    }

    /*private bool TryRemoveAt(FurnitureLayer layer)
    {
        PlacedObject obj = FindObjectAtCurrent(layer);
        if (obj == null)
        {
            Debug.Log("No object to remove at current position!");
            return false;
        }

        RemoveObject(obj);
        return true;
    }*/
    /*private PlacedObject FindObjectAtCurrent(FurnitureLayer layer)
    {
        if(currentRoom == null) return null;
        var state = currentRoom.GridState;

        switch (layer)
        {
            case FurnitureLayer.Wall:
                return state.TryGetWallObject(currentCellPosition, out var wallObj) ? wallObj : null;
            case FurnitureLayer.Floor:
                return state.TryGetFloorObject(currentCellPosition.x, out var floorObj) ? floorObj : null;
        }

        return null;
    }*/
    
    // EXECUTION
    private void PlaceObject()
    {
        if (!isMoving && !InventoryManager.Instance.TryConsumeItem(selectedItem)) return;
        isMoving = false;

        // Prefab生成
        Vector3 worldPos = grid.GetCellCenterWorld(ConvertToVector3Int(currentCellPosition));
        
        GameObject newObj = Instantiate(selectedItem.PlacePrefab, worldPos, Quaternion.identity, wallObjectsParent);
        PlacedObject placedObj = newObj.GetComponent<PlacedObject>();
        if (placedObj == null)
        {
            Debug.LogError("PlacedObject component is missing.");
            Destroy(newObj);
            return;
        }

        placedObj.InitializeWall(selectedItem, currentRoom.RoomId, currentOccupyingCells);

        // 壁の占有辞書に登録
        currentRoom.GridState.PlaceWall(currentOccupyingCells, placedObj);

        // end placing
        //mainController.OnItemPlaced(selectedItem);
        OnItemPlaced?.Invoke(selectedItem);
        Debug.Log($"設置完了: {selectedItem.ItemName} / 残り在庫: {InventoryManager.Instance.GetCount(selectedItem)}");
    }
    /*private void RemoveObject(PlacedObject obj)
    {
        PlaceableItemData data = obj.ItemData;
        var state = currentRoom.GridState;

        // 壁の占有辞書から削除
        switch (data.Layer)
        {
            case FurnitureLayer.Wall:  state.RemoveWall(obj.OccupiedCells); break;
            case FurnitureLayer.Floor: state.RemoveFloor(obj.OccupiedXs);   break;
        }

        Destroy(obj.gameObject);
        OnItemRemoved?.Invoke(data);
    }*/
    private void RemoveFromGrid(PlacedObject obj, RoomInstance room)
    {
        var state = room.GridState;
        switch (obj.ItemData.Layer)
        {
            case FurnitureLayer.Wall:   state.RemoveWall(obj.OccupiedCells); break;
            case FurnitureLayer.Floor:  state.RemoveFloor(obj.OccupiedXs);   break;
        }
    }

}

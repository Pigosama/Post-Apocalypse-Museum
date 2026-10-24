using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }
    void Awake() => Instance = this;

    public GameObject CurrentSelection { get; private set; }
    public PlacedObject SelectedPlacedObject {get; private set;}
    private RoomInstance selectedRoom;


    [Header("Highlight")]
    [SerializeField] private Color highlightColor = Color.yellow;
    private SpriteRenderer currentHighlightRenderer;
    private Color originalColor;

    public void Select(GameObject target)
    {
        if (target == CurrentSelection) return; // 同じ対象を再クリックしても何もしない

        ClearHighlight();
        SelectedPlacedObject = null;
        CurrentSelection = target;

        if (target == null)
        {
            ClearSelection();
            return;
        }

        ApplyHighlight(target);

        // 型ごとにディスパッチ。今後IExhibitEntity等が増えたらここに追加していく
        if (target.TryGetComponent<IVisitorEntity>(out var visitor))
        {
            UIManager.Instance.ShowVisitorPanel(visitor);
        }
        else if (target.TryGetComponent<IAnomalyEntity>(out var anomaly))
        {
            UIManager.Instance.ShowAnomalyPanel(anomaly);
        }
        else
        {
            // 選択可能だが専用パネルを持たない対象(家具など)の場合はパネルを閉じるだけ
            UIManager.Instance.HideAllInfoPanels();
        }
    }

    // PLACED OBJECT
    public void SelectPlacedObject(PlacedObject obj, RoomInstance room)
    {
        if(obj == SelectedPlacedObject) return;

        ClearHighlight();
        CurrentSelection = null;
        SelectedPlacedObject = obj;
        selectedRoom = room;

        ApplyHighlight(obj.gameObject);
        UIManager.Instance.ShowPlacedObjectPanel(obj);
    }
    public void RequestMoveSelected()
    {
        if (SelectedPlacedObject == null)   return;
        PlacementManager.Instance.BeginMove(SelectedPlacedObject, selectedRoom);
        ClearSelection();
    }
    public void RequestRemoveSelected()
    {
        if (SelectedPlacedObject == null)   return;
        PlacementManager.Instance.RemoveAndReturnToInventory(SelectedPlacedObject, selectedRoom);
        ClearSelection();
    }
    public void ClearSelection()
    {
        ClearHighlight();
        CurrentSelection = null;
        SelectedPlacedObject = null;
        UIManager.Instance.HideAllInfoPanels();
        UIManager.Instance.HidePlacedObjectPanel();
    }

    // HIGH LIGHT
    private void ApplyHighlight(GameObject target)
    {
        if (!target.TryGetComponent<SpriteRenderer>(out var sr))
            return;

        currentHighlightRenderer = sr;
        originalColor = sr.color;
        sr.color = highlightColor;
    }
    private void ClearHighlight()
    {
        if(currentHighlightRenderer == null) return;
        currentHighlightRenderer.color = originalColor;
        currentHighlightRenderer = null;
    }

}

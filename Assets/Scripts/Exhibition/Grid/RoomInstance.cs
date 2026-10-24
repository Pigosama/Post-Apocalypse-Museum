using UnityEngine;

public class RoomInstance : MonoBehaviour
{
    public int RoomId { get; private set; }
    public RoomTemplateData Template { get; private set; }
    public Vector2Int Origin { get; private set; } // 施設グリッド上の左下座標
    public RoomPurpose Purpose { get; private set; } = RoomPurpose.Unassigned;
    public RoomGridState GridState { get; private set; }

    [Header("References")]
    [SerializeField] private Transform furnitureContainer; // このRoomの床/壁家具・設備の親(整理用)
    public Transform FurnitureContainer => furnitureContainer;

    [Header("Visual (Optional)")]
    [SerializeField] private SpriteRenderer wallpaperRenderer; // 部屋の背景色/見た目の識別用(なくても可)


    public void Initialize(int roomId, RoomTemplateData template, Vector2Int origin)
    {
        RoomId = roomId;
        Template = template;
        Origin = origin;

        Vector3 cellSize = FacilityGridState.Instance.CellSize;
        wallpaperRenderer.transform.localScale = new Vector3(
            template.Size.x * cellSize.x,
            template.Size.y * cellSize.y,
            1f
        );

        GridState = RoomManager.Instance.GetOrCreateGridState(RoomId);
    }

    public void SetPurpose(RoomPurpose purpose)
    {
        Purpose = purpose;
        // TODO: 用途変更時のバリデーション(例: 展示中のアノマリーがいる部屋をStorageに変更できないようにする等)
    }

    public void SetWallpaper(Sprite sprite) => wallpaperRenderer.sprite = sprite;
}
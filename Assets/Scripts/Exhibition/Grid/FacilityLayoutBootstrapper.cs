// Assets/Scripts/Exhibition/Grid/FacilityLayoutBootstrapper.cs
using UnityEngine;

public class FacilityLayoutBootstrapper : MonoBehaviour
{
    [SerializeField] private RoomTemplateData standardRoomTemplate; // Room_Standard_Aをここにドラッグ
    [SerializeField] private RoomTemplateData corridorTemplate; // Room_Large_Aをここにドラッグ
    [SerializeField] private RoomTemplateData largeRoomTemplate; // Room_Large_Aをここにドラッグ
    [SerializeField] private RoomTemplateData elevatorTemplate; // Elevatorをここにドラッグ

    private int nextRoomId = 0;

    void Start()
    {
        PlaceRoom(corridorTemplate, new Vector2Int(0, 0));
        PlaceRoom(standardRoomTemplate, new Vector2Int(6, 7));
        PlaceRoom(largeRoomTemplate, new Vector2Int(18, 7));
        PlaceRoom(standardRoomTemplate, new Vector2Int(30, 7));
        PlaceRoom(elevatorTemplate, new Vector2Int(40, 0));
    }

    private void PlaceRoom(RoomTemplateData template, Vector2Int origin)
    {
        var footprint = template.GetFootprint(origin);

        if (!FacilityGridState.Instance.CanPlaceRoom(footprint))
        {
            Debug.LogWarning($"配置失敗: {template.DisplayName} は既に占有されているマスと重なっています");
            return;
        }

        Vector3 worldPos = FacilityGridState.Instance.GridToWorld(origin);
        var go = Instantiate(template.RoomPrefab, worldPos, Quaternion.identity);

        var instance = go.GetComponent<RoomInstance>();
        instance.Initialize(nextRoomId++, template, origin);

        FacilityGridState.Instance.PlaceRoom(footprint, instance);
    }
}
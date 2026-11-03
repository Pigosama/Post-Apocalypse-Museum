using UnityEngine;
using UnityEngine.UI;

public class LayerUI : MonoBehaviour
{
    [SerializeField] private Toggle wallToggle;
    [SerializeField] private Toggle floorToggle;
    [SerializeField] private Toggle equipmentToggle;
    [SerializeField] private Toggle entityToggle;

    private LayerSelectionManager layerSelectionManager;
    private bool listenersRegistered;

    private void Awake()
    {
        if (wallToggle == null || floorToggle == null ||
            equipmentToggle == null || entityToggle == null)
        {
            Debug.LogError("LayerUI requires all four layer toggles to be assigned.", this);
            enabled = false;
        }
    }

    private void Start()
    {
        layerSelectionManager = LayerSelectionManager.Instance;
        if (layerSelectionManager == null)
        {
            Debug.LogError("LayerUI requires a LayerSelectionManager in the scene.", this);
            enabled = false;
            return;
        }

        RegisterListeners();
        SynchronizeToggles();
    }

    private void OnEnable()
    {
        if (layerSelectionManager == null)
            return;

        RegisterListeners();
        SynchronizeToggles();
    }

    private void OnDisable()
    {
        if (!listenersRegistered)
            return;

        wallToggle.onValueChanged.RemoveListener(OnWallToggleChanged);
        floorToggle.onValueChanged.RemoveListener(OnFloorToggleChanged);
        equipmentToggle.onValueChanged.RemoveListener(OnEquipmentToggleChanged);
        entityToggle.onValueChanged.RemoveListener(OnEntityToggleChanged);
        layerSelectionManager.OnLayerChanged -= OnLayerChanged;
        listenersRegistered = false;
    }

    private void RegisterListeners()
    {
        if (listenersRegistered)
            return;

        wallToggle.onValueChanged.AddListener(OnWallToggleChanged);
        floorToggle.onValueChanged.AddListener(OnFloorToggleChanged);
        equipmentToggle.onValueChanged.AddListener(OnEquipmentToggleChanged);
        entityToggle.onValueChanged.AddListener(OnEntityToggleChanged);
        layerSelectionManager.OnLayerChanged += OnLayerChanged;
        listenersRegistered = true;
    }

    private void SynchronizeToggles()
    {
        wallToggle.SetIsOnWithoutNotify(layerSelectionManager.IsLayerEnabled(EditLayer.Wall));
        floorToggle.SetIsOnWithoutNotify(layerSelectionManager.IsLayerEnabled(EditLayer.Floor));
        equipmentToggle.SetIsOnWithoutNotify(layerSelectionManager.IsLayerEnabled(EditLayer.Equipment));
        entityToggle.SetIsOnWithoutNotify(layerSelectionManager.IsLayerEnabled(EditLayer.Entity));
    }

    private void OnWallToggleChanged(bool isEnabled)
        => layerSelectionManager.SetLayer(EditLayer.Wall, isEnabled);

    private void OnFloorToggleChanged(bool isEnabled)
        => layerSelectionManager.SetLayer(EditLayer.Floor, isEnabled);

    private void OnEquipmentToggleChanged(bool isEnabled)
        => layerSelectionManager.SetLayer(EditLayer.Equipment, isEnabled);

    private void OnEntityToggleChanged(bool isEnabled)
        => layerSelectionManager.SetLayer(EditLayer.Entity, isEnabled);

    private void OnLayerChanged(EditLayer layer, bool isEnabled)
    {
        switch (layer)
        {
            case EditLayer.Wall:
                wallToggle.SetIsOnWithoutNotify(isEnabled);
                break;
            case EditLayer.Floor:
                floorToggle.SetIsOnWithoutNotify(isEnabled);
                break;
            case EditLayer.Equipment:
                equipmentToggle.SetIsOnWithoutNotify(isEnabled);
                break;
            case EditLayer.Entity:
                entityToggle.SetIsOnWithoutNotify(isEnabled);
                break;
        }
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    void Awake() => Instance = this;

    [Header("Info Panels")]
    [SerializeField] private GameObject panelVisitorInfo;
    [SerializeField] private GameObject panelAnomalyInfo;
    [SerializeField] private GameObject panelExhibitInfo;

    [Header("Visitor Panel Bindings")]
    [SerializeField] private TMP_Text visitorNameText;
    [SerializeField] private UnityEngine.UI.Slider visitorSatisfactionSlider;
    [SerializeField] private TMP_Text visitorStatusText;

    [Header("Anomaly Panel Bindings")]
    [SerializeField] private TMP_Text anomalyNameText;
    [SerializeField] private TMP_Text anomalyStatusText;
    [SerializeField] private UnityEngine.UI.Slider anomalyThreatLevelSlider;

    [Header("Placed Object Panel")]
    [SerializeField] private GameObject panelPlacedObjectInfo;
    [SerializeField] private TMP_Text textPlacedObjectName;
    [SerializeField] private Image imagePlacedObjectIcon;
    [SerializeField] private Button buttonMove;
    [SerializeField] private Button buttonRemove;

    [Header("Cursor Hints")]
    [SerializeField] private GameObject suppressionCursorHint;
    [SerializeField] private GameObject removalCursorHint;

    void Start()
    {
        buttonMove.onClick.AddListener(() => SelectionManager.Instance.RequestMoveSelected());
    buttonRemove.onClick.AddListener(() => SelectionManager.Instance.RequestRemoveSelected());
    }

    // SHOW PANELS
    public void ShowVisitorPanel(IVisitorEntity visitor) { 
        HideAllInfoPanels();
        panelVisitorInfo.SetActive(true);

        visitorNameText.text = visitor.VisitorName;
    }
    public void ShowAnomalyPanel(IAnomalyEntity anomaly) { 
        HideAllInfoPanels();
        panelAnomalyInfo.SetActive(true);

        anomalyNameText.text = anomaly.AnomalyName;
        //anomalyStatusText.text = anomaly.AnomalyStatus;
        //anomalyThreatLevelSlider.value = anomaly.ThreatLevel;
    }
    public void ShowPlacedObjectPanel(PlacedObject obj)
    {
        panelPlacedObjectInfo.SetActive(true);
        textPlacedObjectName.text = obj.ItemData.ItemName;
        imagePlacedObjectIcon.sprite = obj.ItemData.Icon;
    }


    //CURSOR
    public void ShowSuppressionCursorHint() => suppressionCursorHint.SetActive(true);
    public void HideSuppressionCursorHint() => suppressionCursorHint.SetActive(false);

    public void ShowRemovalCursorHint() => removalCursorHint.SetActive(true);
    public void HideRemovalCursorHint() => removalCursorHint.SetActive(false);

    public void ShowConsumableCursorHint(ConsumableItemData itemData)
    {}
    public void HideConsumableCursorHint()
    {}

    //HIDE PANELS
    public void HideAllInfoPanels()
    {
        panelVisitorInfo.SetActive(false);
        panelAnomalyInfo.SetActive(false);
        panelExhibitInfo.SetActive(false);
    }
    public void HidePlacedObjectPanel() => panelPlacedObjectInfo.SetActive(false);
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private MainManager mainManager;
    [SerializeField] private Image inventoryPanel;
    [SerializeField] private Button cancelButton;

    void Start()
    {
        //inventoryPanel.GetComponent<Button>().onClick.AddListener(OnClickInventory);
        cancelButton.onClick.AddListener(OnClickCancel);
    }

    private void OnClickInventory()
    {
        //mainController.OnPressPlaceButton();
    }
    private void OnClickCancel()
    {
        //mainManager.OnPressCancelButton();
    }

}

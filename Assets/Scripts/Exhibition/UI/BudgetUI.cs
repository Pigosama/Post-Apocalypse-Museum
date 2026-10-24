using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BudgetUI : MonoBehaviour
{
    [SerializeField] private TMP_Text budgetText;

    void Start()
    {
        //
    }

    void OnDestroy()
    {
        //
    }

    private void UpdateBudgetText(int budget) => budgetText.text = $"Budget: ${budget}";
}
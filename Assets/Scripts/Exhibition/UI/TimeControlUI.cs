using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeControlUI : MonoBehaviour
{
    [SerializeField] private Button buttonPause;
    [SerializeField] private Button buttonNormal;
    [SerializeField] private Button buttonFast;
    [SerializeField] private Button buttonFastest;
    [SerializeField] private TMP_Text textCurrentSpeed;

    void Start()
    {
        buttonPause.onClick.AddListener(() => SimulationClock.Instance.SetSpeed(SimulationClock.Speed.Paused));
        buttonNormal.onClick.AddListener(() => SimulationClock.Instance.SetSpeed(SimulationClock.Speed.Normal));
        buttonFast.onClick.AddListener(() => SimulationClock.Instance.SetSpeed(SimulationClock.Speed.Fast));
        buttonFastest.onClick.AddListener(() => SimulationClock.Instance.SetSpeed(SimulationClock.Speed.Fastest));

        SimulationClock.Instance.OnSpeedChanged += UpdateSpeedText;
        UpdateSpeedText(SimulationClock.Instance.CurrentSpeed);
    }

    void OnDestroy()
    {
        if (SimulationClock.Instance != null)
            SimulationClock.Instance.OnSpeedChanged -= UpdateSpeedText;
    }

    private void UpdateSpeedText(SimulationClock.Speed speed) => textCurrentSpeed.text = speed.ToString();
}
using UnityEngine;
using System;

public class SimulationClock : MonoBehaviour
{
    public static SimulationClock Instance { get; private set; }
    void Awake() => Instance = this;
    
    public enum Speed { Paused, Normal, Fast, Fastest }
    public Speed CurrentSpeed { get; private set; }
    
    private float tickAccumulator;
    public float TickInterval = 0.1f; // 1シミュレーションTickの長さ

    public event Action<Speed> OnSpeedChanged;
    public event Action OnTick;

    public void SetSpeed(Speed newSpeed)
    {
        if (CurrentSpeed != newSpeed)
        {
            CurrentSpeed = newSpeed;
            OnSpeedChanged?.Invoke(newSpeed);
        }
    }

    void Update()
    {
        if (CurrentSpeed == Speed.Paused) return;

        float multiplier = CurrentSpeed switch {
            Speed.Normal => 1f, Speed.Fast => 3f, Speed.Fastest => 6f, _ => 0f
        };

        tickAccumulator += Time.deltaTime * multiplier;
        while (tickAccumulator >= TickInterval)
        {
            tickAccumulator -= TickInterval;
            OnTick?.Invoke();  // 全システムがこれを購読してロジック更新
        }
    }
}
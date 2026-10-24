using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }
    void Awake() => Instance = this;

    private readonly Dictionary<int, RoomGridState> gridStates = new();
    private readonly Dictionary<int, RoomEntityRegistry> entityRegistries = new();

    public RoomGridState GetOrCreateGridState(int roomId)
    {
        if (!gridStates.TryGetValue(roomId, out var state))
        {
            state = new RoomGridState(roomId);
            gridStates[roomId] = state;
        }
        return state;
    }

    public RoomEntityRegistry GetOrCreateEntityRegistry(int roomId)
    {
        if (!entityRegistries.TryGetValue(roomId, out var registry))
        {
            registry = new RoomEntityRegistry();
            entityRegistries[roomId] = registry;
        }
        return registry;
    }
}
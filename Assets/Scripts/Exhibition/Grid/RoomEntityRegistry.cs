// Assets/Scripts/Grid/RoomEntityRegistry.cs
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1部屋分のエンティティ一覧を保持する。占有排他はしない(重なってよい前提)ため
/// 単純なリストで十分。攻撃範囲判定などはここでXの範囲チェックのみ行う。
/// </summary>

public class RoomEntityRegistry
{
    private readonly List<EntityMover> entities = new();

    public void Register(EntityMover e) => entities.Add(e);
    public void Unregister(EntityMover e) => entities.Remove(e);

    public List<EntityMover> GetInRange(int centerX, int radius)
    {
        var result = new List<EntityMover>();
        foreach (var e in entities)
        {
            if (Mathf.Abs(e.CurrentPosition.X - centerX) <= radius)
                result.Add(e);
        }
        return result;
    }
}

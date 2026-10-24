// Assets/Scripts/Grid/RoomPosition.cs

// エンティティの座標

using System;

[Serializable]
public struct RoomPosition : IEquatable<RoomPosition>
{
    public int RoomId;
    public int X;

    public RoomPosition(int roomId, int x)
    {
        RoomId = roomId;
        X = x;
    }

    public bool Equals(RoomPosition other) => RoomId == other.RoomId && X == other.X;
    public override bool Equals(object obj) => obj is RoomPosition other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(RoomId, X);
}
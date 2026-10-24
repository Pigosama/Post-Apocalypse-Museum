public class RoomConnection
{
    public int Door { get; }
    public int Stairs { get; }
    public int Elevator { get; }

    public RoomConnection(int door, int stairs, int elevator)
    {
        Door = door;
        Stairs = stairs;
        Elevator = elevator;
    }
}
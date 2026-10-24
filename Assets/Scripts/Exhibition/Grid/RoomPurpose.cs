public class RoomPurpose
{
    public static readonly RoomPurpose Unassigned = new("Unassigned");
    public static readonly RoomPurpose Storage = new("Storage");
    public static readonly RoomPurpose Exhibition = new("Exhibition");
    public static readonly RoomPurpose Shelter = new("Shelter");

    public string Name { get; }

    private RoomPurpose(string name)
    {
        Name = name;
    }
}
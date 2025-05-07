namespace Duru.UI.Core.Models;

public class Room
{
    public Room()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        IsActive = true;
    }

    public int Id
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public int Capacity
    {
        get;
        set;
    }

    public bool IsAvailable
    {
        get;
        set;
    }

    public RoomType Type
    {
        get;
        set;
    }

    public RoomStatus Status
    {
        get;
        set;
    }

    public DateTime CreatedAt
    {
        get;
        set;
    }

    public DateTime UpdatedAt
    {
        get;
        set;
    }

    public bool IsActive
    {
        get;
        set;
    }
}

public enum RoomType
{
    Suit,
    Single,
    Double
}

public enum RoomStatus
{
    Available,
    Booked,
    Maintenance,
    OutOfOrder
}
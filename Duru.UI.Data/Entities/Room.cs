using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Duru.Library;
using Duru.UI.Data.Entities.Enums;

namespace Duru.UI.Data.Entities;

[Table("Room", Schema = "Duru")]
public class Room : CommonBase
{
    private int _roomId;
    private int _capacity;
    private int _floor;
    private RoomStatus _roomStatus;
    private RoomType _roomType;

    [Required]
    [Key]
    public int RoomId
    {
        get => _roomId;
        set
        {
            _roomId = value;
            // TODO: Güncelleme eventi eklenecek.
        }
    }
    
    [Required]
    public int Capacity
    {
        get => _capacity;
        set
        {
            _capacity = value;
            // TODO: Güncelleme eventi eklenecek.
        }
    }

    public int Floor
    {
        get => _floor;
        set
        {
            _floor = value;
            // TODO: Güncelleme eventi eklenecek.
        }
    }

    public RoomStatus RoomStatus
    {
        get => _roomStatus;
        set
        {
            _roomStatus = value;
            // TODO: Güncelleme eventi eklenecek.
        }
    }

    public RoomType RoomType
    {
        get => _roomType;
        set
        {
            _roomType = value;
            // TODO: Güncelleme eventi eklenecek.
        }
    }
}




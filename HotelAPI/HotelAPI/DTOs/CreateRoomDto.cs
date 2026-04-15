using System.ComponentModel.DataAnnotations;

namespace HotelAPI.DTOs;

public class CreateRoomDto
{
    [MaxLength(100),  Required]
    public string Name { get; set; } = string.Empty;
    public int BuildingCode {get; set;}
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public bool HasProjector { get; set; }
    public bool IsActive { get; set; }
}
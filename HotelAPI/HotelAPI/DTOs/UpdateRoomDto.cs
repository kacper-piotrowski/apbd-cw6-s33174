using System.ComponentModel.DataAnnotations;

namespace HotelAPI.DTOs;

public class UpdateRoomDto
{
    [MaxLength(100),  Required]
    public string Name { get; set; } = string.Empty;
    [Required, Range(1, int.MaxValue)]
    public int BuildingCode {get; set;}
    [Required, Range(-10, int.MaxValue)]
    public int Floor { get; set; }
    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }
    [Required]
    public bool HasProjector { get; set; }
    [Required]
    public bool IsActive { get; set; }
}
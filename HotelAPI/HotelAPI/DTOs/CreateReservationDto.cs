using System.ComponentModel.DataAnnotations;
using HotelAPI.Models;

namespace HotelAPI.DTOs;

public class CreateReservationDto
{
    [Required, Range(1, int.MaxValue)]
    public int RoomId { get; set; }
    [MaxLength(100),  Required]
    public string OrganizerName { get; set; } = string.Empty;
    [MaxLength(100),  Required]
    public string Topic {get; set;} = string.Empty;
    [Required]
    public DateTime Date {get; set;}
    [Required]
    public TimeOnly StartTime {get; set;}
    [Required]
    public TimeOnly EndTime {get; set;}
    [Required]
    public ReservationStatus Status {get; set;}
}
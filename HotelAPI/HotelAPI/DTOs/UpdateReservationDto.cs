using System.ComponentModel.DataAnnotations;
using HotelAPI.Models;

namespace HotelAPI.DTOs;

public class UpdateReservationDto
{
    public int RoomId { get; set; }
    [MaxLength(100),  Required]
    public string OrganizerName { get; set; } = string.Empty;
    [MaxLength(100),  Required]
    public string Topic {get; set;} = string.Empty;
    public DateTime Date {get; set;}
    public TimeOnly StartTime {get; set;}
    public TimeOnly EndTime {get; set;}
    public ReservationStatus Status {get; set;}
}
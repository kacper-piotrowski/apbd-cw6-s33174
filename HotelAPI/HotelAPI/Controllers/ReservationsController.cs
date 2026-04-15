using HotelAPI.DTOs;
using HotelAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        public static readonly List<Reservation> ReservationList = new List<Reservation>()
        {
            new Reservation
            {
                Id = 1,
                RoomId = 1,
                OrganizerName = "Jan Kowalski",
                Topic = "Szkolenie z bezpieczeństwa",
                Date = new DateTime(2026, 5, 10),
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(11, 0),
                Status = ReservationStatus.Confirmed
            },
            new Reservation
            {
                Id = 2,
                RoomId = 1,
                OrganizerName = "Anna Nowak",
                Topic = "Spotkanie zarządu",
                Date = new DateTime(2026, 5, 10),
                StartTime = new TimeOnly(12, 0),
                EndTime = new TimeOnly(14, 30),
                Status = ReservationStatus.Planned
            },
            new Reservation
            {
                Id = 3,
                RoomId = 2,
                OrganizerName = "Piotr Wiśniewski",
                Topic = "Warsztaty .NET",
                Date = new DateTime(2026, 5, 11),
                StartTime = new TimeOnly(10, 0),
                EndTime = new TimeOnly(16, 0),
                Status = ReservationStatus.Confirmed
            },
            new Reservation
            {
                Id = 4,
                RoomId = 3,
                OrganizerName = "Katarzyna Wójcik",
                Topic = "Prezentacja projektu",
                Date = new DateTime(2026, 5, 12),
                StartTime = new TimeOnly(13, 0),
                EndTime = new TimeOnly(15, 0),
                Status = ReservationStatus.Cancelled
            },
            new Reservation
            {
                Id = 5,
                RoomId = 4,
                OrganizerName = "Michał Kamiński",
                Topic = "Konsultacje indywidualne",
                Date = new DateTime(2026, 5, 12),
                StartTime = new TimeOnly(15, 30),
                EndTime = new TimeOnly(16, 30),
                Status = ReservationStatus.Planned
            }
        };
        
        [HttpGet]
        public IActionResult GetByParams([FromQuery] DateTime? date, [FromQuery] ReservationStatus? status, [FromQuery] int? roomId)
        {
            var accurateReservations = new List<Reservation>();
            foreach (var reservation in ReservationList)
            {
                if (
                    (date == null || reservation.Date == date)
                    && (status == null || reservation.Status == status)
                    && (roomId == null || reservation.RoomId == roomId))
                {
                    accurateReservations.Add(reservation);
                }
            }

            if (accurateReservations.Count == 0)
            {
                return NotFound();
            }
            return Ok(accurateReservations);
        }
        
        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var reservationFound = ReservationList.FirstOrDefault(x => x.Id == id);
            if (reservationFound == null)
            {
                return NotFound();
            }
            return Ok(reservationFound);
        }
        
        [HttpPost]
        public IActionResult AddReservation([FromBody] CreateReservationDto reservation)
        {
            if (reservation.StartTime >= reservation.EndTime)
            {
                return BadRequest();
            }
            var roomFound = RoomsController.RoomList.FirstOrDefault(x => x.Id == reservation.RoomId);
            if (roomFound == null)
            {
                return NotFound();
            }

            if (!roomFound.IsActive)
            {
                return BadRequest();
            }

            List<Reservation> reservationConflicts = new List<Reservation>();

            foreach (Reservation res in ReservationList)
            {
                if (reservation.RoomId == res.RoomId && reservation.Date == res.Date)
                {
                    reservationConflicts.Add(res);
                }
            }

            foreach (var conflict in reservationConflicts)
            {
                if (conflict.StartTime < reservation.EndTime && reservation.StartTime < conflict.EndTime)
                {
                    return Conflict();
                }
            }

            var createdReservation = new Reservation()
            {
                Id = ReservationList.Max(x => x.Id) + 1,
                RoomId = reservation.RoomId,
                OrganizerName = reservation.OrganizerName,
                Topic = reservation.Topic,
                Date = reservation.Date,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Status = reservation.Status
                
            };
            ReservationList.Add(createdReservation);
            return CreatedAtAction(
                nameof(GetById),new {id = createdReservation.Id},createdReservation);
        }
        
        [HttpPut("{id:int}")]
        public IActionResult UpdateReservation([FromRoute] int id, [FromBody] UpdateReservationDto reservation)
        {
            if (reservation.StartTime >= reservation.EndTime)
            {
                return BadRequest();
            }
            
            var roomFound = RoomsController.RoomList.FirstOrDefault(x => x.Id == reservation.RoomId);
            if (roomFound == null)
            {
                return NotFound();
            }

            if (!roomFound.IsActive)
            {
                return BadRequest();
            }
            
            var reservationFound = ReservationList.FirstOrDefault(x => x.Id == id);
            if (reservationFound == null)
            {
                return NotFound();
            }
            
            List<Reservation> reservationConflicts = new List<Reservation>();

            foreach (Reservation res in ReservationList)
            {
                if (reservation.RoomId == res.RoomId && reservation.Date == res.Date)
                {
                    reservationConflicts.Add(res);
                }
            }

            foreach (var conflict in reservationConflicts)
            {
                if (conflict.Id != id && reservation.RoomId == conflict.RoomId && reservation.Date == conflict.Date)
                {
                    if (conflict.StartTime < reservation.EndTime && reservation.StartTime < conflict.EndTime)
                    {
                        return Conflict();
                    }
                }
            }

            var updateReservation = new Reservation()
            {
                Id = id,
                RoomId = reservation.RoomId,
                OrganizerName = reservation.OrganizerName,
                Topic = reservation.Topic,
                Date = reservation.Date,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Status = reservation.Status
            };
            
            var index = ReservationList.IndexOf(reservationFound);
            ReservationList[index] = updateReservation;
            return Ok(updateReservation);
        }
        
        [HttpDelete("{id:int}")]
        public IActionResult DeleteReservation([FromRoute] int id)
        {
            var reservationFound = ReservationList.FirstOrDefault(x => x.Id == id);
            if (reservationFound == null)
            {
                return NotFound();
            }
            ReservationList.Remove(reservationFound);
            return NoContent();
        }
    }
}

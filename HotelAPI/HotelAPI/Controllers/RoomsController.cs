using HotelAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using HotelAPI.Models;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        public readonly static List<Room> RoomList = new List<Room>()
        {
            new Room 
            { 
                Id = 1, 
                Name = "Aula Główna", 
                BuildingCode = 101, 
                Floor = 0, 
                Capacity = 250, 
                HasProjector = true, 
                IsActive = true 
            },
            new Room 
            { 
                Id = 2, 
                Name = "Laboratorium IT", 
                BuildingCode = 102, 
                Floor = 2, 
                Capacity = 30, 
                HasProjector = true, 
                IsActive = true 
            },
            new Room 
            { 
                Id = 3, 
                Name = "Sala Ćwiczeniowa 15", 
                BuildingCode = 101, 
                Floor = 1, 
                Capacity = 45, 
                HasProjector = false, 
                IsActive = true 
            },
            new Room 
            { 
                Id = 4, 
                Name = "Pokój Spotkań B", 
                BuildingCode = 105, 
                Floor = 4, 
                Capacity = 8, 
                HasProjector = true, 
                IsActive = true 
            },
            new Room 
            { 
                Id = 5, 
                Name = "Stary Magazyn", 
                BuildingCode = 102, 
                Floor = -1, 
                Capacity = 0, 
                HasProjector = false, 
                IsActive = false 
            }
        };

        [HttpGet]
        public IActionResult GetByParams([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
        {
            var accurateRooms = new List<Room>();
            foreach (var room in RoomList)
            {
                if (
                    (minCapacity == null || room.Capacity >= minCapacity)
                    && (hasProjector == null || room.HasProjector == hasProjector)
                    && (activeOnly == null || room.IsActive == activeOnly))
                {
                    accurateRooms.Add(room);
                }
            }

            if (accurateRooms.Count == 0)
            {
                return NotFound();
            }
            return Ok(accurateRooms);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var roomFound = RoomList.FirstOrDefault(x => x.Id == id);
            if (roomFound == null)
            {
                return NotFound();
            }
            return Ok(roomFound);
        }

        [HttpGet("building/{buildingCode:int}")]
        public IActionResult GetByBuildingCode([FromRoute] int buildingCode)
        {
            var buildingCodeRooms = new List<Room>();
            foreach (var room in RoomList)
            {
                if (room.BuildingCode == buildingCode)
                {
                    buildingCodeRooms.Add(room);
                }
            }
            if (buildingCodeRooms.Count == 0)
            {
                return NotFound();
            }
            return Ok(buildingCodeRooms);
        }

        [HttpPost]
        public IActionResult AddRoom([FromBody] CreateRoomDto room)
        {
            var createdRoom = new Room()
            {
                Id = RoomList.Max(x => x.Id) + 1,
                Name = room.Name,
                BuildingCode = room.BuildingCode,
                Floor = room.Floor,
                Capacity = room.Capacity,
                HasProjector = room.HasProjector,
                IsActive = room.IsActive
            };
            RoomList.Add(createdRoom);
            return CreatedAtAction(
                nameof(GetById),new {id = createdRoom.Id},createdRoom);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateRoom([FromRoute] int id, [FromBody] UpdateRoomDto room)
        {
            var updatedRoom = new Room()
            {
                Id = id,
                Name = room.Name,
                BuildingCode = room.BuildingCode,
                Floor = room.Floor,
                Capacity = room.Capacity,
                HasProjector = room.HasProjector,
                IsActive = room.IsActive
            };
            
            var roomFound = RoomList.FirstOrDefault(x => x.Id == id);
            if (roomFound == null)
            {
                return NotFound();
            }
            var index = RoomList.IndexOf(roomFound);
            RoomList[index] = updatedRoom;
            return Ok(updatedRoom);

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteRoom([FromRoute] int id)
        {
            var roomFound = RoomList.FirstOrDefault(x => x.Id == id);
            if (roomFound == null)
            {
                return NotFound();
            }
            RoomList.Remove(roomFound);
            return NoContent();
        }


    }
}

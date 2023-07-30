using Abstractions;
using AutoMapper;
using Entities;
using Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace library_management_system_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReservationManager _reservationManager;
        private readonly IBookManager _bookManager;
        private readonly IUserAuthManager _userManager;

        public ReservationsController(IMapper mapper, IReservationManager reservationManager, IBookManager bookManager, IUserAuthManager userManager)
        {
            _mapper = mapper;
            _reservationManager = reservationManager;
            _bookManager = bookManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var reservations = await _reservationManager.GetAll();
            if(!reservations.Any())
                return NotFound($"No Reservations are found");

            var data = _mapper.Map<IEnumerable<ReturnedReservationDTO>>(reservations);

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var reservation = await _reservationManager.Get(id);


            var dto = _mapper.Map<ReturnedReservationDTO>(reservation);
            
            return Ok(dto);
        }

        [HttpGet("GetByUserId/{appUserId}")]
        public async Task<IActionResult> GetByUserIdAsync(string appUserId)
        {
            var reservations = await _reservationManager.GetByUserId(appUserId);
            if (!reservations.Any())
                return NotFound($"No Reservations are found");

            var data = _mapper.Map<IEnumerable<ReturnedReservationDTO>>(reservations);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RecievedReservationDTO dto)
        {
            //Check if book id exists
            if (!await _bookManager.IsValidBookIdAsync(dto.BookId))
                return BadRequest("Invalid book Id");

            //Check if user id exists
            if(!await _userManager.UserExists(dto.AppUserId))
                return BadRequest("Invalid user Id");

            var reservation = _mapper.Map<Reservation>(dto);
            await _reservationManager.Add(reservation);
            return Ok(reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] RecievedReservationDTO dto)
        {
            var reservation = await _reservationManager.Get(id);
            if (reservation == null)
                return NotFound($"No reservation with id : {id} was found");

            //Check if book id exists
            if (!await _bookManager.IsValidBookIdAsync(dto.BookId))
                return BadRequest("Invalid book Id");

            //Check if user id exists
            if (!await _userManager.UserExists(dto.AppUserId))
                return BadRequest("Invalid user Id");

            reservation.StartDate = dto.StartDate;
            reservation.IsReturned = dto.IsReturned;
            reservation.BookId = dto.BookId;
            reservation.AppUserId = dto.AppUserId;

            _reservationManager.Update(reservation);
            return Ok(reservation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var reservation = await _reservationManager.Get(id);
            if (reservation == null)
                return NotFound($"No reservation with id : {id} was found");

            _reservationManager.Delete(reservation);
            return Ok(reservation);
        }

    }
}

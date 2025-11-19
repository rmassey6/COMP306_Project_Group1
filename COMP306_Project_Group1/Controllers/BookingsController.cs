using AutoMapper;
using COMP306_Project_Group1.DTOs;
using COMP306_Project_Group1.Services;
using FlightLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMP306_Project_Group1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingsController(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
        {
            var bookings = await _bookingRepository.GetBookingsAsync();
            var results = _mapper.Map<IEnumerable<BookingDto>>(bookings);
            return Ok(results);
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBooking(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookingDto>(booking));
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, [FromBody] BookingForUpdateDto booking)
        {
            if (booking == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Booking oldBookingEntity = await _bookingRepository.GetBookingByIdAsync(id);

            if (oldBookingEntity == null) return NotFound();

            _mapper.Map(booking, oldBookingEntity);

            if (!await _bookingRepository.SaveAsync())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookingDto>> PostBooking([FromBody] BookingForCreationDto booking)
        {
            if (booking == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            var finalBooking = _mapper.Map<Booking>(booking);

            _bookingRepository.AddBooking(finalBooking);
            if (!await _bookingRepository.SaveAsync())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var bookingToReturn = _mapper.Map<BookingDto>(finalBooking);

            return CreatedAtAction("GetBooking", new { id = bookingToReturn.Id }, bookingToReturn);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (!await _bookingRepository.BookingExistsAsync(id)) return NotFound();

            Booking bookingEntity2Delete = await _bookingRepository.GetBookingByIdAsync(id);
            if (bookingEntity2Delete == null) return BadRequest();

            _bookingRepository.DeleteBooking(bookingEntity2Delete);

            if (!await _bookingRepository.SaveAsync())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        // PATCH: api/Bookings/5
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdatedBooking(int id, JsonPatchDocument<BookingForUpdateDto> patchDocument)
        {
            if (!await _bookingRepository.BookingExistsAsync(id))
            {
                return NotFound();
            }

            var bookingEntity = await _bookingRepository.GetBookingByIdAsync(id);
            if (bookingEntity == null)
            {
                return NotFound();
            }

            var bookingToPatch = _mapper.Map<BookingForUpdateDto>(bookingEntity);

            patchDocument.ApplyTo(bookingToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(bookingToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(bookingToPatch, bookingEntity);
            await _bookingRepository.SaveAsync();

            return NoContent();
        }
    }
}

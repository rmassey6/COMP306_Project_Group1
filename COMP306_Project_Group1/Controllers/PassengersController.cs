using AutoMapper;
using COMP306_Project_Group1.DTOs;
using COMP306_Project_Group1.Services;
using FlightLibrary.Models;
using Microsoft.AspNetCore.Http;
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
    public class PassengersController : ControllerBase
    {
        private IPassengerRepository _passengerRepository;
        private readonly IMapper _mapper;

        public PassengersController(IPassengerRepository passengerRepository, IMapper mapper)
        {
            _passengerRepository = passengerRepository;
            _mapper = mapper;
        }

        // GET: api/Passengers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PassengerDto>>> GetPassengers()
        {
            var passengers = await _passengerRepository.GetPassengersAsync();
            var results = _mapper.Map<IEnumerable<PassengerDto>>(passengers);
            return Ok(results);
        }

        // GET: api/Passengers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PassengerDto>> GetPassenger(int id)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id);

            if (passenger == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PassengerDto>(passenger));
        }

        // PUT: api/Passengers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPassenger(int id, [FromBody] PassengerForUpdateDto passenger)
        {
            if (passenger == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Passenger oldPassengerEntity = await _passengerRepository.GetPassengerByIdAsync(id);

            if (oldPassengerEntity == null) return NotFound();

            _mapper.Map(passenger, oldPassengerEntity);

            if (!await _passengerRepository.SaveAsync())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        // POST: api/Passengers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PassengerDto>> PostPassenger([FromBody] PassengerForCreationDto passenger)
        {
            if (passenger == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            var finalPassenger = _mapper.Map<Passenger>(passenger);

            _passengerRepository.AddPassenger(finalPassenger);
            if (!await _passengerRepository.SaveAsync())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var passengerToReturn = _mapper.Map<PassengerDto>(finalPassenger);

            return CreatedAtAction("GetPassenger", new { id = passengerToReturn.Id }, passengerToReturn);
        }

        // DELETE: api/Passengers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassenger(int id)
        {
            if (!await _passengerRepository.PassengerExistsAsync(id)) return NotFound();

            Passenger passengerEntity2Delete = await _passengerRepository.GetPassengerByIdAsync(id);
            if (passengerEntity2Delete == null) return BadRequest();

            _passengerRepository.DeletePassenger(passengerEntity2Delete);

            if (!await _passengerRepository.SaveAsync())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightLibrary.Models;
using COMP306_Project_Group1.Services;
using AutoMapper;
using COMP306_Project_Group1.DTOs;
using Azure;
using Microsoft.AspNetCore.JsonPatch;

namespace COMP306_Project_Group1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public FlightsController(IFlightRepository flightRepository, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightDto>>> GetFlights()
        {
            var flights = await _flightRepository.GetFlightsAsync();
            var results = _mapper.Map<IEnumerable<FlightDto>>(flights);
            return Ok(results);
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightDto>> GetFlight(int id)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FlightDto>(flight));
        }

        // PUT: api/Flights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight(int id, [FromBody] FlightForUpdateDto flight)
        {
            if (flight == null) return BadRequest();

            if (flight.ArrivalTime < DateTime.Now || flight.DepartureTime < DateTime.Now)
            {
                ModelState.AddModelError("Arrival time and departure time", "The provided arrival time or departure time is in the past.");
            }

            if (flight.ArrivalTime < flight.DepartureTime)
            {
                ModelState.AddModelError("Arrival time", "The provided arrival time is before the departure time.");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Flight oldFlightEntity = await _flightRepository.GetFlightByIdAsync(id);

            if (oldFlightEntity == null) return NotFound();

            _mapper.Map(flight, oldFlightEntity);

            if (!await _flightRepository.SaveAsync())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        // POST: api/Flights
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FlightDto>> PostFlight([FromBody] FlightForCreationDto flight)
        {
            if (flight == null) return BadRequest();

            if (flight.ArrivalTime < DateTime.Now || flight.DepartureTime < DateTime.Now)
            {
                ModelState.AddModelError("Arrival time and departure time", "The provided arrival time or departure time is in the past.");
            }

            if (flight.ArrivalTime < flight.DepartureTime)
            {
                ModelState.AddModelError("Arrival time", "The provided arrival time is before the departure time.");
            }

            if (!ModelState.IsValid) return BadRequest();

            var finalFlight = _mapper.Map<Flight>(flight);

            _flightRepository.AddFlight(finalFlight);
            if (!await _flightRepository.SaveAsync())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var flightToReturn = _mapper.Map<FlightDto>(finalFlight);

            return CreatedAtAction("GetFlight", new { id = flightToReturn.Id }, flightToReturn);
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            if (!await _flightRepository.FlightExistsAsync(id)) return NotFound();

            Flight flightEntity2Delete = await _flightRepository.GetFlightByIdAsync(id);
            if (flightEntity2Delete == null) return BadRequest();   

            _flightRepository.DeleteFlight(flightEntity2Delete);

            if (!await _flightRepository.SaveAsync())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        // PATCH: api/Flights/5
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdatedFlight(int id, JsonPatchDocument<FlightForUpdateDto> patchDocument)
        {
            if (!await _flightRepository.FlightExistsAsync(id))
            {
                return NotFound();
            }

            var flightEntity = await _flightRepository.GetFlightByIdAsync(id);
            if (flightEntity == null)
            {
                return NotFound();
            }

            var flightToPatch = _mapper.Map<FlightForUpdateDto>(flightEntity);

            patchDocument.ApplyTo(flightToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(flightToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(flightToPatch, flightEntity);
            await _flightRepository.SaveAsync();

            return NoContent();
        }
    }
}

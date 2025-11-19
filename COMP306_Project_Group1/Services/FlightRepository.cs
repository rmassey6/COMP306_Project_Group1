using FlightLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP306_Project_Group1.Services
{
    public class FlightRepository : IFlightRepository
    {
        private FlightdbContext _context;
        public FlightRepository(FlightdbContext context)
        {
            _context = context;
        }

        public async Task<bool> FlightExistsAsync(int id)
        {
            return await _context.Flights.AnyAsync<Flight>(f => f.Id == id);
        }

        public async Task<IEnumerable<Flight>> GetFlightsAsync()
        {
            var result = _context.Flights.OrderBy(f => f.Id);
            return await result.ToListAsync();
        }

        public async Task<Flight> GetFlightByIdAsync(int id)
        {
            IQueryable<Flight> result;
            result = _context.Flights.Where(f => f.Id == id);

            return await result.FirstOrDefaultAsync();
        }

        public void AddFlight(Flight flight)
        {
            _context.Flights.Add(flight);
        }

        public void DeleteFlight(Flight flight)
        {
            _context.Flights.Remove(flight);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}

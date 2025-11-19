using FlightLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP306_Project_Group1.Services
{
    public class PassengerRepository : IPassengerRepository
    {
        private FlightdbContext _context;
        public PassengerRepository(FlightdbContext context)
        {
            _context = context;
        }

        public async Task<bool> PassengerExistsAsync(int id)
        {
            return await _context.Passengers.AnyAsync<Passenger>(p => p.Id == id);
        }

        public async Task<IEnumerable<Passenger>> GetPassengersAsync()
        {
            var result = _context.Passengers.OrderBy(p => p.Id);
            return await result.ToListAsync();
        }

        public async Task<Passenger> GetPassengerByIdAsync(int id)
        {
            IQueryable<Passenger> result;
            result = _context.Passengers.Where(p => p.Id == id);

            return await result.FirstOrDefaultAsync();
        }

        public void AddPassenger(Passenger passenger)
        {
            _context.Passengers.Add(passenger);
        }

        public void DeletePassenger(Passenger passenger)
        {
            _context.Passengers.Remove(passenger);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}

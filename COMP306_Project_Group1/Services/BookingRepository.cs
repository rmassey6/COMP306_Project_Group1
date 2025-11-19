using FlightLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP306_Project_Group1.Services
{
    public class BookingRepository : IBookingRepository
    {
        private FlightdbContext _context;
        public BookingRepository(FlightdbContext context)
        {
            _context = context;
        }

        public async Task<bool> BookingExistsAsync(int id)
        {
            return await _context.Bookings.AnyAsync<Booking>(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            var result = _context.Bookings.OrderBy(b => b.Id);
            return await result.ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            IQueryable<Booking> result;
            result = _context.Bookings.Where(b => b.Id == id);

            return await result.FirstOrDefaultAsync();
        }

        public void AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
        }

        public void DeleteBooking(Booking booking)
        {
            _context.Bookings.Remove(booking);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}

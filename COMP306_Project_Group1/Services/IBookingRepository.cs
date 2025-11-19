using FlightLibrary.Models;

namespace COMP306_Project_Group1.Services
{
    public interface IBookingRepository
    {
        Task<bool> BookingExistsAsync(int id);
        Task<IEnumerable<Booking>> GetBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        void AddBooking(Booking booking);
        void DeleteBooking(Booking booking);
        Task<bool> SaveAsync();
    }
}

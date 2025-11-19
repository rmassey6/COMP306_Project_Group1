using FlightLibrary.Models;

namespace COMP306_Project_Group1.Services
{
    public interface IPassengerRepository
    {
        Task<bool> PassengerExistsAsync(int id);
        Task<IEnumerable<Passenger>> GetPassengersAsync();
        Task<Passenger> GetPassengerByIdAsync(int id);
        void AddPassenger(Passenger passenger);
        void DeletePassenger(Passenger passenger);
        Task<bool> SaveAsync();
    }
}

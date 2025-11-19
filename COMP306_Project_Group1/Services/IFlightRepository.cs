using FlightLibrary.Models;

namespace COMP306_Project_Group1.Services
{
    public interface IFlightRepository
    {
        Task<bool> FlightExistsAsync(int id);
        Task<IEnumerable<Flight>> GetFlightsAsync();
        Task<Flight> GetFlightByIdAsync(int id);
        void AddFlight(Flight flight);
        void DeleteFlight(Flight flight);
        Task<bool> SaveAsync();
    }
}

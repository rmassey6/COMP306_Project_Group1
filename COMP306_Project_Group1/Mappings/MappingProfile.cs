using AutoMapper;
using COMP306_Project_Group1.DTOs;
using FlightLibrary.Models;

namespace COMP306_Project_Group1.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Flight, FlightDto>();
            CreateMap<FlightForUpdateDto, Flight>();
            CreateMap<FlightForCreationDto, Flight>();

            CreateMap<Passenger, PassengerDto>();
            CreateMap<PassengerForUpdateDto, Passenger>();
            CreateMap<PassengerForCreationDto, Passenger>();

            CreateMap<Booking, BookingDto>();
            CreateMap<BookingForUpdateDto, Booking>();
            CreateMap<BookingForCreationDto, Booking>();
        }
    }
}

using AutoMapper;
using Entities;
using Entities.AuthModels;
using Entities.DTO;

namespace Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RecievedBookDTO, Book>();
            CreateMap<RecievedReservationDTO, Reservation>();
            CreateMap<Reservation, ReturnedReservationDTO>();

            CreateMap<RegisterModel, AppUser>();
        }
    }
}

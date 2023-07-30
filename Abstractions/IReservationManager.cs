using Entities;

namespace Abstractions
{
    public interface IReservationManager
    {
        Task<Reservation> Get(int id);
        Task<IEnumerable<Reservation>> GetAll();
        Task<IEnumerable<Reservation>> GetByUserId(string appUserId);
        Task<Reservation> Add(Reservation reservation);
        Reservation Update(Reservation reservation);
        Reservation Delete(Reservation reservation);
    }
}

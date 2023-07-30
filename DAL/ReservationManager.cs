using Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ReservationManager : IReservationManager
    {
        private readonly LibraryDbContext _context;

        public ReservationManager(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> Get(int id)
        {
            return await _context.Reservations.SingleOrDefaultAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<Reservation>> GetAll()
        {
            return await _context.Reservations.
                OrderBy(r => r.Id).
                ToListAsync();
        }
        public async Task<IEnumerable<Reservation>> GetByUserId(string appUserId)
        {
            return await _context.Reservations.
                Where(r => r.AppUserId == appUserId).
                OrderBy(r => r.Id).
                Include(r => r.AppUser).
                ToListAsync();
        }
        public async Task<Reservation> Add(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            _context.SaveChanges();
            return reservation;
        }
        public Reservation Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            _context.SaveChanges();
            return reservation;
        }
        public Reservation Delete(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
            return reservation;
        }
    }
}

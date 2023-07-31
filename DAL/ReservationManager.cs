using Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL
{
    public class ReservationManager : IReservationManager
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<ReservationManager> _logger;

        public ReservationManager(LibraryDbContext context, ILogger<ReservationManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Reservation> Get(int id)
        {
            try
            {
                return await _context.Reservations.SingleOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }
        public async Task<IEnumerable<Reservation>> GetAll()
        {
            try
            {
                return await _context.Reservations.
                OrderBy(r => r.Id).
                Include(r => r.AppUser).
                Include(r => r.Book).
                ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }
        public async Task<IEnumerable<Reservation>> GetByUserId(string appUserId)
        {
            try
            {
                return await _context.Reservations.
                Where(r => r.AppUserId == appUserId).
                OrderBy(r => r.Id).
                Include(r => r.AppUser).
                ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }
        public async Task<Reservation> Add(Reservation reservation)
        {
            try
            {
                await _context.Reservations.AddAsync(reservation);
                _context.SaveChanges();
                return reservation;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }
        public Reservation Update(Reservation reservation)
        {
            try
            {
                _context.Reservations.Update(reservation);
                _context.SaveChanges();
                return reservation;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }
        public Reservation Delete(Reservation reservation)
        {
            try
            {

                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
                return reservation;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }

    }
}

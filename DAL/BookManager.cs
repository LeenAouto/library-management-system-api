using Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL
{
    public class BookManager : IBookManager
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<BookManager> _logger;

        public BookManager(LibraryDbContext context, ILogger<BookManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Book> Get(int id)
        {
            try
            {
                return await _context.Books.SingleOrDefaultAsync(b => b.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            try
            {

                return await _context.Books
                    .OrderBy(b => b.Title)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }
        public async Task<Book> Add(Book book)
        {
            try
            {
                await _context.Books.AddAsync(book);
                _context.SaveChanges();
                return book;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }        
        }
        public Book Update(Book book)
        {
            try
            {
                _context.Books.Update(book);
                _context.SaveChanges();
                return book;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }
        public Book Delete(Book book)
        {
            try
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                return book;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }
        public async Task<bool> IsValidBookIdAsync(int id)
        {
            try
            {
                return await _context.Books.AnyAsync(g => g.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                throw;
            }
        }

    }
}

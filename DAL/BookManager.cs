using Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class BookManager : IBookManager
    {
        private readonly LibraryDbContext _context;

        public BookManager(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Book> Get(int id)
        {
            return await _context.Books.SingleOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _context.Books
                .OrderBy(b => b.Title)
                .ToListAsync();
        }
        public async Task<Book> Add(Book book)
        {
            await _context.Books.AddAsync(book);
            _context.SaveChanges();
            return book;
        }
        public Book Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
            return book;
        }
        public Book Delete(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
            return book;
        }
        public async Task<bool> IsValidBookIdAsync(int id)
        {
            return await _context.Books.AnyAsync(g => g.Id == id);
        }

    }
}

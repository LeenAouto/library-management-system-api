using Entities;

namespace Abstractions
{
    public interface IBookManager
    {
        Task<Book> Get(int id);
        Task<IEnumerable<Book>> GetAll();
        Task<Book> Add(Book book);
        Book Update(Book book);
        Book Delete(Book book);
        Task<bool> IsValidBookIdAsync(int bookId);
    }
}

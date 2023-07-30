using Abstractions;
using AutoMapper;
using Entities;
using Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace library_management_system_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookManager _bookManager;

        public BooksController(IBookManager bookManager, IMapper mapper)
        {
            _mapper = mapper;
            _bookManager = bookManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            //Console.WriteLine("Testrttt");
            var books = await _bookManager.GetAll();
            if (!books.Any())
                return NotFound($"No books are found");

            //Console.WriteLine("Test");
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var book = await _bookManager.Get(id);
            if (book == null)
                return NotFound($"No book with id : {id} was found.");
            
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RecievedBookDTO dto)
        {
            var book = _mapper.Map<Book>(dto);
            await _bookManager.Add(book);
            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] RecievedBookDTO dto)
        {
            var book = await _bookManager.Get(id);
            if (book == null)
                return NotFound($"No book with id : {id} was found.");

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Publisher = dto.Publisher;
            book.PublishYear = dto.PublishYear;
            book.IsAvailable = dto.IsAvailable;

            _bookManager.Update(book);

            return Ok(book);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var book = await _bookManager.Get(id);
            if (book == null)
                return NotFound($"No book with id : {id} was found.");

            _bookManager.Delete(book);

            return Ok(book);
        }
    }
}

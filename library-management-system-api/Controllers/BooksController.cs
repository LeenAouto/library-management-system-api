using Abstractions;
using AutoMapper;
using Entities;
using Entities.DTO;
using Microsoft.AspNetCore.Mvc;

namespace library_management_system_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookManager _bookManager;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookManager bookManager, IMapper mapper, ILogger<BooksController> logger)
        {
            _mapper = mapper;
            _bookManager = bookManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var books = await _bookManager.GetAll();
                if (!books.Any())
                    return NotFound($"No books are found");

                return Ok(books);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var book = await _bookManager.Get(id);
                if (book == null)
                    return NotFound($"No book with id : {id} was found.");

                return Ok(book);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RecievedBookDTO dto)
        {
            try
            {
                var book = _mapper.Map<Book>(dto);
                await _bookManager.Add(book);
                return Ok(book);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] RecievedBookDTO dto)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var book = await _bookManager.Get(id);
                if (book == null)
                    return NotFound($"No book with id : {id} was found.");

                _bookManager.Delete(book);

                return Ok(book);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(e.Message);
            }
        }
    }
}

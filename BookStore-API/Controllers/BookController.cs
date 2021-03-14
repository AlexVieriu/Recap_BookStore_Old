using AutoMapper;
using BookStore_API.Data;
using BookStore_API.DTOs;
using BookStore_API.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BookController(IBookRepository bookRepository,
                              ILoggerService logger,
                              IMapper mapper)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                _logger.LogInfo("Atemtting to get all books");
                var books = await _bookRepository.FindAll();

                if (books is null || books.Count == 0)
                {
                    _logger.LogInfo("There are no books in the DataBase");
                    return NoContent();
                }

                _logger.LogInfo("Books are Displayed");
                return Ok(books);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                _logger.LogInfo($"Attempting to get the book with the id : {id}");
                if (id < 1)
                {
                    _logger.LogWarn($"Attempting to get the book with id:{id} failed");
                    return BadRequest(id);
                }

                var book = await _bookRepository.FindById(id);
                if (book is null)
                {
                    _logger.LogInfo($"We can't find the book with the id:{id}"); ;
                    return NotFound();
                }

                _logger.LogInfo($"We got the book with the id:{id}");
                return Ok(book);

            }
            catch (Exception e)
            {

                return InternalError(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookCreateDTO)
        {
            try
            {
                if (bookCreateDTO is null)
                {
                    _logger.LogWarn("The book object is empty");
                    return BadRequest();
                }

                var book = _mapper.Map<Book>(bookCreateDTO);

                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("The ModelState is not valid");
                    return BadRequest();
                }

                var isSuccess = await _bookRepository.Create(book);

                if (!isSuccess)
                {
                    _logger.LogInfo("The book cound't be created");
                    return NoContent();
                }

                return Ok();
            }

            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO bookUpdateDTO)
        {

            try
            {
                if (id < 0 || bookUpdateDTO is null)
                {
                    return BadRequest();
                }

                var author = _mapper.Map<Book>(bookUpdateDTO);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var isExist = await _bookRepository.Exists(id);
                if (!isExist)
                {
                    _logger.LogInfo($"The book with the id:{id} was not found");
                    return NotFound();
                }

                var isSuccess = await _bookRepository.Update(author);
                if (!isSuccess)
                {
                    _logger.LogInfo($"The book with the id:{id} could not be updated");
                    return BadRequest();
                }

                return Ok(author);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1)
                {
                    _logger.LogInfo($"The id:{id} is not valid");
                    return BadRequest();
                }

                var author = await _bookRepository.FindById(id);
                if (author is null)
                {
                    _logger.LogInfo($"Coudn't found the book with the id:{id}");
                    return NotFound();
                }

                var isSuccess = await _bookRepository.Delete(author);

                if (!isSuccess)
                {
                    _logger.LogInfo($"Cound't delete the book with the id:{id}");
                    return BadRequest();
                }

                _logger.LogInfo($"Book with the id:{id} was deleted");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        private ObjectResult InternalError(Exception e)
        {
            _logger.LogError($"{e.Message} - {e.InnerException}");
            return StatusCode(500, "Something wenr wront, contact the Administrator");
        }
    }
}

using AutoMapper;
using BookStore_API.Data;
using BookStore_API.DTOs;
using BookStore_API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Interacts with the Books Table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public BooksController(IBookRepository bookRepository,
                               ILoggerService logger,
                               IMapper mapper,
                               IWebHostEnvironment environment)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper;
            _environment = environment;
        }

        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns>A List of Books</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBooks()
        {
            var location = GetControllerActionNames();

            try
            {
                _logger.LogInfo($"{location}: Attempted Call");
                var books = await _bookRepository.FindAll();
                var booksDTO = _mapper.Map<IList<BookDTO>>(books);

                foreach (var item in booksDTO)
                {
                    item.File = GetFileImg(item.Image);
                }

                _logger.LogInfo($"{location} : Succefull");
                return Ok(booksDTO);
            }
            catch (Exception e)
            {
                return InternalError(e, location);
            }
        }

        /// <summary>
        /// Gets a Book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Book record</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookById(int id)
        {
            var location = GetControllerActionNames();

            try
            {
                _logger.LogInfo($"{location}: Attempted to call id : {id}");

                var book = await _bookRepository.FindById(id);
                if (book is null)
                {
                    _logger.LogInfo($"{location}: Failed to retrive the record with id:{id}"); ;
                    return NotFound();
                }

                var bookDTO = _mapper.Map<BookDTO>(book);

                bookDTO.File = GetFileImg(bookDTO.Image);

                _logger.LogInfo($"{location}: Successfully got record with id:{id}");
                return Ok(bookDTO);
            }
            catch (Exception e)
            {

                return InternalError(e, location);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookCreateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Create Attempted");

                if (bookCreateDTO is null)
                {
                    _logger.LogWarn($"{location}: Empty request was submited");
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogInfo($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }

                var book = _mapper.Map<Book>(bookCreateDTO);
                var isSuccess = await _bookRepository.Create(book);

                if (!isSuccess)
                {
                    return StatusCode(500, "Created failed");
                }

                StoreFileImg(bookCreateDTO.File, bookCreateDTO.Image, "");

                _logger.LogInfo($"{ location}: Successfully created");
                return Created("Create", new { bookCreateDTO });
            }

            catch (Exception e)
            {
                return InternalError(e, location);
            }
        }

        /// <summary>
        /// Update a Book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO bookUpdateDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location} : Update Attempted on record with id: {id}");
                if (id < 0 || bookUpdateDTO is null || id != bookUpdateDTO.Id)
                {
                    _logger.LogWarn($"{location}: Updated failed with id: {id}");
                    return BadRequest();
                }

                var isExists = await _bookRepository.Exists(id);

                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Failed to find record with id: {id}");
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }

                var oldImage = await _bookRepository.GetImageFileName(id);
                var book = _mapper.Map<Book>(bookUpdateDTO);
                var isSuccess = await _bookRepository.Update(book);

                if (!isSuccess)
                {
                    return StatusCode(500, $"{location}: Failed Update with id:{id}");
                }

                StoreFileImg(bookUpdateDTO.File, bookUpdateDTO.Image, oldImage);

                _logger.LogInfo($"{location}: Update successful with id: {id}");
                return Ok(book);
            }
            catch (Exception e)
            {
                return InternalError(e, location);
            }
        }

        /// <summary>
        /// Removes an book by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Delete Attempted with id : {id}");
                if (id < 1)
                {
                    _logger.LogWarn($"Delete failed with bad data at with id: {id}");
                    return BadRequest();
                }

                var isExists = await _bookRepository.Exists(id);

                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Record with id :{id} does not exists");
                    return NotFound();
                }

                var author = await _bookRepository.FindById(id);
                var isSuccess = await _bookRepository.Delete(author);

                if (!isSuccess)
                {
                    return StatusCode(500, $"{location}: Delete failed for record with id:{id}");
                }

                _logger.LogInfo($"{location}: Record with the id:{id} was deleted");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError(e, location);
            }
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(Exception e, string location)
        {
            _logger.LogError($"{location}: {e.Message} - {e.InnerException}");
            return StatusCode(500, "Something went wront, contact the Administrator");
        }

        private string GetFileImg(string image)
        {
            if (!string.IsNullOrWhiteSpace(image))
            {
                var imgPath = $"{_environment.ContentRootPath}\\images\\{image}";
                if (System.IO.File.Exists(imgPath))
                {
                    byte[] imgBytes = System.IO.File.ReadAllBytes(imgPath);
                    var file = Convert.ToBase64String(imgBytes);
                    return file;
                }
            }

            return string.Empty;
        }

        private void StoreFileImg(string file, string image, string oldImage)
        {
            if (image.Equals(oldImage))
            {
                if (System.IO.File.Exists($"{_environment.ContentRootPath}\\images\\{oldImage}"))
                {
                    System.IO.File.Delete($"{_environment.ContentRootPath}\\images\\{oldImage}");
                }
            }

            if (!string.IsNullOrWhiteSpace(file))
            {
                var imgPath = $"{_environment.ContentRootPath}\\images\\{image}";
                byte[] imageBytes = Convert.FromBase64String(file);
                System.IO.File.WriteAllBytes(imgPath, imageBytes);
            }
        }
    }
}

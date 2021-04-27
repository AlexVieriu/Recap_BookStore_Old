using AutoMapper;
using BookStore_API.Data;
using BookStore_API.DTOs;
using BookStore_API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Enpoint used to interact with the Authors in the book store's database
    /// </summary>    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository,
                                 ILoggerService logger,
                                 IMapper mapper)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get All Authors
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet]  
        [Authorize(Roles = "Administrator, Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            var location = GetControllerActionname();

            try
            {
                _logger.LogInfo($"{location} : Attempted to get all records");

                var authors = await _authorRepository.FindAll();
                var authorsDTO = _mapper.Map<IList<AuthorDTO>>(authors);

                _logger.LogInfo($"{location}: Successfully got all records");
                return Ok(authorsDTO);
            }
            catch (Exception e)
            {
                return InternalError(e, location);
            }
        }


        /// <summary>
        /// Get Author by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Author's record</returns>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Administrator,Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var location = GetControllerActionname();
            try
            {
                _logger.LogInfo($"Attempted Get Record with id: {id}");               

                var isExist = await _authorRepository.Exists(id);

                if(!isExist)
                {
                    _logger.LogWarn($"{location}: Record with id :{id} was not found");
                    return BadRequest();
                }

                var author = await _authorRepository.FindById(id);
                var authorDTO = _mapper.Map<AuthorDTO>(author);

                _logger.LogInfo("Successfully the Author");

                return Ok(authorDTO);
            }
            catch (Exception e)
            {
                return InternalError(e, location);
            }
        }


        /// <summary>
        /// Creates An Author
        /// </summary>
        /// <param name="authorCreateDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorCreateDTO)
        {
            var location = GetControllerActionname();
            try
            {
                _logger.LogInfo($"{location}: Attempting to create record");
                if (authorCreateDTO is null)
                {
                    _logger.LogWarn($"{location}: Empty request was submited");
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Records ModelState si not valid");
                    return BadRequest(ModelState);
                }

                var author = _mapper.Map<Author>(authorCreateDTO);
                var isSuccess = await _authorRepository.Create(author);

                if (isSuccess == false)
                {
                    _logger.LogWarn($"{location}: Record created failed");
                    return StatusCode(500, "The record was not created");
                }

                _logger.LogInfo($"{location}: Record Created");
                return Created("Create", new { author });
            }
            catch (Exception e)
            {
                return InternalError(e, location);
            }
        }


        /// <summary>
        /// Update an Author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorUpdateDTO"></param>
        /// <returns>Returns the Author Updated</returns>
        [HttpPut("{id:int}")]
        [Authorize(Roles ="Administrator")]
        //[AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorUpdateDTO authorUpdateDTO)
        {
            var location = GetControllerActionname();
            try
            {
                _logger.LogInfo($"{location}: Attempting to update record with id : {id}");

                if (authorUpdateDTO is null || id < 1 || id != authorUpdateDTO.Id)
                {
                    _logger.LogInfo($"{location}: Update failed with bad data - id: {id}");
                    return BadRequest();
                }

                var isExists = await _authorRepository.Exists(id);

                if (!isExists)
                {
                    _logger.LogWarn($"{location}: The record with id:{id} doesn't exists");
                    return NotFound();
                }

                if (ModelState.IsValid == false)
                {
                    _logger.LogWarn($"{location}: The ModelState record with id: {id} is not valid");
                    return BadRequest(ModelState);
                }

                var author = _mapper.Map<Author>(authorUpdateDTO);
                var isSuccess = await _authorRepository.Update(author);

                if (isSuccess == false)
                {
                    _logger.LogInfo($"{location}: Record coudn't be updated");
                    return StatusCode(500, "The record was not updated, please contact the administrator");
                }

                _logger.LogInfo($"{location}: Record successfully updated");
                return Ok(isSuccess);
            }
            catch (Exception e)
            {
                return InternalError(e, location);
            }
        }


        /// <summary>
        /// Delete an Author
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [Authorize(Roles ="Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionname();

            try
            {
                _logger.LogInfo($"Attempting to delete record with id :{id}");

                var isExists = await _authorRepository.Exists(id);

                if (id < 1 )
                {
                    _logger.LogInfo($"{location}: Deleted failed with bad data - id: {id}");
                    return BadRequest();
                }

                if (!isExists)
                {
                    _logger.LogWarn($"{location}: The record with id:{id} doesn't exists");
                    return NotFound();
                }

                var author = await _authorRepository.FindById(id);
                var isSuccess = await _authorRepository.Delete(author);

                if (!isSuccess)
                {
                    _logger.LogWarn($"{location}: Coudn't Delete record with id: {id}");
                    return StatusCode(500, "Something went wrong, contact the administrator");
                }

                _logger.LogInfo($"Record deleted");
                return NoContent();
            }

            catch (Exception e)
            {
                return InternalError(e, location);
            }
        }


        private string GetControllerActionname()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(Exception e, string location)
        {
            _logger.LogError($"{location}: {e.Message} - {e.InnerException}");
            return StatusCode(500, "Something went wrong, contact the administrator");
        }
    }
}

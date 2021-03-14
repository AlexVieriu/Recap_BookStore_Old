using AutoMapper;
using BookStore_API.Data;
using BookStore_API.DTOs;
using BookStore_API.Services.Contracts;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _logger.LogInfo("Attempted Get All Authors");
                var authors = await _authorRepository.FindAll();
                var authorsDTO = _mapper.Map<IList<AuthorDTO>>(authors);
                _logger.LogInfo("Successfully got all Authors");

                return Ok(authorsDTO);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }


        /// <summary>
        /// Get Author by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Author's record</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            try
            {
                _logger.LogInfo("Attempted Get one Author");

                if (id <= 0)
                    return BadRequest();

                var author = await _authorRepository.FindById(id);

                if (author is null)
                {
                    _logger.LogWarn($"Author with id:{id} was not found");
                    return NotFound();
                }

                var authorDTO = _mapper.Map<AuthorDTO>(author);
                _logger.LogInfo("Successfully the Author");

                return Ok(authorDTO);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }


        /// <summary>
        /// Creates An Author
        /// </summary>
        /// <param name="authorCreateDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorCreateDTO)
        {
            try
            {
                _logger.LogInfo("Attempting to create Author");
                if (authorCreateDTO is null)
                {
                    _logger.LogWarn("Empty request was submited");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("Author data was incomplete");
                    return BadRequest(ModelState);
                }

                var author = _mapper.Map<Author>(authorCreateDTO);

                var isSuccess = await _authorRepository.Create(author);

                if (isSuccess == false)
                {
                    _logger.LogWarn("Author creation failed");
                    return StatusCode(500, "The record was not created");
                }

                _logger.LogInfo("Author Created");
                return Created("Create", new { author });
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }


        /// <summary>
        /// Update an Author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorUpdateDTO"></param>
        /// <returns>Returns the Author Updated</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id,[FromBody] AuthorUpdateDTO authorUpdateDTO)
        {
            try
            {
                _logger.LogInfo($"Attemptin to update the Author with id : {id}");

                if (authorUpdateDTO is null || id < 1 || id != authorUpdateDTO.Id)
                {
                    _logger.LogInfo("Empty request was summited");
                    return BadRequest();
                }

                var isExists = await _authorRepository.Exists(id);

                if(!isExists)
                {
                    _logger.LogWarn($"The Author with id:{id} doesn't exists");
                    return NotFound();
                }

                if(ModelState.IsValid == false)
                {
                    return BadRequest(authorUpdateDTO);
                }

                var author = _mapper.Map<Author>(authorUpdateDTO);
                var isSuccess = await _authorRepository.Update(author);

                if (isSuccess == false)
                {
                    _logger.LogInfo("Author coudn't be updated");
                    return StatusCode(500, "The record was not updated, please contact the administrator");
                }

                return Ok(authorUpdateDTO);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }


        /// <summary>
        /// Delete an Author
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInfo($"Attempting to delete Author with id :{id}");
                
                if(id<1)
                {
                    _logger.LogWarn("Delete fail with bad data");
                    return BadRequest();
                }

                var isExists = await _authorRepository.Exists(id);

                if (!isExists)
                {
                    _logger.LogWarn($"The Author with id:{id} doesn't exists");
                    return NotFound();
                }

                var author = await _authorRepository.FindById(id);
                var isSuccess = await _authorRepository.Delete(author);

                if(!isSuccess)
                {
                    _logger.LogWarn("Coudn't Delete the Author");
                    return StatusCode(500, "Something went wrong, contact the administrator");
                }

                _logger.LogInfo($"Author with id:{id} was deleted");

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
            return StatusCode(500, "Something went wrong, contact the administrator");
        }
    }
}

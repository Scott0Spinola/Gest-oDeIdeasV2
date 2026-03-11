using System.Runtime.CompilerServices;
using GestãoDeIdeasV2.Models;
using GestãoDeIdeasV2.Dtos;
using GestãoDeIdeasV2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestãoDeIdeasV2.Controllers
{
    [Route("Idea")]
    [ApiController]
    public class IdeaController : ControllerBase
    {
        private readonly IdeasService _service;

        private readonly ILogger<IdeaController> _logger;

        public IdeaController(IdeasService service, ILogger<IdeaController> logger, IAdviceService adviceService)
        {
            _service = service;
            _logger = logger;

        }


        /// <summary>
        /// Retrieves all ideas with pagination.
        /// </summary>
        /// <param name="pageParameters">Paging parameters from the query string.</param>
        /// <remarks>
        /// Returns a paged list of ideas based on the provided page number and size.
        /// </remarks>
        /// <response code="200">Ideas retrieved successfully.</response>
        [HttpGet]
        public async Task<ActionResult<PagedList<Idea>>> GetAll([FromQuery] PageParameters pageParameters)
        {
            _logger.LogInformation("Controller {Controller} - Action {Action} called at {Time}",
            nameof(IdeaController), nameof(GetAll), DateTime.UtcNow);

            var pagedIdeas = await _service.GetAllPagedAsync(pageParameters);
            return Ok(pagedIdeas);
        }


        /// <summary>
        /// Retrieves an idea by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the idea.</param>
        /// <remarks>
        /// Returns the idea that matches the provided ID.
        /// </remarks>
        /// <response code="200">Idea retrieved successfully.</response>
        /// <response code="404">No idea exists with the provided ID.</response>
        [HttpGet("{id:int}")]
        public ActionResult<Idea> GetById(int id)
        {

            _logger.LogInformation("Controller {Controller} - Action {Action} - Id {Id} called at {Time}",
            nameof(IdeaController), nameof(GetById), id, DateTime.UtcNow);

            var idea = _service.GetByIdea(id);
            if (idea is null)
            {
                return NotFound($"No Idea exist with the provided ID: {id}.");
            }
            return Ok(idea);
        }

        /// <summary>
        /// Retrieves ideas filtered by priority.
        /// </summary>
        /// <param name="priority">The priority value used to filter ideas.</param>
        /// <remarks>
        /// Returns all ideas that have the specified priority level.
        /// </remarks>
        /// <response code="200">Ideas with the specified priority retrieved successfully.</response>
        /// <response code="404">No ideas exist with the provided priority.</response>
        [HttpGet("priority/{priority:int}")]
        public ActionResult<List<Idea>> GetByPriority(int priority)
        {
            _logger.LogInformation("Controller {Controller} - Action {Action} - Priority {Priority} called at {Time}",
            nameof(IdeaController), nameof(GetByPriority), priority, DateTime.UtcNow);

            var idea = _service.GetByPriority(priority);
            if (idea.Count == 0)
            {
                return NotFound($"No Idea exist with the provided priority: {priority}");
            }
            return Ok(idea);
        }


        /// <summary>
        /// Creates a new idea.
        /// </summary>
        /// <param name="createIdea">The data used to create a new idea.</param>
        /// <remarks>
        /// Creates a new idea and returns the created resource with its generated identifier.
        /// </remarks>
        /// <response code="201">Idea created successfully.</response>
        /// <response code="400">The request data is invalid.</response>
        [HttpPost]
        public ActionResult<Idea> Create(CreateIdea createIdea)
        {
            try
            {
                _logger.LogInformation("Create called at {Time}", DateTime.UtcNow);
                var createdIdea = _service.Create(createIdea);
                return CreatedAtAction(nameof(GetById), new { id = createdIdea.Id }, createdIdea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                        "Error in {Controller}.{Action} at {Time}",
                        nameof(IdeaController), nameof(Create), DateTime.UtcNow);

                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");

            }
        }

        /// <summary>
        /// Updates an existing idea.
        /// </summary>
        /// <param name="id">The unique identifier of the idea to update.</param>
        /// <param name="updateIdea">The updated values for the idea.</param>
        /// <remarks>
        /// Updates the idea that matches the provided ID with the supplied data.
        /// </remarks>
        /// <response code="200">Idea updated successfully.</response>
        /// <response code="404">No idea exists with the provided ID.</response>
        [HttpPut("{id:int}")]
        public ActionResult<Idea> Update(int id, UpdateIdea updateIdea)
        {
            try
            {   
                _logger.LogInformation("Update called at {Time}", DateTime.UtcNow);
                var updatedIdea = _service.Update(id, updateIdea);

                if (updatedIdea is null)
                {
                    return NotFound($"No Idea exist with the provided ID: {id}.");
                }

                return Ok(updatedIdea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Controller}.{Action} for Id {Id} at {Time}",
                nameof(IdeaController), nameof(Update), id, DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }

        /// <summary>
        /// Deletes an idea.
        /// </summary>
        /// <param name="id">The unique identifier of the idea to delete.</param>
        /// <remarks>
        /// Removes the idea that matches the provided ID from the system.
        /// </remarks>
        /// <response code="204">Idea deleted successfully.</response>
        /// <response code="404">No idea exists with the provided ID.</response>
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {   
                _logger.LogInformation("Delete called at {Time}", DateTime.UtcNow);
                var DeleteIdea = _service.Delete(id);
                if (!DeleteIdea)
                {
                    return NotFound($"No Idea exists with the provided ID: {id}.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Controller}.{Action} for Id {Id} at {Time}",
                nameof(IdeaController), nameof(Update), id, DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }


    }
}

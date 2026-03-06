using System.Runtime.CompilerServices;
using GestãoDeIdeasV2.Models;
using GestãoDeIdeasV2.Dtos;
using GestãoDeIdeasV2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestãoDeIdeasV2.Controllers;



[Route("IdeaWithAdvice")]
[ApiController]

public class IdeaWithAdviceController : ControllerBase
{
    private readonly IAdviceService _adviceService;
    private readonly IdeasService _service;

    private readonly ILogger<IdeaWithAdviceController> _logger;

    public IdeaWithAdviceController(IdeasService service, ILogger<IdeaWithAdviceController> logger, IAdviceService adviceService)
    {
        _service = service;
        _logger = logger;
        _adviceService = adviceService;
    }

    /// <summary>
    /// Retrieves all ideas with a random advice.
    /// </summary>
    /// <remarks>
    /// Returns the complete list of ideas along with a single random advice message.
    /// </remarks>
    /// <response code="200">Ideas and advice retrieved successfully.</response>
    [HttpGet]
    public async Task<ActionResult<IdeasWithAdviceResponse>> GetAll()
    {
        _logger.LogInformation("GetAll with Advice called at {Time}", DateTime.UtcNow);

        var ideas = _service.GetAll();
        var advice = await _adviceService.GetRandomAdviceAsync();

        var response = new IdeasWithAdviceResponse
        {
            Ideas = ideas,
            Advice = advice
        };

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a specific idea with a random advice.
    /// </summary>
    /// <param name="id">The unique identifier of the idea.</param>
    /// <remarks>
    /// Returns the idea that matches the provided ID together with a random advice message.
    /// </remarks>
    /// <response code="200">Idea and advice retrieved successfully.</response>
    /// <response code="404">No idea exists with the provided ID.</response>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IdeaWithAdviceResponse>> GetById(int id)
    {
        _logger.LogInformation("GetById wiht Advice called at {Time}", DateTime.UtcNow);

        var idea = _service.GetByIdea(id);
        if (idea is null)
        {
            return NotFound($"No Idea exist with the provided ID: {id}.");
        }

        var advice = await _adviceService.GetRandomAdviceAsync();

        var response = new IdeaWithAdviceResponse
        {
            Idea = idea,
            Advice = advice
        };

        return Ok(response);
    }
}


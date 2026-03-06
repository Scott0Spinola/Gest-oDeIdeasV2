using System.Runtime.CompilerServices;
using GestãoDeIdeasV2.Models;
using GestãoDeIdeasV2.Dtos;
using GestãoDeIdeasV2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestãoDeIdeasV2.Controllers
{
    [Route("idea")]
    [ApiController]
    public class IdeaController : ControllerBase
    {
        private readonly IdeasService _service;

        private readonly ILogger<IdeaController> _logger;

        public IdeaController(IdeasService service, ILogger<IdeaController> logger)
        {
            _service = service;
            _logger = logger;

        }



        [HttpGet]
        public ActionResult<List<Idea>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        
        [HttpGet("{id:int}")]
        public ActionResult<Idea> GetById(int id)
        {
            var idea = _service.GetByIdea(id);
            if (idea is null)
            {
                return NotFound($"No Idea exist with the provided ID: {id}.");
            }
            return Ok(idea);
        }

        [HttpGet("priority/{priority:int}")]
        public ActionResult<List<Idea>> GetByPriority(int priority)
        {
            var idea = _service.GetByPriority(priority);
            if (idea is null)
            {
                return NotFound($"No Idea exist with the provided priority: {priority}");
            }
            return Ok(idea);
        }


        [HttpPost]
        public ActionResult<Idea> Create(CreateIdea createIdea)
        {
            try
            {
                var createdIdea = _service.Create(createIdea);
                return CreatedAtAction(nameof(GetById), new { id = createdIdea.Id }, createdIdea);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult<Idea> Update(int id, UpdateIdea updateIdea)
        {
            try
            {
                var updatedIdea = _service.Update(id, updateIdea);

                if (updatedIdea is null)
                {
                    return NotFound($"No Idea exist with the provided ID: {id}.");
                }

                return Ok(updatedIdea);
            }
            catch (Exception)
            {

                throw;
            }

        }
        
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var DeleteIdea = _service.Delete(id);
                if (!DeleteIdea)
                {
                    return NotFound($"No Idea exists with the provided ID: {id}.");
                }
                return NoContent();
            }
            catch (Exception)
            {
                
                throw;
            }
        }


    }
}

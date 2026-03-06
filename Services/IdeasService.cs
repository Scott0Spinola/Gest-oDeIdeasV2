using GestãoDeIdeasV2.Data;
using GestãoDeIdeasV2.Models;
using GestãoDeIdeasV2.Dtos;
using Microsoft.EntityFrameworkCore;
namespace GestãoDeIdeasV2.Services;

public class IdeasService
{
    private readonly IdeaContext _context;

    private readonly ILogger<IdeasService> _logger;


    public IdeasService(IdeaContext context, ILogger<IdeasService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public List<Idea> GetAll()
    {
        return _context.Ideas.OrderBy(i => i.Id).ToList();
    }

    public Idea? GetByIdea(int id)
    {
        return _context.Ideas.FirstOrDefault(i => i.Id == id);
    }

    public List<Idea> GetByPriority(int priority)
    {
        return _context.Ideas.Where(p => p.Priority == priority).ToList();
    }

    public Idea Create(CreateIdea createIdea)
    {
        var idea = new Idea
        {
            Id = 0,
            Name = createIdea.Name,
            Description = createIdea.Description,
            State = createIdea.State,
            Priority = createIdea.Priority,
            CreatedAt = DateTime.UtcNow
        };

        _context.Ideas.Add(idea);
        _context.SaveChanges();
        return idea;
    }

    public Idea? Update(int id, UpdateIdea updateIdea)
    {
        var idea = _context.Ideas.FirstOrDefault(i => i.Id == id);

        if (idea is null)
        {
            return null;
        }

        idea.Name = updateIdea.Name;
        idea.Description = updateIdea.Description;
        idea.State = updateIdea.State;
        idea.Priority = updateIdea.Priority;

        _context.SaveChanges();
        return idea;
    }

    public bool Delete(int id)
    {
        var idea = _context.Ideas.FirstOrDefault(i => i.Id == id);
        if(idea is null)
        {
            return false;
        }
        _context.Ideas.Remove(idea);
        _context.SaveChanges();
        return true;
    }
}

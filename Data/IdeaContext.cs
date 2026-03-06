using GestãoDeIdeasV2.Models;
using Microsoft.EntityFrameworkCore;

namespace GestãoDeIdeasV2.Data;

public class IdeaContext(DbContextOptions<IdeaContext> options ) : DbContext (options)
{
    public DbSet<Idea> Ideas => Set<Idea>();
}

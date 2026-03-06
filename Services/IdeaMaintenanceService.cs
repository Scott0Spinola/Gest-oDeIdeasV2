using GestãoDeIdeasV2.Data;
using GestãoDeIdeasV2.Models;

namespace GestãoDeIdeasV2.Services;

public class IdeaMaintenanceService
{
     public static void UpdateOutdatedIdeas(IdeaContext db, DateTime utcNow)
    {
        var cutoffDate = utcNow.AddMonths(-1);

        var outdatedIdeas = db.Ideas
            .Where(i => i.CreatedAt <= cutoffDate && i.State != IdeaState.COMPLETED && i.State != IdeaState.ABANDONED)
            .ToList();

        if (outdatedIdeas.Count == 0)
        {
            return;
        }

        foreach (var idea in outdatedIdeas)
        {
            idea.State = IdeaState.ABANDONED;
        }

        db.SaveChanges();
    }
}

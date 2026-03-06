using GestãoDeIdeasV2.Models;

namespace GestãoDeIdeasV2.Dtos;

public class IdeaWithAdviceResponse
{
    public Idea? Idea { get; set; }
    public string Advice { get; set; } = string.Empty;
}

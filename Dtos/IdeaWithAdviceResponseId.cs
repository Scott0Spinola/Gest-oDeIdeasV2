
using GestãoDeIdeasV2.Models;
namespace GestãoDeIdeasV2.Dtos;

public class IdeaWithAdviceResponseId
{
    public Idea? Idea { get; set; }
    public string Advice { get; set; } = string.Empty;
}

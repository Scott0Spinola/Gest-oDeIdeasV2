
using GestãoDeIdeasV2.Models;
namespace GestãoDeIdeasV2.Dtos;

public class IdeasWithAdviceResponseAll
{
    public List<Idea> Ideas { get; set; } = new();
    public string Advice { get; set; } = string.Empty;
}

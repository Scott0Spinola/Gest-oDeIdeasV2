using System.Collections.Generic;
using GestãoDeIdeasV2.Models;

namespace GestãoDeIdeasV2.Dtos;

public class IdeasWithAdviceResponse
{
    public List<Idea> Ideas { get; set; } = new();
    public string Advice { get; set; } = string.Empty;
}

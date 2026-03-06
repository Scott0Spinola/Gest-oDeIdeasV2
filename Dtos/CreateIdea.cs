using GestãoDeIdeasV2.Models;
using System.ComponentModel.DataAnnotations;
namespace GestãoDeIdeasV2.Dtos;

public record CreateIdea
(
   [Required]
   [MaxLength(100)]
   string Name,
   
   [Required]
   [MaxLength(500)]
   string Description,

   [Required]
   IdeaState State,

   [Range(1, 5)]
   int Priority

);

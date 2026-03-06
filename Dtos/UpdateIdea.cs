namespace GestãoDeIdeasV2.Dtos;
using GestãoDeIdeasV2.Models;
using System.ComponentModel.DataAnnotations;
public record UpdateIdea
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

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StealCatsService.Entities;

[Table("Tags")]
public class Tag
{
  public int Id { get; set; }
  public string BreedId { get; set; }
  public string Name { get; set; }
  public DateTime CreatedAt { get; set;} = DateTime.UtcNow;

  public ICollection<Cat> Cats { get; set; }
}

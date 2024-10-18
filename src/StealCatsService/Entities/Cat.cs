using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace StealCatsService.Entities;
[Table("Cats")]
public class Cat
{
  public int Id { get; set;}
  public int CatId { get; set;}
  public int Width { get; set;}
  public int Height { get; set;}
  public DateTime CreatedAt { get; set;} = DateTime.UtcNow;
  

  public CatImage Image { get; set;}
  // N <--> N Relationship
  public ICollection<Tag> Tags {get; set;}
}

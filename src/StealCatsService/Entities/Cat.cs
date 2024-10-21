using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace StealCatsService.Entities;
[Table("Cats")]
public class Cat
{
  
  public int Id { get; set;}
  public string CatId { get; set;}
  public int Width { get; set;}
  public int Height { get; set;}
  public string ImageUrl { get; set;}
  public DateTime CreatedAt { get; set;} = DateTime.UtcNow;
  
  // Reference navigation to dependent
  public ICollection<Tag> Tags {get; set;}  // N <--> N Relationship
}

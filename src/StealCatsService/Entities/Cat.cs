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
  public ImageStruct Image { get; set;}
  public DateTime CreatedAt { get; set;} = DateTime.UtcNow;
  
  // N <--> N Relationship
  public ICollection<Tag> Tags {get; set;}
}


public struct ImageStruct
{
  public string ImageId { get; set; }
  public string Url { get; set; }
  public string Description { get; set; }
  public int Width {  get; set; }
  public int Height { get; set; }

  
}

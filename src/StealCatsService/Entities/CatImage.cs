using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StealCatsService.Entities;

[Table("Image")]
public class CatImage
{
  
  public int Id { get; set; }
  public string Url { get; set; }
  public string BreedId { get; set; }
  public int CatImageForeignKey { get; set; } // Foreign Key property
  // Navigation property
  public Cat Cat { get; set;} = null;

}

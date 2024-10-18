using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StealCatsService.Entities;

[Table("Images")]
public class ImageClass
{
  [Key]
  public string ImageId { get; set; }
  public string Url { get; set; }
  public string Description { get; set; }
  public int Width {  get; set; }
  public int Height { get; set; }

}
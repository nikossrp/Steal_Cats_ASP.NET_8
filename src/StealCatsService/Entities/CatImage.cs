using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace StealCatsService.Entities;

[Table("Images")]
public class CatImage
{
  [Key]
  public string ImageId { get; set; }
  public string Url { get; set; }
  public int Width {  get; set; }
  public int Height { get; set; }
}
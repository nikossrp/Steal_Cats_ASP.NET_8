using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace StealCatsService.Entities;

[Table("Images")]
public class CatImage
{
  [Key]
  public string ImageId { get; set; }
  public string Url { get; set; }
  [NotMapped]     // we keep this on Cats Table
  public int Width {  get; set; }
  [NotMapped]
  public int Height { get; set; }
  public List<string> Breed_Temperament { get; set; }
}
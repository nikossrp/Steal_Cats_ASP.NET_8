using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StealCatsService.Entities;

public class CatDto
{
  public string CatId { get; set; }
  public string BreedId { get; set; }
  public string ImageUrl { get; set; }
  public int Width {  get; set; }
  public int Height { get; set; }
  public string Breed_Temperament { get; set; }
}
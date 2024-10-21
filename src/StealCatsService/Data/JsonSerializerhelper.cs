using System;
using System.Text.Json;
using StealCatsService.Entities;

namespace StealCatsService.Data;

public class JsonSerializerHelper
{
  public static List<CatDto> CatDeserializer(string JsonString)
  {
    
    // string [] jsonArr = SplitBetweenDelimiters(Json, "", "");
    List<CatDto> catImageList = new List<CatDto>();

    foreach (JsonElement item in JsonDocument.Parse(JsonString).RootElement.EnumerateArray())
    {
        CatDto catImage = new CatDto();
        catImage.CatId = item.GetProperty("id").GetString();

        catImage.ImageUrl = item.GetProperty("url").GetString();
        catImage.Width = item.GetProperty("width").GetInt32();
        catImage.Height = item.GetProperty("height").GetInt32();
        if (item.TryGetProperty("breeds", out JsonElement breedsArray))
        {
          foreach(JsonElement breed in breedsArray.EnumerateArray())
          {
            catImage.BreedId = breed.GetProperty("id").GetString();
            catImage.Breed_Temperament = breed.GetProperty("temperament").GetString();
          }
        }

        Console.WriteLine(catImage.ImageUrl);

        catImageList.Add(catImage);
    }

    return catImageList;
  }



}

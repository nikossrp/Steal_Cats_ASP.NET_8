using System;
using System.Text.Json;
using StealCatsService.Entities;

namespace StealCatsService.Data;

public class JsonSerializerHelper
{
  public static List<CatImage> CatDeserializer(string JsonString)
  {
    
    // string [] jsonArr = SplitBetweenDelimiters(Json, "", "");
    List<CatImage> catImageList = new List<CatImage>();

    foreach (JsonElement item in JsonDocument.Parse(JsonString).RootElement.EnumerateArray())
    {
        CatImage catImage = new CatImage();
        catImage.ImageId = item.GetProperty("id").GetString();

        catImage.Url = item.GetProperty("url").GetString();
        catImage.Width = item.GetProperty("width").GetInt32();
        catImage.Height = item.GetProperty("height").GetInt32();
        if (item.TryGetProperty("breeds", out JsonElement breedsArray))
        {
          foreach(JsonElement breed in breedsArray.EnumerateArray())
          {
            string breedId = breed.GetProperty("id").GetString();
            string temperament = breed.GetProperty("temperament").GetString();
            catImage.Breed_Temperament = temperament.Split(',').ToList();
          }
        }

        catImageList.Add(catImage);
    }

    return catImageList;
  }



}

using System;
using StealCatsService.Entities;

namespace StealCatsService.Data;

public class JsonSerializerHelper
{
  public static CatImage CatDeserializer(string Json)
  {
    // string [] jsonArr = SplitBetweenDelimiters(Json, "", "");
    CatImage catImage = new CatImage();
    catImage.ImageId = "awdq";//jsonArr[0];
    catImage.Url = "adw";//jsonArr[1];
    catImage.Width = 1;//Int32.Parse(jsonArr[2]);
    catImage.Height = 2;//Int32.Parse(jsonArr[3]);

    return catImage;
  }

  private static string[] SplitBetweenDelimiters(string input, string startDel, string endDel)
  {


    return [];
  }
}

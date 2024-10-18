using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StealCatsService.Data;
using StealCatsService.Entities;

namespace StealCatsService.Controllers;

[ApiController]
[Route("api/cats")]
public class CatsController : ControllerBase
{
  private readonly HttpClient _httpClient;
  public CatsController(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  [HttpPost("fetch")]
  public async Task<IActionResult> FetchCatsImages()
  {

    string url = "https://api.thecatapi.com/v1/images/search";
    int nImageCats = 25;

    try 
    {
      var response = await _httpClient.GetAsync(url);
      if (response.IsSuccessStatusCode) 
      {
        CatImage[] catImagesArr = new CatImage[nImageCats];

        for (int i = 0; i < nImageCats; i++ ) 
        {
          var data = await response.Content.ReadAsStringAsync();
          var catImage = JsonSerializerHelper.CatDeserializer(data);
          catImagesArr[i] = catImage;
        }
        
        return Ok(catImagesArr[0].ImageId);
      }

      return StatusCode((int)response.StatusCode);

    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Error occured: {ex.Message}");
    }






  }
}

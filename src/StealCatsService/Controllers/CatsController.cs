using System;
using Microsoft.AspNetCore.Mvc;
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
    string apiKey = "live_KRXInFTvQOVG2t5MbHVEJOccLWcJpW401uMFrRhwSUaIYtlOoY58n5WnEnMc4W1z";
    string breeds = "has_breeds=true";
    int nImageCats = 1;

    // https://api.thecatapi.com/v1/images/search?limit=1&has_breeds=true&api_key=live_KRXInFTvQOVG2t5MbHVEJOccLWcJpW401uMFrRhwSUaIYtlOoY58n5WnEnMc4W1z&id
    // https://api.thecatapi.com/v1/images/N8bl5RjPG
    string url = $"https://api.thecatapi.com/v1/images/search?limit={nImageCats}&{breeds}&api_key={apiKey}?";
    
    try 
    {
      var response = await _httpClient.GetAsync(url);
      if (response.IsSuccessStatusCode) 
      {
        var data = await response.Content.ReadAsStringAsync();
        List<CatImage> dataCat = JsonSerializerHelper.CatDeserializer(data);

        return Ok(dataCat[0].Breed_Temperament[0]);
      }

      return StatusCode((int)response.StatusCode);

    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Error occured: {ex.Message}");
    }
  }
}

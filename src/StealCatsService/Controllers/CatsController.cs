using System;
using Microsoft.AspNetCore.Mvc;

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
    string url = "https://api.thecatapi.com/v1/images/search?limit=25";

    try 
    {
      var response = await _httpClient.GetAsync(url);
      if (response.IsSuccessStatusCode) 
      {
        var data = await response.Content.ReadAsStringAsync();

        return Ok(data);
      }

      return StatusCode((int)response.StatusCode);

    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Error occured: {ex.Message}");
    }
  }

   

}

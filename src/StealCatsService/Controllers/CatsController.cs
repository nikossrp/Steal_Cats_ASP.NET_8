using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StealCatsService.Data;
using StealCatsService.Entities;

namespace StealCatsService.Controllers;

[ApiController]
[Route("api/cats")]
public class CatsController : ControllerBase
{
  private readonly HttpClient _httpClient;
  private readonly CatDbContent _catDbContent;
  private readonly string apiKey;


  public CatsController(HttpClient httpClient, CatDbContent catDbContent)
  {
    _httpClient = httpClient;
    _catDbContent = catDbContent;
    apiKey = "live_KRXInFTvQOVG2t5MbHVEJOccLWcJpW401uMFrRhwSUaIYtlOoY58n5WnEnMc4W1z";  //Environment.GetEnvironmentVariable("API_KEY");
  }


  [HttpPost("fetch")]
  public async Task<IActionResult> FetchCatsImages()
  {
    int count = 0;    // for let the user know how much images inserted on DB on each fetch request
    bool boolBreeds = true;
    int limit = 2;
    string url =  $"https://api.thecatapi.com/v1/images/search?limit={limit}&has_breeds={boolBreeds}&api_key={apiKey}";

    try 
    {
      var response = await _httpClient.GetAsync(url);

      if (response.IsSuccessStatusCode) 
      {
        var data = await response.Content.ReadAsStringAsync();
        List<CatDto> dataCatImages = JsonSerializerHelper.CatDeserializer(data);
        HashSet<string> seenBreedIds = new HashSet<string>();   // store breedId to prevent adding it to the dB again

        foreach(var catImage in dataCatImages) 
        {
          // Check if CatImage already exist in DB 
          bool catImageExists = await _catDbContent.Cats.AnyAsync(i => i.CatId == catImage.CatId);
          if (!catImageExists) {    
            var cat = new Cat();

            cat.CatId = catImage.CatId;
            cat.Width = catImage.Width;
            cat.Height = catImage.Height;
            cat.Tags = new List<Tag>();
            
            //  Tags on DB
            var existingTag = await _catDbContent.CatImages
              .FirstOrDefaultAsync(v => v.BreedId == catImage.BreedId);

            // check if it exists on DB or exists on fetch request
            if (existingTag == null)    // empty string means BreedId already exist on List and may have inserted. 
            {
              if (!seenBreedIds.Contains(catImage.BreedId))
              {
                cat.Tags.Add(new Tag { Name = catImage.Breed_Temperament });
                seenBreedIds.Add(catImage.BreedId);  
              }
            }
            
            var existingImage = await _catDbContent.Cats
              .FirstOrDefaultAsync(v => v.CatId == catImage.CatId);
            if (existingImage == null) 
            {
              cat.Image = new CatImage{ 
                  Url = catImage.Url, 
                  BreedId = catImage.BreedId 
              };

              Console.WriteLine(cat.Image.Url);
              _catDbContent.Cats.Add(cat);

              count ++;
            }

          }
        }
     
        await _catDbContent.SaveChangesAsync();

        return Ok($"{limit} fetched from API, {count} Images saved on DB");
      }

      return StatusCode((int)response.StatusCode);

    }
    catch (Exception ex)
    {
      var innerExceptionMessage = ex.InnerException?.Message ?? "No inner exception.";

      return StatusCode(500, $"Error occurred: {ex.Message}. Inner exception: {innerExceptionMessage}");

    }
  }


[HttpGet("{id}")]
public async Task<IActionResult> GetCatById(string id)
{

    // Correct SQL query
    var sqlQuery = @"
      SELECT c.Id, c.CatId, c.Width, c.Height, i.Url, t.Name
      FROM Cats c
      JOIN Image i ON c.Id = i.CatImageForeignKey
      JOIN CatTags ct on c.Id = ct.TagsId
      JOIN Tags t ON c.Id = t.Id
      WHERE c.CatId = @p0";

    var catTable = await _catDbContent.Cats
      .FromSqlRaw(sqlQuery, id)
      .Select(c => new {
        CatId = c.CatId,
        Width = c.Width,
        Height = c.Height,
        Url = c.Image.Url,
      })
      .FirstOrDefaultAsync();


    if (catTable == null)
    {
        return NotFound("Cat data could not be retrieved");
    }

    return Ok(catTable);
}


  // [HttpGet]
  // public async Task<IActionResult> GetCatsWithPagingSupport(int page = 1, int pageSize = 10, string tagName = null)
  // {
  //   var totalCound = await _catDbContent.Cats.CountAsync();
    
  //   var cats = await _catDbContent.Cats
  //     .Skip((page - 1) * pageSize)
  //     .Take(pageSize)
  //     .ToListAsync();

  //   var results = new { 
  //     TotalCound = totalCound, 
  //     Page = page, 
  //     PageSize = pageSize, 
  //     Cats = cats
  //   };

  //   return  Ok(results);

  // }



}

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
    int count = 0;    // keep tracking of the Images adding on each fetch request
    bool boolBreeds = true;
    int limit = 25;
    string url =  $"https://api.thecatapi.com/v1/images/search?limit={limit}&has_breeds={boolBreeds}&api_key={apiKey}";

    try 
    {
      var response = await _httpClient.GetAsync(url);
      if (response.IsSuccessStatusCode) 
      {
        var data = await response.Content.ReadAsStringAsync();
        List<CatDto> dataCatImages = JsonSerializerHelper.CatDeserializer(data);
        HashSet<string> seenBreedIds = new HashSet<string>();   // store breedId to prevent adding it to the DB again on the current fetch request
        List<Tag> seenTags = new List<Tag>();



        foreach(var catImage in dataCatImages) 
        {
          // Check if CatImage already exist in DB 
          bool catImageExists = await _catDbContent.Cats.AnyAsync(i => i.CatId == catImage.CatId);
          if (!catImageExists) {    
            var cat = new Cat();

            cat.CatId = catImage.CatId;
            cat.Width = catImage.Width;
            cat.Height = catImage.Height;
            cat.ImageUrl = catImage.ImageUrl;
            
            //  Tags on DB
            var existingTag = await _catDbContent.Tags
              .FirstOrDefaultAsync(v => v.BreedId == catImage.BreedId);

            // check if it exists on DB 
            if (existingTag == null)   
            {
              cat.Tags = new List<Tag>();

              // check if it exists on the list (prevent adding duplicate tags on DB)
              if (!seenBreedIds.Contains(catImage.BreedId))
              {
                Tag tempTag = new Tag {
                  Name = catImage.Breed_Temperament, 
                  BreedId = catImage.BreedId 
                };

                cat.Tags.Add(tempTag);

                // keep track on which Tags you have on the fetch request 
                seenBreedIds.Add(catImage.BreedId); 
                seenTags.Add(tempTag);      
              }
              else {  // if you have already seen this tag on the fetch List, associate with it
                cat.Tags = new List<Tag> {seenTags.FirstOrDefault(c => c.BreedId == catImage.BreedId)};
              }
            }
            else {    // if already exists on DB associate with it
                cat.Tags = new List<Tag> {existingTag};
            }

            _catDbContent.Cats.Add(cat);
            count ++;
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

      var sqlQuery = @"
        SELECT c.Id, c.Width, c.Height, c.ImageUrl, t.Name
        FROM Cats c
        JOIN CatTags ct ON c.Id = ct.CatsId
        JOIN Tags t ON c.Id = t.Id
        WHERE c.CatId = @p0";

      var catTable = await _catDbContent.Cats
        .FromSqlRaw(sqlQuery, id)
        .Select(c => new {
          // CatId = c.CatId,
          Width = c.Width,
          Height = c.Height,
          ImageUrl = c.ImageUrl,
          Temperament = c.Tags.FirstOrDefault().Name    // on this level we have 1 cat
        })
        .FirstOrDefaultAsync();


      if (catTable == null)
      {
          return NotFound("Cat data not found in the database");
      }
      

      return Ok(catTable);
  }


  [HttpGet]
  public async Task<IActionResult> GetCatsWithPagingSupport(int page = 1, int pageSize = 10, string tagName = null)
  {
    var totalCound = await _catDbContent.Cats.CountAsync();
    List<Cat> cats = null;
    List<Tag> tags = null;

    if (tagName == null) 
    {
      cats = await _catDbContent.Cats
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }
    
    if (tagName != null)
    {

      // join tables and extract c.Id, c.CatId, c.Width, c.Height, i.Url, t.Name
      var sqlQuery = @"
        SELECT *
        FROM Cats c
        JOIN CatTags ct ON c.Id = ct.CatsId
        JOIN Tags t ON c.Id = t.Id
        WHERE t.Name LIKE '%p0%'";

      var catsTags = await _catDbContent.Cats
          .FromSqlRaw(sqlQuery, tagName)
          .Select(c => new {
            CatId = c.CatId,
            Width = c.Width,
            Height = c.Height,
            ImageUrl = c.ImageUrl,
          })
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

      tags = await _catDbContent.Tags
        .Where(t => t.Name == tagName)
        .ToListAsync();

      return Ok(catsTags);
    }


    var results = new { 
      TotalCound = totalCound, 
      Page = page, 
      PageSize = pageSize, 
      Cats = cats
    };




    return  Ok(results);
  }
}

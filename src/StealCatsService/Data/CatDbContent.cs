using System;
using Microsoft.EntityFrameworkCore;
using StealCatsService.Entities;

namespace StealCatsService.Data;

public class CatDbContent : DbContext
{
    public CatDbContent(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Cat> Cats { get; set; }    // Tables Cats, Tags 
}

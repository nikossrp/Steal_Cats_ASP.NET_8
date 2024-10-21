using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StealCatsService.Controllers;
using StealCatsService.Entities;

namespace StealCatsService.Data;

public class CatDbContent : DbContext
{
    public CatDbContent(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Cat> Cats { get; set; }    // Tables Cats
    public DbSet<Tag> Tags { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cat>()
            .HasMany(c => c.Tags)
            .WithMany(c => c.Cats)    // Join table for the many-to-many relationship
            .UsingEntity(t => t.ToTable("CatTags"));    
    }

}

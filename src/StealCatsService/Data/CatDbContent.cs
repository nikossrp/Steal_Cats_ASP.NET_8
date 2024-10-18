using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StealCatsService.Entities;

namespace StealCatsService.Data;

public class CatDbContent : DbContext
{
    public CatDbContent(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Cat> Cats { get; set; }    // Tables Cats, Tags 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ValueComparer for List<string>
        var stringListComparer = new ValueComparer<List<string>>(
            (c1, c2) => c1.SequenceEqual(c2),  // Compare list contents
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),  // Hash code for the list
            c => c.ToList() 
        );


        // Cat
        modelBuilder.Entity<CatImage>().Property(x => x.Breed_Temperament)
            .HasConversion(     // Converts List<string> to a comma-separated string for storing in the DB
                v => string.Join(",", v),      
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .Metadata
            .SetValueComparer(stringListComparer);

        // Tag
        modelBuilder.Entity<Tag>().Property(x => x.Name)
            .HasConversion(     // Converts List<string> to a comma-separated string for storing in the DB
                v => string.Join(",", v),      
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .Metadata
            .SetValueComparer(stringListComparer);

    }

}

using Microsoft.EntityFrameworkCore;
using DotNetProject.Models;

namespace DotNetProject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Character> Characters { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Quote relationships
        modelBuilder.Entity<Quote>()
            .HasOne(q => q.Character)
            .WithMany(c => c.Quotes)
            .HasForeignKey(q => q.CharacterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Quote>()
            .HasOne(q => q.Episode)
            .WithMany(e => e.Quotes)
            .HasForeignKey(q => q.EpisodeId)
            .OnDelete(DeleteBehavior.Restrict);


        // Seed initial data for Characters (with media)
        modelBuilder.Entity<Character>().HasData(
            new Character
            {
                Id = 1,
                Name = "Rachel Green",
                ActorName = "Jennifer Aniston",
                Occupation = "Fashion Executive",
                Description = "Initially a waitress at Central Perk, later becomes a fashion executive at Ralph Lauren.",
                ImageUrl = "/img/friends/rachel.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Rachel%20Green%20Friends%20best%20moments"
            },
            new Character
            {
                Id = 2,
                Name = "Monica Geller",
                ActorName = "Courteney Cox",
                Occupation = "Head Chef",
                Description = "A perfectionist chef known for her cleanliness obsession and competitive nature.",
                ImageUrl = "/img/friends/monica.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Monica%20Geller%20Friends%20kitchen%20scenes"
            },
            new Character
            {
                Id = 3,
                Name = "Phoebe Buffay",
                ActorName = "Lisa Kudrow",
                Occupation = "Masseuse/Musician",
                Description = "A quirky masseuse and musician known for her song 'Smelly Cat'.",
                ImageUrl = "/img/friends/phoebe.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Phoebe%20Buffay%20Smelly%20Cat"
            },
            new Character
            {
                Id = 4,
                Name = "Joey Tribbiani",
                ActorName = "Matt LeBlanc",
                Occupation = "Actor",
                Description = "A lovable but dim-witted aspiring actor best known for his role as Dr. Drake Ramoray.",
                ImageUrl = "/img/friends/joey.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Joey%20Tribbiani%20how%20you%20doin"
            },
            new Character
            {
                Id = 5,
                Name = "Chandler Bing",
                ActorName = "Matthew Perry",
                Occupation = "IT Procurement/Advertising",
                Description = "Known for his sarcastic wit and comedic timing. Later transitions to advertising.",
                ImageUrl = "/img/friends/chandler.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Chandler%20Bing%20sarcasm"
            },
            new Character
            {
                Id = 6,
                Name = "Ross Geller",
                ActorName = "David Schwimmer",
                Occupation = "Paleontologist",
                Description = "Monica's older brother, a paleontologist who has been married three times.",
                ImageUrl = "/img/friends/ross.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Ross%20Geller%20pivot%20scene"
            }
        );

        // Seed initial data for Episodes (with media)
        modelBuilder.Entity<Episode>().HasData(
            new Episode
            {
                Id = 1,
                Title = "The One Where Monica Gets a Roommate",
                Season = 1,
                EpisodeNumber = 1,
                AirDate = new DateTime(1994, 9, 22),
                Description = "Rachel runs away from her wedding and moves in with Monica.",
                ImageUrl = "/img/friends/ep-s01e01.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Friends%20S01E01%20pilot%20clip"
            },
            new Episode
            {
                Id = 2,
                Title = "The One with the Sonogram at the End",
                Season = 1,
                EpisodeNumber = 2,
                AirDate = new DateTime(1994, 9, 29),
                Description = "Ross finds out his ex-wife Carol is pregnant with his baby.",
                ImageUrl = "/img/friends/ep-s01e02.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Friends%20sonogram%20episode"
            },
            new Episode
            {
                Id = 3,
                Title = "The One with the Blackout",
                Season = 1,
                EpisodeNumber = 7,
                AirDate = new DateTime(1994, 11, 3),
                Description = "A power outage in New York City leads to adventures for the group.",
                ImageUrl = "/img/friends/ep-s01e07.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Friends%20blackout%20episode"
            },
            new Episode
            {
                Id = 4,
                Title = "The One Where Ross Finds Out",
                Season = 2,
                EpisodeNumber = 7,
                AirDate = new DateTime(1995, 11, 9),
                Description = "Ross discovers Rachel's feelings for him through a drunken voicemail.",
                ImageUrl = "/img/friends/ep-s02e07.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Friends%20Ross%20finds%20out%20episode"
            },
            new Episode
            {
                Id = 5,
                Title = "The One with the Prom Video",
                Season = 2,
                EpisodeNumber = 14,
                AirDate = new DateTime(1996, 2, 1),
                Description = "The gang watches Rachel and Monica's prom video, revealing Ross's long-time love for Rachel.",
                ImageUrl = "/img/friends/ep-s02e14.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Friends%20prom%20video%20episode"
            },
            new Episode
            {
                Id = 6,
                Title = "The One Where No One's Ready",
                Season = 3,
                EpisodeNumber = 2,
                AirDate = new DateTime(1996, 9, 26),
                Description = "Ross struggles to get everyone ready for a museum event.",
                ImageUrl = "/img/friends/ep-s03e02.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Friends%20no%20ones%20ready%20episode"
            }
        );

        // Seed initial data for Locations (with media)
        modelBuilder.Entity<Location>().HasData(
            new Location
            {
                Id = 1,
                Name = "Central Perk",
                Type = "Coffee Shop",
                Address = "90 Bedford Street, New York",
                Description = "The iconic coffee house where the friends spend most of their time.",
                ImageUrl = "/img/friends/location-central-perk.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Central%20Perk%20set%20tour"
            },
            new Location
            {
                Id = 2,
                Name = "Monica's Apartment",
                Type = "Apartment",
                Address = "90 Bedford Street, Apt 20, New York",
                Description = "Monica's purple-walled apartment where most scenes take place.",
                ImageUrl = "/img/friends/location-monicas-apartment.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Monica%20Geller%20apartment%20tour"
            },
            new Location
            {
                Id = 3,
                Name = "Joey & Chandler's Apartment",
                Type = "Apartment",
                Address = "90 Bedford Street, Apt 19, New York",
                Description = "The apartment across the hall from Monica's, featuring the famous foosball table.",
                ImageUrl = "/img/friends/location-joey-chandler-apartment.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Joey%20and%20Chandler%20apartment%20tour"
            },
            new Location
            {
                Id = 4,
                Name = "Ross's Apartment",
                Type = "Apartment",
                Address = "New York",
                Description = "Ross's various apartments throughout the series.",
                ImageUrl = "/img/friends/location-ross-apartment.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Ross%20Geller%20apartment%20tour"
            },
            new Location
            {
                Id = 5,
                Name = "Museum of Prehistoric History",
                Type = "Workplace",
                Address = "New York",
                Description = "Where Ross works as a paleontologist.",
                ImageUrl = "/img/friends/location-museum.jpg",
                VideoUrl = "https://www.youtube.com/embed?listType=search&list=Friends%20museum%20set%20tour"
            }
        );

        // Seed initial data for Quotes
        modelBuilder.Entity<Quote>().HasData(
            new Quote { Id = 1, Text = "We were on a break!", CharacterId = 6, EpisodeId = 1, Context = "Ross's famous catchphrase about his relationship with Rachel" },
            new Quote { Id = 2, Text = "How you doin'?", CharacterId = 4, EpisodeId = 1, Context = "Joey's signature pickup line" },
            new Quote { Id = 3, Text = "Could this BE any more awkward?", CharacterId = 5, EpisodeId = 1, Context = "Chandler's sarcastic observation" },
            new Quote { Id = 4, Text = "Oh. My. God!", CharacterId = 2, EpisodeId = 2, Context = "Janice's iconic catchphrase, often imitated by others" },
            new Quote { Id = 5, Text = "See, he's her lobster!", CharacterId = 3, EpisodeId = 5, Context = "Phoebe explaining Ross and Rachel's relationship" },
            new Quote { Id = 6, Text = "PIVOT! PIVOT! PIVOT!", CharacterId = 6, EpisodeId = 6, Context = "Ross trying to move a couch up the stairs" }
        );
    }

    public override int SaveChanges()
    {
        ApplyAuditRules();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditRules();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditRules()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not Character
                && entry.Entity is not Episode
                && entry.Entity is not Location
                && entry.Entity is not Quote)
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Property("UpdatedAtUtc").CurrentValue = now;
                entry.Property("IsDeleted").CurrentValue = false;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAtUtc").CurrentValue = now;
            }
            else if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Property("IsDeleted").CurrentValue = true;
                entry.Property("UpdatedAtUtc").CurrentValue = now;
            }
        }
    }
}

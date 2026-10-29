using GeographyGame.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace GeographyGame.Backend.Data;

public class GeographyGameContext : DbContext
{
    public GeographyGameContext(DbContextOptions<GeographyGameContext> options) : base(options) { }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<User> Users => Set<User>();
    public DbSet<TravelPhoto> TravelPhotos => Set<TravelPhoto>();
    public DbSet<Match> Matches => Set<Match>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(c => c.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(c => c.FlagImgPath)
                .HasColumnName("flag_img_path")
                .HasMaxLength(255);

            entity.Property(c => c.OnMapImgPath)
                .HasColumnName("on_map_img_path")
                .HasMaxLength(255);

            entity.Property(c => c.OutlineImgPath)
                .HasColumnName("outline_img_path")
                .HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(u => u.CountryId)
                .HasColumnName("country_id");

            entity.Property(u => u.Login)
                .HasColumnName("login")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(u => u.PasswordHash)
                .HasColumnName("password")
                .HasMaxLength(32)
                .IsRequired();

            entity.Property(u => u.RegistrationDate)
                .HasColumnName("registration_date")
                .HasColumnType("date");

            entity.Property(u => u.Matches)
                .HasColumnName("matches");

            entity.Property(u => u.Wins)
                .HasColumnName("wins");

            entity.HasOne<Country>()
                .WithMany()
                .HasForeignKey(u => u.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TravelPhoto>(entity =>
        {
            entity.ToTable("TravelPhoto");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(p => p.UserId)
                .HasColumnName("user_id");

            entity.Property(p => p.CountryId)
                .HasColumnName("country_id");

            entity.Property(p => p.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(p => p.Details)
                .HasColumnName("details")
                .HasMaxLength(255);

            entity.Property(p => p.CreationDate)
                .HasColumnName("creation_date")
                .HasColumnType("date");

            entity.Property(p => p.EditDate)
                .HasColumnName("edit_date")
                .HasColumnType("date");

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Country>()
                .WithMany()
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("Match");

            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(m => m.Player1Id)
                .HasColumnName("player_1_id")
                .IsRequired();

            entity.Property(m => m.Player2Id)
                .HasColumnName("player_2_id")
                .IsRequired();

            entity.Property(m => m.Player1Score)
                .HasColumnName("player_1_score")
                .IsRequired();

            entity.Property(m => m.Player2Score)
                .HasColumnName("player_2_score")
                .IsRequired();

            entity.Property(m => m.WinnerId)
                .HasColumnName("winner_id");

            entity.Property(m => m.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(m => m.StartedAt)
                .HasColumnName("started_at")
                .HasColumnType("date");

            entity.Property(m => m.FinishedAt)
                .HasColumnName("finished_at")
                .HasColumnType("date");

            entity.Property(m => m.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
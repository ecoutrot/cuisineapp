
using Cuisine.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cuisine.Infrastructure.Persistence.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)

{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Recipe> Recipes { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<IngredientCategory> IngredientCategories { get; set; } = null!;
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; } = null!;
    public DbSet<RecipeCategory> RecipeCategories { get; set; } = null!;
    public DbSet<Unit> Units { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeIngredient>()
            .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.RecipeIngredients)
            .WithOne(ri => ri.Recipe)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.RecipeCategory)
            .WithMany(rc => rc.Recipes)
            .HasForeignKey(r => r.RecipeCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.User)
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Unit)
            .WithMany(u => u.RecipeIngredients)
            .HasForeignKey(ri => ri.UnitId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Ingredient>()
            .HasOne(i => i.IngredientCategory)
            .WithMany(ic => ic.Ingredients)
            .HasForeignKey(i => i.IngredientCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        
        /* var user = new User();
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = Guid.NewGuid(),
            Username = "Edwin",
            PasswordHash = new PasswordHasher<User>().HashPassword(user, "edwin"),
            Role = "Admin"
        }); */
    }
}

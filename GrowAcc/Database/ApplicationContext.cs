using GrowAcc.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    // Додайте DbSet'и для ваших моделей
    public DbSet<UserAccount> YourModels { get; set; }
}

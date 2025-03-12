using Microsoft.EntityFrameworkCore;
using Notes.Context.Entities;

namespace Notes.Context;

public static class UsersContextConfiguration
{
    public static void ConfigureUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Uid).IsUnique();

            entity.HasMany(e => e.NotesDatas)
                      .WithOne(n => n.User)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
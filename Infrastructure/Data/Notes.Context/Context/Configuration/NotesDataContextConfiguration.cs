using Microsoft.EntityFrameworkCore;
using Notes.Context.Entities;

namespace Notes.Context;

public static class NotesDataContextConfiguration
{
    public static void ConfigureNotesDatas(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NoteDataEntity>(entity =>
        {
            entity.ToTable("notes_datas");

            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Uid).IsUnique();

            entity.Property(e => e.Name)
                      .IsRequired();
            entity.Property(e => e.Text)
                      .HasColumnType("text");
            entity.Property(e => e.DateСhange)
                  .IsRequired();
            entity.Property(e => e.Marked)
                  .IsRequired();
            entity.Property(e => e.UserId)
                  .IsRequired();

            entity.HasOne(e => e.User)
                      .WithMany(u => u.NotesDatas)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Photos)
                      .WithOne(p => p.NoteData)
                      .HasForeignKey(p => p.NoteDataId)
                      .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
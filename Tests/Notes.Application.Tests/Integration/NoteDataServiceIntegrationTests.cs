using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notes.Context;
using Notes.Context.Entities;
using Notes.Models;
using Notes.Services;
using Notes.Services.Logger;
using Notes.Services.NoteData;

namespace Notes.Tests.Application.Integration;

[TestClass]
public class NoteDataServiceIntegrationTests
{
    private ServiceProvider _serviceProvider;
    private MainDbContext _context;
    private INoteDataService _noteDataService;
    private Mock<IAppLogger> _mockLogger;

    [TestInitialize]
    public void Setup()
    {
        var services = new ServiceCollection();
        // Используем уникальное имя in‑memory базы для изоляции теста
        string dbName = "NoteDataIntegrationDb_" + Guid.NewGuid();
        services.AddDbContext<MainDbContext>(options =>
            options.UseInMemoryDatabase(dbName));
        services.AddDbContextFactory<MainDbContext>(options =>
            options.UseInMemoryDatabase(dbName));

        // Регистрируем AutoMapper с нужными профилями
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<NoteDataCreateProfile>();
            cfg.AddProfile<NoteDataProfile>();
            cfg.AddProfile<NoteDataUpdateProfile>();
        });
        services.AddSingleton(mapperConfig.CreateMapper());

        // Регистрируем IAppLogger как мок
        _mockLogger = new Mock<IAppLogger>();
        services.AddSingleton<IAppLogger>(_mockLogger.Object);

        // Регистрируем сервис заметок
        services.AddTransient<INoteDataService, NoteDataService>();

        _serviceProvider = services.BuildServiceProvider();

        // Получаем необходимые зависимости
        _context = _serviceProvider.GetRequiredService<MainDbContext>();
        _noteDataService = _serviceProvider.GetRequiredService<INoteDataService>();
    }

    [TestMethod]
    public async Task CreateNoteAsync_CreatesNote_ReturnsNoteDataModel()
    {
        // Arrange
        var createModel = new NoteDataCreateModel
        {
            Title = "Integration Test Note",
            Text = "This is a test note.",
            Marked = false,
            UserId = Guid.NewGuid()
        };

        // Act
        var result = await _noteDataService.CreateNoteAsync(createModel);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(createModel.Title, result.Title);
        Assert.AreEqual(createModel.Text, result.Text);
        Assert.AreEqual(createModel.Marked, result.Marked);
        Assert.AreEqual(createModel.UserId, result.UserId);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Created note")),
            It.IsAny<object[]>()), Times.Once);

        // Для проверки создаем новый контекст через фабрику, чтобы получить актуальное состояние базы
        using var verificationContext = _serviceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>()
            .CreateDbContext();
        var noteInDb = await verificationContext.NotesDatas.FindAsync(result.Uid);
        Assert.IsNotNull(noteInDb);
    }

    [TestMethod]
    public async Task GetNotesByUserIdAsync_ReturnsNotes_ForExistingUser()
    {
        // Arrange: засеваем несколько заметок для определённого пользователя
        var userId = Guid.NewGuid();
        _context.NotesDatas.AddRange(
            new NoteDataEntity { Uid = Guid.NewGuid(), Title = "Note 1", Text = "Text 1", Marked = false, UserId = userId, DateСhange = DateTime.Now },
            new NoteDataEntity { Uid = Guid.NewGuid(), Title = "Note 2", Text = "Text 2", Marked = true, UserId = userId, DateСhange = DateTime.Now },
            new NoteDataEntity { Uid = Guid.NewGuid(), Title = "Other Note", Text = "Text 3", Marked = false, UserId = Guid.NewGuid(), DateСhange = DateTime.Now }
        );
        await _context.SaveChangesAsync();

        // Act
        var notes = await _noteDataService.GetNotesByUserIdAsync(userId);

        // Assert
        Assert.AreEqual(2, notes.Count());
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Retrieved") && s.Contains("notes")),
            It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task GetNoteByIdAsync_ReturnsNote_WhenNoteExists()
    {
        // Arrange: создаем заметку
        var note = new NoteDataEntity
        {
            Uid = Guid.NewGuid(),
            Title = "Detail Note",
            Text = "Detailed text",
            Marked = false,
            UserId = Guid.NewGuid(),
            DateСhange = DateTime.Now
        };
        _context.NotesDatas.Add(note);
        await _context.SaveChangesAsync();

        // Act
        var result = await _noteDataService.GetNoteByIdAsync(note.Uid);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(note.Title, result.Title);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Retrieved note")),
            It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task GetNoteByIdAsync_ReturnsNull_AndLogsWarning_WhenNotFound()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _noteDataService.GetNoteByIdAsync(nonExistingId);

        // Assert
        Assert.IsNull(result);
        _mockLogger.Verify(x => x.Warning(
            It.Is<string>(s => s.Contains("not found")),
            It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateNoteAsync_UpdatesNote_WhenNoteExists()
    {
        // Arrange: создаем заметку для обновления
        var note = new NoteDataEntity
        {
            Uid = Guid.NewGuid(),
            Title = "Original Title",
            Text = "Original text",
            Marked = false,
            UserId = Guid.NewGuid(),
            DateСhange = DateTime.Now
        };
        _context.NotesDatas.Add(note);
        await _context.SaveChangesAsync();

        var updateModel = new NoteDataUpdateModel
        {
            Title = "Updated Title",
            Text = "Updated text",
            Marked = true
        };

        // Act
        await _noteDataService.UpdateNoteAsync(note.Uid, updateModel);

        // Assert: создаем новый контекст для проверки изменений
        using var verificationContext = _serviceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>()
            .CreateDbContext();
        var updatedNote = await verificationContext.NotesDatas.FindAsync(note.Uid);
        Assert.IsNotNull(updatedNote);
        Assert.AreEqual(updateModel.Title, updatedNote.Title);
        Assert.AreEqual(updateModel.Text, updatedNote.Text);
        Assert.AreEqual(updateModel.Marked, updatedNote.Marked);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Updated note")),
            It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteNoteAsync_DeletesNote_WhenNoteExists()
    {
        // Arrange: создаем заметку для удаления
        var note = new NoteDataEntity
        {
            Uid = Guid.NewGuid(),
            Title = "Note to Delete",
            Text = "Some text",
            Marked = false,
            UserId = Guid.NewGuid(),
            DateСhange = DateTime.Now
        };
        _context.NotesDatas.Add(note);
        await _context.SaveChangesAsync();

        // Act
        await _noteDataService.DeleteNoteAsync(note.Uid);

        // Assert: создаем новый контекст для проверки, что заметка удалена
        using var verificationContext = _serviceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>()
            .CreateDbContext();
        var deletedNote = await verificationContext.NotesDatas.FindAsync(note.Uid);
        Assert.IsNull(deletedNote);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Deleted note")),
            It.IsAny<object[]>()), Times.Once);
    }
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notes.Context;
using Notes.Context.Entities;
using Notes.Models;
using Notes.Services;
using Notes.Services.Logger;
using Notes.Services.NoteData;

namespace Notes.Tests.Application.Unit;

[TestClass]
public class NoteDataServiceUnitTests
{
    private DbContextOptions<MainDbContext> _options;
    private IDbContextFactory<MainDbContext> _dbContextFactory;
    private IMapper _mapper;
    private Mock<IAppLogger> _mockLogger;
    private INoteDataService _noteDataService;
    private MainDbContext _seedContext;

    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase("NoteDataUnitTestDb_" + Guid.NewGuid())
            .Options;

        _seedContext = new MainDbContext(_options);
        
        var userId = Guid.NewGuid();
        _seedContext.NotesDatas.AddRange(
            new NoteDataEntity { Uid = Guid.NewGuid(), Title = "Unit Note 1", Text = "Text 1", Marked = false, UserId = userId, DateСhange = DateTime.Now },
            new NoteDataEntity { Uid = Guid.NewGuid(), Title = "Unit Note 2", Text = "Text 2", Marked = true, UserId = userId, DateСhange = DateTime.Now },
            new NoteDataEntity { Uid = Guid.NewGuid(), Title = "Other Note", Text = "Text 3", Marked = false, UserId = Guid.NewGuid(), DateСhange = DateTime.Now }
        );
        _seedContext.SaveChanges();

        var mockFactory = new Mock<IDbContextFactory<MainDbContext>>();
        mockFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new MainDbContext(_options));
        _dbContextFactory = mockFactory.Object;

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<NoteDataCreateProfile>();
            cfg.AddProfile<NoteDataProfile>();
            cfg.AddProfile<NoteDataUpdateProfile>();
        });

        _mapper = mapperConfig.CreateMapper();
        _mockLogger = new Mock<IAppLogger>();
        _noteDataService = new NoteDataService(_dbContextFactory, _mapper, _mockLogger.Object);
    }

    [TestMethod]
    public async Task GetNotesByUserIdAsync_ReturnsCorrectNotes()
    {
        // Arrange
        var userId = _seedContext.NotesDatas.First(n => n.Title == "Unit Note 1").UserId;

        // Act
        var notes = await _noteDataService.GetNotesByUserIdAsync(userId);

        // Assert
        var expectedCount = _seedContext.NotesDatas.Count(n => n.UserId == userId);
        Assert.AreEqual(expectedCount, notes.Count());
        _mockLogger.Verify(x => x.Information(It.Is<string>(s => s.Contains("Retrieved")), It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task GetNoteByIdAsync_ReturnsNote_WhenExists()
    {
        // Arrange
        var note = _seedContext.NotesDatas.First(n => n.Title == "Unit Note 1");

        // Act
        var result = await _noteDataService.GetNoteByIdAsync(note.Uid);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(note.Title, result.Title);
        _mockLogger.Verify(x => x.Information(It.Is<string>(s => s.Contains("Retrieved note")), It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task GetNoteByIdAsync_ReturnsNull_AndLogsWarning_WhenNotExists()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _noteDataService.GetNoteByIdAsync(nonExistingId);

        // Assert
        Assert.IsNull(result);
        _mockLogger.Verify(x => x.Warning(It.Is<string>(s => s.Contains("not found")), It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateNoteAsync_CreatesNote()
    {
        // Arrange
        var createModel = new NoteDataCreateModel
        {
            Title = "New Unit Note",
            Text = "New text",
            Marked = false,
            UserId = Guid.NewGuid()
        };

        // Act
        var result = await _noteDataService.CreateNoteAsync(createModel);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(createModel.Title, result.Title);
        _mockLogger.Verify(x => x.Information(It.Is<string>(s => s.Contains("Created note")), It.IsAny<object[]>()), Times.Once);

        using var verificationContext = new MainDbContext(_options);
        var noteInDb = await verificationContext.NotesDatas.FindAsync(result.Uid);
        Assert.IsNotNull(noteInDb);
    }

    [TestMethod]
    public async Task UpdateNoteAsync_UpdatesNote()
    {
        // Arrange
        var note = _seedContext.NotesDatas.First(n => n.Title == "Unit Note 1");
        var updateModel = new NoteDataUpdateModel
        {
            Title = "Updated Unit Note",
            Text = "Updated text",
            Marked = true
        };

        // Act
        await _noteDataService.UpdateNoteAsync(note.Uid, updateModel);

        // Assert
        using var verificationContext = new MainDbContext(_options);
        var updatedNote = await verificationContext.NotesDatas.FindAsync(note.Uid);
        Assert.IsNotNull(updatedNote);
        Assert.AreEqual(updateModel.Title, updatedNote.Title);
        Assert.AreEqual(updateModel.Text, updatedNote.Text);
        Assert.AreEqual(updateModel.Marked, updatedNote.Marked);
        _mockLogger.Verify(x => x.Information(It.Is<string>(s => s.Contains("Updated note")), It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteNoteAsync_DeletesNote()
    {
        // Arrange
        var note = _seedContext.NotesDatas.First(n => n.Title == "Unit Note 1");

        // Act
        await _noteDataService.DeleteNoteAsync(note.Uid);

        // Assert
        using var verificationContext = new MainDbContext(_options);
        var deletedNote = await verificationContext.NotesDatas.FindAsync(note.Uid);
        Assert.IsNull(deletedNote);
        _mockLogger.Verify(x => x.Information(It.Is<string>(s => s.Contains("Deleted note")), It.IsAny<object[]>()), Times.Once);
    }
}

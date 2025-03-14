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
using Notes.Services.Photo;
using Notes.Services.Logger;

namespace Notes.Tests.Application.Unit;

[TestClass]
public class PhotoServiceUnitTests
{
    private DbContextOptions<MainDbContext> _options;
    private IDbContextFactory<MainDbContext> _dbContextFactory;
    private IMapper _mapper;
    private Mock<IAppLogger> _mockLogger;
    private IPhotoService _photoService;
    private MainDbContext _seedContext;

    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase("PhotoServiceUnitTestDb_" + Guid.NewGuid())
            .Options;

        _seedContext = new MainDbContext(_options);
        // Засеиваем несколько фото
        var noteId = Guid.NewGuid();
        _seedContext.Photos.AddRange(
            new PhotoEntity { Uid = Guid.NewGuid(), NoteDataId = noteId, Url = "http://example.com/unit_photo1.jpg" },
            new PhotoEntity { Uid = Guid.NewGuid(), NoteDataId = noteId, Url = "http://example.com/unit_photo2.jpg" },
            new PhotoEntity { Uid = Guid.NewGuid(), NoteDataId = Guid.NewGuid(), Url = "http://example.com/unit_photo3.jpg" }
        );
        _seedContext.SaveChanges();

        var mockFactory = new Mock<IDbContextFactory<MainDbContext>>();
        mockFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new MainDbContext(_options));
        _dbContextFactory = mockFactory.Object;

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoCreateModel, PhotoEntity>();
            cfg.CreateMap<PhotoEntity, PhotoModel>();
        });
        _mapper = mapperConfig.CreateMapper();

        _mockLogger = new Mock<IAppLogger>();

        _photoService = new PhotoService(_dbContextFactory, _mapper, _mockLogger.Object);
    }

    [TestMethod]
    public async Task CreatePhotoAsync_CreatesPhoto_ReturnsPhotoModel()
    {
        // Arrange
        var noteId = Guid.NewGuid();
        var createModel = new PhotoCreateModel
        {
            NoteDataId = noteId,
            Url = "http://example.com/unit_new_photo.jpg"
        };

        // Act
        var result = await _photoService.CreatePhotoAsync(createModel);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(createModel.Url, result.Url);
        Assert.AreEqual(createModel.NoteDataId, result.NoteDataId);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Created photo")),
            It.IsAny<object[]>()), Times.Once);

        using var verificationContext = new MainDbContext(_options);
        var photoInDb = await verificationContext.Photos.FindAsync(result.Uid);
        Assert.IsNotNull(photoInDb);
    }

    [TestMethod]
    public async Task GetPhotosByNoteIdAsync_ReturnsCorrectPhotos()
    {
        // Arrange
        var noteId = _seedContext.Photos.First().NoteDataId;

        // Act
        var photos = await _photoService.GetPhotosByNoteIdAsync(noteId);

        // Assert
        var expectedCount = _seedContext.Photos.Count(p => p.NoteDataId == noteId);
        Assert.AreEqual(expectedCount, photos.Count());
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Retrieved") && s.Contains("photos")),
            It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task GetPhotoByIdAsync_ReturnsPhoto_WhenPhotoExists()
    {
        // Arrange
        var photo = _seedContext.Photos.First();
        // Act
        var result = await _photoService.GetPhotoByIdAsync(photo.Uid);
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(photo.Uid, result.Uid);
        Assert.AreEqual(photo.Url, result.Url);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Retrieved photo")),
            It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task GetPhotoByIdAsync_ReturnsNull_AndLogsWarning_WhenPhotoNotFound()
    {
        // Arrange
        var nonExistingPhotoId = Guid.NewGuid();
        // Act
        var result = await _photoService.GetPhotoByIdAsync(nonExistingPhotoId);
        // Assert
        Assert.IsNull(result);
        _mockLogger.Verify(x => x.Warning(
            It.Is<string>(s => s.Contains("Photo with id") && s.Contains("not found")),
            It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task DeletePhotoAsync_RemovesPhoto_WhenPhotoExists()
    {
        // Arrange
        var contextForDelete = new MainDbContext(_options);
        var photo = new PhotoEntity { Uid = Guid.NewGuid(), NoteDataId = Guid.NewGuid(), Url = "http://example.com/unit_delete_photo.jpg" };
        contextForDelete.Photos.Add(photo);
        await contextForDelete.SaveChangesAsync();

        // Act
        await _photoService.DeletePhotoAsync(photo.Uid);

        // Assert
        using var verificationContext = new MainDbContext(_options);
        var deletedPhoto = await verificationContext.Photos.FindAsync(photo.Uid);
        Assert.IsNull(deletedPhoto);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Deleted photo")),
            It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task DeletePhotoAsync_LogsWarning_WhenPhotoNotFound()
    {
        // Arrange
        var nonExistingPhotoId = Guid.NewGuid();

        // Act
        await _photoService.DeletePhotoAsync(nonExistingPhotoId);

        // Assert
        _mockLogger.Verify(x => x.Warning(
            It.Is<string>(s => s.Contains("not found")),
            It.IsAny<object[]>()), Times.Once);
    }
}

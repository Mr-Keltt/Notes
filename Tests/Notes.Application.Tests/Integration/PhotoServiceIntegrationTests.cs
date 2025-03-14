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
using Notes.Services.Logger;
using Notes.Services.Photo;

namespace Notes.Tests.Application.Integration;

[TestClass]
public class PhotoServiceIntegrationTests
{
    private ServiceProvider _serviceProvider;
    private MainDbContext _context;
    private IPhotoService _photoService;
    private Mock<IAppLogger> _mockLogger;

    [TestInitialize]
    public void Setup()
    {
        var services = new ServiceCollection();
        // Регистрируем EF Core с InMemory-провайдером с уникальным именем базы
        string dbName = "PhotoServiceIntegrationDb_" + Guid.NewGuid();
        services.AddDbContext<MainDbContext>(options =>
            options.UseInMemoryDatabase(dbName));
        services.AddDbContextFactory<MainDbContext>(options =>
            options.UseInMemoryDatabase(dbName));

        // Настраиваем AutoMapper с профилями для фото
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoCreateModel, PhotoEntity>();
            cfg.CreateMap<PhotoEntity, PhotoModel>();
        });
        services.AddSingleton(mapperConfig.CreateMapper());

        _mockLogger = new Mock<IAppLogger>();
        services.AddSingleton<IAppLogger>(_mockLogger.Object);

        services.AddTransient<IPhotoService, PhotoService>();

        _serviceProvider = services.BuildServiceProvider();

        _context = _serviceProvider.GetRequiredService<MainDbContext>();
        _photoService = _serviceProvider.GetRequiredService<IPhotoService>();
    }

    [TestMethod]
    public async Task CreatePhotoAsync_CreatesPhoto_ReturnsPhotoModel()
    {
        // Arrange
        var noteId = Guid.NewGuid();
        var createModel = new PhotoCreateModel
        {
            NoteDataId = noteId,
            Url = "http://example.com/photo.jpg"
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

        using var verificationContext = _serviceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>()
            .CreateDbContext();
        var photoInDb = await verificationContext.Photos.FindAsync(result.Uid);
        Assert.IsNotNull(photoInDb);
    }

    [TestMethod]
    public async Task GetPhotosByNoteIdAsync_ReturnsCorrectPhotos()
    {
        // Arrange
        var noteId = Guid.NewGuid();
        _context.Photos.AddRange(
            new PhotoEntity { Uid = Guid.NewGuid(), NoteDataId = noteId, Url = "http://example.com/photo1.jpg" },
            new PhotoEntity { Uid = Guid.NewGuid(), NoteDataId = noteId, Url = "http://example.com/photo2.jpg" },
            new PhotoEntity { Uid = Guid.NewGuid(), NoteDataId = Guid.NewGuid(), Url = "http://example.com/photo3.jpg" } // не для данного noteId
        );
        await _context.SaveChangesAsync();

        // Act
        var photos = await _photoService.GetPhotosByNoteIdAsync(noteId);

        // Assert
        Assert.AreEqual(2, photos.Count());
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Retrieved") && s.Contains("photos")),
            It.IsAny<object[]>()), Times.Once);
    }

    [TestMethod]
    public async Task GetPhotoByIdAsync_ReturnsPhoto_WhenPhotoExists()
    {
        // Arrange: добавляем фото в контекст
        var photo = new PhotoEntity
        {
            Uid = Guid.NewGuid(),
            NoteDataId = Guid.NewGuid(),
            Url = "http://example.com/getphoto.jpg"
        };
        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

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
        var photo = new PhotoEntity
        {
            Uid = Guid.NewGuid(),
            NoteDataId = Guid.NewGuid(),
            Url = "http://example.com/photo_delete.jpg"
        };
        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        // Act
        await _photoService.DeletePhotoAsync(photo.Uid);

        // Assert
        using var verificationContext = _serviceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>()
            .CreateDbContext();
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

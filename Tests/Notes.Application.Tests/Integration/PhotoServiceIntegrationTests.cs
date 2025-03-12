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
        // Регистрируем EF Core с InMemory-провайдером
        services.AddDbContext<MainDbContext>(options =>
            options.UseInMemoryDatabase("PhotoServiceIntegrationDb_" + Guid.NewGuid()));
        services.AddDbContextFactory<MainDbContext>(options =>
            options.UseInMemoryDatabase("PhotoServiceIntegrationDb_" + Guid.NewGuid()));

        // Настраиваем AutoMapper с необходимыми профилями
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoCreateModel, PhotoEntity>();
            cfg.CreateMap<PhotoEntity, PhotoModel>();
        });
        services.AddSingleton(mapperConfig.CreateMapper());

        // Регистрируем мок логгера
        _mockLogger = new Mock<IAppLogger>();
        services.AddSingleton<IAppLogger>(_mockLogger.Object);

        // Регистрируем PhotoService
        services.AddTransient<IPhotoService, PhotoService>();

        _serviceProvider = services.BuildServiceProvider();

        // Получаем контекст и сервис из DI
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

        // Проверяем, что фото появилось в базе (создаем новый контекст)
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
        // Добавляем несколько фото для одного заметки
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
    public async Task DeletePhotoAsync_RemovesPhoto_WhenPhotoExists()
    {
        // Arrange
        var photo = new PhotoEntity { Uid = Guid.NewGuid(), NoteDataId = Guid.NewGuid(), Url = "http://example.com/photo_delete.jpg" };
        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        // Act
        await _photoService.DeletePhotoAsync(photo.Uid);

        // Assert - создаем новый контекст для проверки
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

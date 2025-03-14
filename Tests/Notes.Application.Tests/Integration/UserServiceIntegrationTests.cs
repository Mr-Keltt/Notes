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
using Notes.Services.User;

namespace Notes.Tests.Application.Integration;

[TestClass]
public class UserServiceIntegrationTests
{
    private ServiceProvider _serviceProvider;
    private MainDbContext _context;
    private IUserService _userService;
    private Mock<IAppLogger> _mockLogger;

    [TestInitialize]
    public void Setup()
    {
        var services = new ServiceCollection();
        string dbName = "UserServiceIntegrationDb_" + Guid.NewGuid();
        services.AddDbContext<MainDbContext>(options =>
            options.UseInMemoryDatabase(dbName));
        services.AddDbContextFactory<MainDbContext>(options =>
            options.UseInMemoryDatabase(dbName));

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserEntity, UserModel>();
        });
        services.AddSingleton(mapperConfig.CreateMapper());

        _mockLogger = new Mock<IAppLogger>();
        services.AddSingleton<IAppLogger>(_mockLogger.Object);

        services.AddTransient<IUserService, UserService>();

        _serviceProvider = services.BuildServiceProvider();

        _context = _serviceProvider.GetRequiredService<MainDbContext>();
        _userService = _serviceProvider.GetRequiredService<IUserService>();

        SeedTestData();
    }

    private void SeedTestData()
    {
        _context.Users.RemoveRange(_context.Users);
        _context.SaveChanges();
        _context.Users.Add(new UserEntity { Uid = Guid.NewGuid(), NotesDatas = new System.Collections.Generic.List<NoteDataEntity>() });
        _context.Users.Add(new UserEntity { Uid = Guid.NewGuid(), NotesDatas = new System.Collections.Generic.List<NoteDataEntity>() });
        _context.SaveChanges();
    }

    [TestMethod]
    public async Task CreateAsync_CreatesUser_ReturnsUserModel()
    {
        // Act
        var newUser = await _userService.CreateAsync();

        // Assert
        Assert.IsNotNull(newUser);
        Assert.AreNotEqual(Guid.Empty, newUser.Uid);

        using var verificationContext = _serviceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>()
            .CreateDbContext();
        var userInDb = await verificationContext.Users.FindAsync(newUser.Uid);
        Assert.IsNotNull(userInDb);

        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Created new user")),
            It.IsAny<object[]>()),
            Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
    {
        // Arrange
        var userEntity = await _context.Users.FirstAsync();
        var userId = userEntity.Uid;

        // Act
        var userModel = await _userService.GetByIdAsync(userId);

        // Assert
        Assert.IsNotNull(userModel);
        Assert.AreEqual(userId, userModel!.Uid);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Retrieved user")),
            It.IsAny<object[]>()),
            Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsNull_AndLogsWarning_WhenUserNotFound()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var userModel = await _userService.GetByIdAsync(nonExistingId);

        // Assert
        Assert.IsNull(userModel);
        _mockLogger.Verify(x => x.Warning(
            It.Is<string>(s => s.Contains("not found")),
            It.IsAny<object[]>()),
            Times.Once);
    }

    [TestMethod]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        var expectedCount = await _context.Users.CountAsync();

        // Act
        var users = await _userService.GetAllAsync();

        // Assert
        Assert.AreEqual(expectedCount, users.Count());
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Retrieved")),
            It.IsAny<object[]>()),
            Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_RemovesUser_WhenUserExists()
    {
        // Arrange
        var userEntity = await _context.Users.FirstAsync();
        var userId = userEntity.Uid;

        // Act
        await _userService.DeleteAsync(userId);

        using var newContext = _serviceProvider
            .GetRequiredService<IDbContextFactory<MainDbContext>>()
            .CreateDbContext();
        var deletedUser = await newContext.Users.FindAsync(userId);

        // Assert
        Assert.IsNull(deletedUser);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Deleted user")),
            It.IsAny<object[]>()),
            Times.Once);
    }

}

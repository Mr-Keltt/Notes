using System;
using System.Collections.Generic;
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
using Notes.Services.User;

namespace Notes.Tests.Application.Unit;

[TestClass]
public class UserServiceUnitTests
{
    private DbContextOptions<MainDbContext> _options;
    private IDbContextFactory<MainDbContext> _dbContextFactory;
    private MainDbContext _initialContext;
    private IMapper _mapper;
    private Mock<IAppLogger> _mockLogger;
    private IUserService _userService;

    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_" + Guid.NewGuid())
            .Options;

        _initialContext = new MainDbContext(_options);

        var userData = new List<UserEntity>
        {
            new UserEntity { Uid = Guid.NewGuid() },
            new UserEntity { Uid = Guid.NewGuid() }
        };
        _initialContext.Users.AddRange(userData);
        _initialContext.SaveChanges();

        var mockFactory = new Mock<IDbContextFactory<MainDbContext>>();
        mockFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new MainDbContext(_options));
        _dbContextFactory = mockFactory.Object;

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserEntity, UserModel>();
        });
        _mapper = mapperConfig.CreateMapper();

        _mockLogger = new Mock<IAppLogger>();

        _userService = new UserService(_dbContextFactory, _mapper, _mockLogger.Object);
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsUser_WhenUserExists()
    {
        // Arrange
        var existingUser = _initialContext.Users.First();

        // Act
        var result = await _userService.GetByIdAsync(existingUser.Uid);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(existingUser.Uid, result!.Uid);
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
        var result = await _userService.GetByIdAsync(nonExistingId);

        // Assert
        Assert.IsNull(result);
        _mockLogger.Verify(x => x.Warning(
            It.Is<string>(s => s.Contains("not found")),
            It.IsAny<object[]>()),
            Times.Once);
    }

    [TestMethod]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        Assert.AreEqual(_initialContext.Users.Count(), result.Count());
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Retrieved")),
            It.IsAny<object[]>()),
            Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_RemovesUser_WhenUserExists()
    {
        // Arrange
        var userToDelete = _initialContext.Users.First();

        // Act
        await _userService.DeleteAsync(userToDelete.Uid);

        using var verificationContext = new MainDbContext(_options);
        var deletedUser = await verificationContext.Users.FindAsync(userToDelete.Uid);

        // Assert
        Assert.IsNull(deletedUser);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Deleted user")),
            It.IsAny<object[]>()),
            Times.Once);
    }
}

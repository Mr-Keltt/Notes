using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notes.Context;
using Notes.Context.Entities;
using Notes.Models;
using Notes.Services.Logger;
using Notes.Services.User;

namespace Notes.Tests.Application.Unit;

[TestClass]
public class UserServiceUnitTests
{
    private DbContextOptions<MainDbContext> _options;
    private IDbContextFactory<MainDbContext> _dbContextFactory;
    private IMapper _mapper;
    private Mock<IAppLogger> _mockLogger;
    private IUserService _userService;
    private MainDbContext _seedContext;

    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase("UserServiceUnitTestDb_" + Guid.NewGuid())
            .Options;

        _seedContext = new MainDbContext(_options);

        var user1 = new UserEntity { Uid = Guid.NewGuid(), NotesDatas = new System.Collections.Generic.List<NoteDataEntity>() };
        var user2 = new UserEntity { Uid = Guid.NewGuid(), NotesDatas = new System.Collections.Generic.List<NoteDataEntity>() };
        _seedContext.Users.AddRange(user1, user2);
        _seedContext.SaveChanges();

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
    public async Task CreateAsync_CreatesUser()
    {
        // Act
        var newUser = await _userService.CreateAsync();

        // Assert
        Assert.IsNotNull(newUser);
        Assert.AreNotEqual(Guid.Empty, newUser.Uid);

        using var verificationContext = new MainDbContext(_options);
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
        var existingUser = _seedContext.Users.First();
        var userId = existingUser.Uid;

        // Act
        var result = await _userService.GetByIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result!.Uid);
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
        Assert.AreEqual(_seedContext.Users.Count(), result.Count());
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Retrieved")),
            It.IsAny<object[]>()),
            Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_RemovesUser_WhenUserExists()
    {
        // Arrange
        var userToDelete = _seedContext.Users.First();
        var userId = userToDelete.Uid;

        // Act
        await _userService.DeleteAsync(userId);

        using var verificationContext = new MainDbContext(_options);
        var deletedUser = await verificationContext.Users.FindAsync(userId);

        // Assert
        Assert.IsNull(deletedUser);
        _mockLogger.Verify(x => x.Information(
            It.Is<string>(s => s.Contains("Deleted user")),
            It.IsAny<object[]>()),
            Times.Once);
    }
}
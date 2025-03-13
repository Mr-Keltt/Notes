using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Notes.API.Controllers;
using Notes.API.Models;
using Notes.Models;
using Notes.Services.User;

namespace Notes.Tests.Unit;

[TestClass]
public class UsersControllerUnitTests
{
    private Mock<IUserService> _userServiceMock;
    private Mock<IMapper> _mapperMock;
    private UsersController _controller;

    [TestInitialize]
    public void Setup()
    {
        _userServiceMock = new Mock<IUserService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new UsersController(_userServiceMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task GetUser_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userModel = new UserModel { Uid = userId, NotesDatas = new List<NoteDataModel>() };
        var response = new UserResponse { Uid = userId, NotesDatas = new List<NoteDataResponse>() };

        _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(userModel);
        _mapperMock.Setup(m => m.Map<UserResponse>(userModel)).Returns(response);

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedResponse = okResult.Value as UserResponse;
        Assert.IsNotNull(returnedResponse);
        Assert.AreEqual(response.Uid, returnedResponse.Uid);
    }

    [TestMethod]
    public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((UserModel)null);

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task GetAllUsers_ReturnsOkWithUsers()
    {
        // Arrange
        var users = new List<UserModel>
        {
            new UserModel { Uid = Guid.NewGuid(), NotesDatas = new List<NoteDataModel>() },
            new UserModel { Uid = Guid.NewGuid(), NotesDatas = new List<NoteDataModel>() }
        };
        var responses = users.Select(u => new UserResponse { Uid = u.Uid, NotesDatas = new List<NoteDataResponse>() }).ToList();

        _userServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<IEnumerable<UserResponse>>(users)).Returns(responses);

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var list = okResult.Value as IEnumerable<UserResponse>;
        Assert.IsNotNull(list);
        Assert.AreEqual(responses.Count, list.Count());
    }

    [TestMethod]
    public async Task DeleteUser_ReturnsNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userServiceMock.Setup(s => s.DeleteAsync(userId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
}

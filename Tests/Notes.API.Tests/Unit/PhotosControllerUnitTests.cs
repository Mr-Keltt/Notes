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
using Notes.Services.Photo;

namespace Notes.Tests.API.Unit;

[TestClass]
public class PhotosControllerUnitTests
{
    private Mock<IPhotoService> _photoServiceMock;
    private Mock<IMapper> _mapperMock;
    private PhotosController _controller;

    [TestInitialize]
    public void Setup()
    {
        _photoServiceMock = new Mock<IPhotoService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new PhotosController(_photoServiceMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task CreatePhoto_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var request = new PhotoCreateRequest
        {
            NoteDataId = Guid.NewGuid(),
            Url = "http://example.com/photo.jpg"
        };
        var createModel = new PhotoCreateModel
        {
            NoteDataId = request.NoteDataId,
            Url = request.Url
        };
        var photoModel = new PhotoModel
        {
            Uid = Guid.NewGuid(),
            NoteDataId = request.NoteDataId,
            Url = request.Url
        };
        var response = new PhotoResponse
        {
            Uid = photoModel.Uid,
            NoteDataId = photoModel.NoteDataId,
            Url = photoModel.Url
        };

        _mapperMock.Setup(m => m.Map<PhotoCreateModel>(request)).Returns(createModel);
        _photoServiceMock.Setup(s => s.CreatePhotoAsync(createModel)).ReturnsAsync(photoModel);
        _mapperMock.Setup(m => m.Map<PhotoResponse>(photoModel)).Returns(response);

        // Act
        var result = await _controller.CreatePhoto(request);

        // Assert
        var createdAtResult = result.Result as CreatedAtActionResult;
        Assert.IsNotNull(createdAtResult);
        Assert.AreEqual("GetPhotosByNoteId", createdAtResult.ActionName);
        var returnedResponse = createdAtResult.Value as PhotoResponse;
        Assert.IsNotNull(returnedResponse);
        Assert.AreEqual(response.Uid, returnedResponse.Uid);
    }

    [TestMethod]
    public async Task GetPhotosByNoteId_ReturnsOkWithPhotos()
    {
        // Arrange
        var noteId = Guid.NewGuid();
        var photoModels = new List<PhotoModel>
        {
            new PhotoModel { Uid = Guid.NewGuid(), NoteDataId = noteId, Url = "http://example.com/1.jpg" },
            new PhotoModel { Uid = Guid.NewGuid(), NoteDataId = noteId, Url = "http://example.com/2.jpg" }
        };
        var responses = photoModels.Select(pm => new PhotoResponse
        {
            Uid = pm.Uid,
            NoteDataId = pm.NoteDataId,
            Url = pm.Url
        }).ToList();

        _photoServiceMock.Setup(s => s.GetPhotosByNoteIdAsync(noteId)).ReturnsAsync(photoModels);
        _mapperMock.Setup(m => m.Map<IEnumerable<PhotoResponse>>(photoModels)).Returns(responses);

        // Act
        var result = await _controller.GetPhotosByNoteId(noteId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var list = okResult.Value as IEnumerable<PhotoResponse>;
        Assert.IsNotNull(list);
        Assert.AreEqual(responses.Count, list.Count());
    }

    [TestMethod]
    public async Task GetPhotoById_ReturnsOk_WhenPhotoExists()
    {
        // Arrange
        var photoId = Guid.NewGuid();
        var photoModel = new PhotoModel
        {
            Uid = photoId,
            NoteDataId = Guid.NewGuid(),
            Url = "http://example.com/photo_detail.jpg"
        };
        var response = new PhotoResponse
        {
            Uid = photoModel.Uid,
            NoteDataId = photoModel.NoteDataId,
            Url = photoModel.Url
        };

        _photoServiceMock.Setup(s => s.GetPhotoByIdAsync(photoId)).ReturnsAsync(photoModel);
        _mapperMock.Setup(m => m.Map<PhotoResponse>(photoModel)).Returns(response);

        // Act
        var result = await _controller.GetPhotoById(photoId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedResponse = okResult.Value as PhotoResponse;
        Assert.IsNotNull(returnedResponse);
        Assert.AreEqual(response.Uid, returnedResponse.Uid);
    }

    [TestMethod]
    public async Task GetPhotoById_ReturnsNotFound_WhenPhotoDoesNotExist()
    {
        // Arrange
        var nonExistingPhotoId = Guid.NewGuid();
        _photoServiceMock.Setup(s => s.GetPhotoByIdAsync(nonExistingPhotoId)).ReturnsAsync((PhotoModel)null);

        // Act
        var result = await _controller.GetPhotoById(nonExistingPhotoId);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task DeletePhoto_ReturnsNoContent()
    {
        // Arrange
        var photoId = Guid.NewGuid();
        _photoServiceMock.Setup(s => s.DeletePhotoAsync(photoId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletePhoto(photoId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
}

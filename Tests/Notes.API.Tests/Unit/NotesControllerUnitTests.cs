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
using Notes.Services.NoteData;

namespace Notes.Tests.Unit
{
    [TestClass]
    public class NotesControllerUnitTests
    {
        private Mock<INoteDataService> _noteDataServiceMock;
        private Mock<IMapper> _mapperMock;
        private NotesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _noteDataServiceMock = new Mock<INoteDataService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new NotesController(_noteDataServiceMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task GetNotesByUserId_ReturnsOkWithNotes()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var noteModels = new List<NoteDataModel>
            {
                new NoteDataModel { Uid = Guid.NewGuid(), Title = "Note1", Text = "Text1", DateСhange = DateTime.Now, Marked = false, UserId = userId, Photos = new List<PhotoModel>() },
                new NoteDataModel { Uid = Guid.NewGuid(), Title = "Note2", Text = "Text2", DateСhange = DateTime.Now, Marked = true, UserId = userId, Photos = new List<PhotoModel>() }
            };
            var responses = noteModels.Select(n => new NoteDataResponse
            {
                Uid = n.Uid,
                Title = n.Title,
                Text = n.Text,
                DateChange = n.DateСhange,
                Marked = n.Marked,
                UserId = n.UserId,
                Photos = new List<PhotoResponse>()
            }).ToList();

            _noteDataServiceMock.Setup(s => s.GetNotesByUserIdAsync(userId)).ReturnsAsync(noteModels);
            _mapperMock.Setup(m => m.Map<IEnumerable<NoteDataResponse>>(noteModels)).Returns(responses);

            // Act
            var result = await _controller.GetNotesByUserId(userId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var list = okResult.Value as IEnumerable<NoteDataResponse>;
            Assert.IsNotNull(list);
            Assert.AreEqual(responses.Count, list.Count());
        }

        [TestMethod]
        public async Task GetNote_ReturnsOk_WhenNoteExists()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var noteModel = new NoteDataModel
            {
                Uid = noteId,
                Title = "Detail Note",
                Text = "Detail Text",
                DateСhange = DateTime.Now,
                Marked = false,
                UserId = Guid.NewGuid(),
                Photos = new List<PhotoModel>()
            };
            var response = new NoteDataResponse
            {
                Uid = noteId,
                Title = "Detail Note",
                Text = "Detail Text",
                DateChange = noteModel.DateСhange,
                Marked = false,
                UserId = noteModel.UserId,
                Photos = new List<PhotoResponse>()
            };
            _noteDataServiceMock.Setup(s => s.GetNoteByIdAsync(noteId)).ReturnsAsync(noteModel);
            _mapperMock.Setup(m => m.Map<NoteDataResponse>(noteModel)).Returns(response);

            // Act
            var result = await _controller.GetNote(noteId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedResponse = okResult.Value as NoteDataResponse;
            Assert.IsNotNull(returnedResponse);
            Assert.AreEqual(response.Uid, returnedResponse.Uid);
        }

        [TestMethod]
        public async Task GetNote_ReturnsNotFound_WhenNoteDoesNotExist()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            _noteDataServiceMock.Setup(s => s.GetNoteByIdAsync(noteId)).ReturnsAsync((NoteDataModel)null);

            // Act
            var result = await _controller.GetNote(noteId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateNote_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var createRequest = new NoteDataCreateRequest
            {
                Title = "New Note",
                Text = "New Text",
                Marked = false,
                UserId = Guid.NewGuid()
            };
            var createModel = new NoteDataCreateModel
            {
                Title = createRequest.Title,
                Text = createRequest.Text,
                Marked = createRequest.Marked,
                UserId = createRequest.UserId
            };
            var noteModel = new NoteDataModel
            {
                Uid = Guid.NewGuid(),
                Title = createRequest.Title,
                Text = createRequest.Text,
                DateСhange = DateTime.Now,
                Marked = createRequest.Marked,
                UserId = createRequest.UserId,
                Photos = new List<PhotoModel>()
            };
            var response = new NoteDataResponse
            {
                Uid = noteModel.Uid,
                Title = noteModel.Title,
                Text = noteModel.Text,
                DateChange = noteModel.DateСhange,
                Marked = noteModel.Marked,
                UserId = noteModel.UserId,
                Photos = new List<PhotoResponse>()
            };

            _mapperMock.Setup(m => m.Map<NoteDataCreateModel>(createRequest)).Returns(createModel);
            _noteDataServiceMock.Setup(s => s.CreateNoteAsync(createModel)).ReturnsAsync(noteModel);
            _mapperMock.Setup(m => m.Map<NoteDataResponse>(noteModel)).Returns(response);

            // Act
            var result = await _controller.CreateNote(createRequest);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(_controller.GetNote), createdResult.ActionName);
            var returnedResponse = createdResult.Value as NoteDataResponse;
            Assert.IsNotNull(returnedResponse);
            Assert.AreEqual(response.Uid, returnedResponse.Uid);
        }

        [TestMethod]
        public async Task UpdateNote_ReturnsNoContent()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var updateRequest = new NoteDataUpdateRequest
            {
                Title = "Updated Note",
                Text = "Updated Text",
                Marked = true
            };
            var updateModel = new NoteDataUpdateModel
            {
                Title = updateRequest.Title,
                Text = updateRequest.Text,
                Marked = updateRequest.Marked
            };

            _mapperMock.Setup(m => m.Map<NoteDataUpdateModel>(updateRequest)).Returns(updateModel);
            _noteDataServiceMock.Setup(s => s.UpdateNoteAsync(noteId, updateModel)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateNote(noteId, updateRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteNote_ReturnsNoContent()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            _noteDataServiceMock.Setup(s => s.DeleteNoteAsync(noteId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteNote(noteId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}

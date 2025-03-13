using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notes.Context.Entities;
using Notes.Models;

namespace Notes.Tests.Application.Unit;

[TestClass]
public class NoteDataUpdateProfileTests
{
    private IMapper _mapper;

    [TestInitialize]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<NoteDataUpdateProfile>();
            cfg.AddProfile<PhotoProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public void NoteDataUpdateMapping_Should_MapPropertiesAndUpdateDateChange()
    {
        // Arrange
        var noteUpdate = new NoteDataUpdateModel
        {
            Title = "Updated Title",
            Text = "Updated text",
            Marked = false
        };

        // Act
        var noteEntity = _mapper.Map<NoteDataEntity>(noteUpdate);

        // Assert
        Assert.AreEqual(noteUpdate.Title, noteEntity.Title);
        Assert.AreEqual(noteUpdate.Text, noteEntity.Text);
        Assert.AreEqual(noteUpdate.Marked, noteEntity.Marked);
        // Сравниваем с DateTime.UtcNow
        Assert.IsTrue((DateTime.UtcNow - noteEntity.DateСhange).TotalSeconds < 2);
    }
}

[TestClass]
public class NoteDataProfileTests
{
    private IMapper _mapper;

    [TestInitialize]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<NoteDataProfile>();
            cfg.AddProfile<PhotoProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public void NoteDataMapping_Should_MapEntityToModel()
    {
        // Arrange
        var noteEntity = new NoteDataEntity
        {
            Uid = Guid.NewGuid(),
            Title = "Entity Title",
            Text = "Entity text",
            Marked = true,
            DateСhange = DateTime.Now.AddMinutes(-10),
            UserId = Guid.NewGuid(),
            Photos = new List<PhotoEntity>
            {
                new PhotoEntity { Uid = Guid.NewGuid(), NoteDataId = Guid.NewGuid(), Url = "http://example.com/photo.jpg" }
            }
        };

        // Act
        var noteModel = _mapper.Map<NoteDataModel>(noteEntity);

        // Assert
        Assert.AreEqual(noteEntity.Uid, noteModel.Uid);
        Assert.AreEqual(noteEntity.Title, noteModel.Title);
        Assert.AreEqual(noteEntity.Text, noteModel.Text);
        Assert.AreEqual(noteEntity.Marked, noteModel.Marked);
        Assert.AreEqual(noteEntity.DateСhange, noteModel.DateСhange);
        Assert.AreEqual(noteEntity.UserId, noteModel.UserId);
        Assert.IsNotNull(noteModel.Photos);
        Assert.AreEqual(noteEntity.Photos.Count, noteModel.Photos.Count);
    }
}

[TestClass]
public class PhotoCreateProfileTests
{
    private IMapper _mapper;

    [TestInitialize]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PhotoCreateProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public void PhotoCreateMapping_Should_MapProperties()
    {
        // Arrange
        var photoCreate = new PhotoCreateModel
        {
            NoteDataId = Guid.NewGuid(),
            Url = "http://example.com/photo.jpg"
        };

        // Act
        var photoEntity = _mapper.Map<PhotoEntity>(photoCreate);

        // Assert
        Assert.AreEqual(photoCreate.NoteDataId, photoEntity.NoteDataId);
        Assert.AreEqual(photoCreate.Url, photoEntity.Url);
    }
}

[TestClass]
public class PhotoProfileTests
{
    private IMapper _mapper;

    [TestInitialize]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PhotoProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public void PhotoMapping_Should_MapEntityToModel()
    {
        // Arrange
        var photoEntity = new PhotoEntity
        {
            Uid = Guid.NewGuid(),
            NoteDataId = Guid.NewGuid(),
            Url = "http://example.com/photo.jpg"
        };

        // Act
        var photoModel = _mapper.Map<PhotoModel>(photoEntity);

        // Assert
        Assert.AreEqual(photoEntity.Uid, photoModel.Uid);
        Assert.AreEqual(photoEntity.NoteDataId, photoModel.NoteDataId);
        Assert.AreEqual(photoEntity.Url, photoModel.Url);
    }
}

[TestClass]
public class UserProfileTests
{
    private IMapper _mapper;

    [TestInitialize]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserProfile>();
            cfg.AddProfile<NoteDataProfile>();
            cfg.AddProfile<PhotoProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public void UserMapping_Should_MapEntityToModel()
    {
        // Arrange
        var userEntity = new UserEntity
        {
            Uid = Guid.NewGuid(),
            NotesDatas = new List<NoteDataEntity>
            {
                new NoteDataEntity
                {
                    Uid = Guid.NewGuid(),
                    Title = "Note Title",
                    Text = "Note Text",
                    Marked = false,
                    DateСhange = DateTime.Now.AddHours(-1),
                    UserId = Guid.NewGuid(),
                    Photos = new List<PhotoEntity>
                    {
                        new PhotoEntity { Uid = Guid.NewGuid(), NoteDataId = Guid.NewGuid(), Url = "http://example.com/photo.jpg" }
                    }
                }
            }
        };

        // Act
        var userModel = _mapper.Map<UserModel>(userEntity);

        // Assert
        Assert.AreEqual(userEntity.Uid, userModel.Uid);
        Assert.IsNotNull(userModel.NotesDatas);
        Assert.AreEqual(userEntity.NotesDatas.Count, userModel.NotesDatas.Count);
    }
}

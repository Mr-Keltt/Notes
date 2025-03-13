using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Notes.API.Models;
using Notes.Models;

namespace Notes.Tests.API.Unit;

[TestClass]
public class MappingProfilesTests
{
    [TestMethod]
    public void NoteDataCreateRequestMapping_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<NoteDataCreateRequestProfile>();
        });
        config.AssertConfigurationIsValid();
        var mapper = config.CreateMapper();

        var request = new NoteDataCreateRequest
        {
            Title = "Test Title",
            Text = "Test Text",
            Marked = true,
            UserId = Guid.NewGuid()
        };

        var businessModel = mapper.Map<NoteDataCreateModel>(request);
        Assert.AreEqual(request.Title, businessModel.Title);
        Assert.AreEqual(request.Text, businessModel.Text);
        Assert.AreEqual(request.Marked, businessModel.Marked);
        Assert.AreEqual(request.UserId, businessModel.UserId);

        var reverse = mapper.Map<NoteDataCreateRequest>(businessModel);
        Assert.AreEqual(request.Title, reverse.Title);
        Assert.AreEqual(request.Text, reverse.Text);
        Assert.AreEqual(request.Marked, reverse.Marked);
        Assert.AreEqual(request.UserId, reverse.UserId);
    }

    [TestMethod]
    public void NoteDataResponseMapping_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<NoteDataResponseProfile>();
            cfg.AddProfile<PhotoResponseProfile>();
        });
        config.AssertConfigurationIsValid();
        var mapper = config.CreateMapper();

        var response = new NoteDataResponse
        {
            Uid = Guid.NewGuid(),
            Title = "Response Title",
            Text = "Response Text",
            DateChange = new DateTime(2023, 1, 1),
            Marked = true,
            UserId = Guid.NewGuid(),
            Photos = new List<PhotoResponse>
            {
                new PhotoResponse { Uid = Guid.NewGuid(), NoteDataId = Guid.NewGuid(), Url = "http://example.com/photo.jpg" }
            }
        };

        var businessModel = mapper.Map<NoteDataModel>(response);
        Assert.AreEqual(response.Uid, businessModel.Uid);
        Assert.AreEqual(response.Title, businessModel.Title);
        Assert.AreEqual(response.Text, businessModel.Text);
        Assert.AreEqual(response.DateChange, businessModel.DateСhange);
        Assert.AreEqual(response.Marked, businessModel.Marked);
        Assert.AreEqual(response.UserId, businessModel.UserId);
        Assert.AreEqual(response.Photos.Count, businessModel.Photos.Count);

        var reverse = mapper.Map<NoteDataResponse>(businessModel);
        Assert.AreEqual(response.Uid, reverse.Uid);
        Assert.AreEqual(response.Title, reverse.Title);
        Assert.AreEqual(response.Text, reverse.Text);
        Assert.AreEqual(response.DateChange, reverse.DateChange);
        Assert.AreEqual(response.Marked, reverse.Marked);
        Assert.AreEqual(response.UserId, reverse.UserId);
        Assert.AreEqual(response.Photos.Count, reverse.Photos.Count);
    }

    [TestMethod]
    public void NoteDataUpdateRequestMapping_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<NoteDataUpdateRequestProfile>();
        });
        config.AssertConfigurationIsValid();
        var mapper = config.CreateMapper();

        var request = new NoteDataUpdateRequest
        {
            Title = "Update Title",
            Text = "Update Text",
            Marked = false
        };

        var updateModel = mapper.Map<NoteDataUpdateModel>(request);
        Assert.AreEqual(request.Title, updateModel.Title);
        Assert.AreEqual(request.Text, updateModel.Text);
        Assert.AreEqual(request.Marked, updateModel.Marked);

        var reverse = mapper.Map<NoteDataUpdateRequest>(updateModel);
        Assert.AreEqual(request.Title, reverse.Title);
        Assert.AreEqual(request.Text, reverse.Text);
        Assert.AreEqual(request.Marked, reverse.Marked);
    }

    [TestMethod]
    public void PhotoCreateRequestMapping_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PhotoCreateRequestProfile>();
        });
        config.AssertConfigurationIsValid();
        var mapper = config.CreateMapper();

        var request = new PhotoCreateRequest
        {
            NoteDataId = Guid.NewGuid(),
            Url = "http://example.com/photo_create.jpg"
        };

        var businessModel = mapper.Map<PhotoCreateModel>(request);
        Assert.AreEqual(request.NoteDataId, businessModel.NoteDataId);
        Assert.AreEqual(request.Url, businessModel.Url);

        var reverse = mapper.Map<PhotoCreateRequest>(businessModel);
        Assert.AreEqual(request.NoteDataId, reverse.NoteDataId);
        Assert.AreEqual(request.Url, reverse.Url);
    }

    [TestMethod]
    public void PhotoResponseMapping_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PhotoResponseProfile>();
        });
        config.AssertConfigurationIsValid();
        var mapper = config.CreateMapper();

        var response = new PhotoResponse
        {
            Uid = Guid.NewGuid(),
            NoteDataId = Guid.NewGuid(),
            Url = "http://example.com/photo_response.jpg"
        };

        var businessModel = mapper.Map<PhotoModel>(response);
        Assert.AreEqual(response.Uid, businessModel.Uid);
        Assert.AreEqual(response.NoteDataId, businessModel.NoteDataId);
        Assert.AreEqual(response.Url, businessModel.Url);

        var reverse = mapper.Map<PhotoResponse>(businessModel);
        Assert.AreEqual(response.Uid, reverse.Uid);
        Assert.AreEqual(response.NoteDataId, reverse.NoteDataId);
        Assert.AreEqual(response.Url, reverse.Url);
    }

    [TestMethod]
    public void UserResponseMapping_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserResponseProfile>();
            cfg.AddProfile<NoteDataResponseProfile>();
            cfg.AddProfile<PhotoResponseProfile>();
        });
        config.AssertConfigurationIsValid();
        var mapper = config.CreateMapper();

        var response = new UserResponse
        {
            Uid = Guid.NewGuid(),
            NotesDatas = new List<NoteDataResponse>
            {
                new NoteDataResponse
                {
                    Uid = Guid.NewGuid(),
                    Title = "User Note",
                    Text = "User Note Text",
                    DateChange = new DateTime(2023, 1, 1),
                    Marked = true,
                    UserId = Guid.NewGuid(),
                    Photos = new List<PhotoResponse>()
                }
            }
        };

        var businessModel = mapper.Map<UserModel>(response);
        Assert.AreEqual(response.Uid, businessModel.Uid);
        Assert.AreEqual(response.NotesDatas.Count, businessModel.NotesDatas.Count);

        var reverse = mapper.Map<UserResponse>(businessModel);
        Assert.AreEqual(response.Uid, reverse.Uid);
        Assert.AreEqual(response.NotesDatas.Count, reverse.NotesDatas.Count);
    }
}

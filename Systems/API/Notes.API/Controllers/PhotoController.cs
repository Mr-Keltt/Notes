using Microsoft.AspNetCore.Mvc;
using Notes.API.Models;
using Notes.Services.Photo;
using AutoMapper;
using Notes.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotosController : ControllerBase
{
    private readonly IPhotoService _photoService;
    private readonly IMapper _mapper;

    public PhotosController(IPhotoService photoService, IMapper mapper)
    {
        _photoService = photoService;
        _mapper = mapper;
    }

    // POST: api/Photos
    [HttpPost]
    [ProducesResponseType(201)]
    public async Task<ActionResult<PhotoResponse>> CreatePhoto([FromBody] PhotoCreateRequest request)
    {
        var createModel = _mapper.Map<PhotoCreateModel>(request);
        var photoModel = await _photoService.CreatePhotoAsync(createModel);
        var response = _mapper.Map<PhotoResponse>(photoModel);
        // Возвращаем 201 с ссылкой на получение фото по noteId
        return CreatedAtAction(nameof(GetPhotosByNoteId), new { noteId = response.NoteDataId }, response);
    }

    // GET: api/Photos?noteId={noteId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PhotoResponse>>> GetPhotosByNoteId([FromQuery] Guid noteId)
    {
        var photoModels = await _photoService.GetPhotosByNoteIdAsync(noteId);
        var responses = _mapper.Map<IEnumerable<PhotoResponse>>(photoModels);
        return Ok(responses);
    }

    // GET: api/Photos/{photoId}
    [HttpGet("{photoId:guid}")]
    public async Task<ActionResult<PhotoResponse>> GetPhotoById(Guid photoId)
    {
        var photoModel = await _photoService.GetPhotoByIdAsync(photoId);
        if (photoModel == null)
        {
            return NotFound();
        }
        var response = _mapper.Map<PhotoResponse>(photoModel);
        return Ok(response);
    }

    // DELETE: api/Photos/{photoId}
    [HttpDelete("{photoId:guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeletePhoto(Guid photoId)
    {
        await _photoService.DeletePhotoAsync(photoId);
        return NoContent();
    }
}

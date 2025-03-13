using Microsoft.AspNetCore.Mvc;
using Notes.API.Models;
using Notes.Services.NoteData;
using AutoMapper;
using Notes.Models;

namespace Notes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INoteDataService _noteDataService;
    private readonly IMapper _mapper;

    public NotesController(INoteDataService noteDataService, IMapper mapper)
    {
        _noteDataService = noteDataService;
        _mapper = mapper;
    }

    // GET: api/Notes?userId={userId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NoteDataResponse>>> GetNotesByUserId([FromQuery] Guid userId)
    {
        var noteModels = await _noteDataService.GetNotesByUserIdAsync(userId);
        var responses = _mapper.Map<IEnumerable<NoteDataResponse>>(noteModels);
        return Ok(responses);
    }

    // GET: api/Notes/{noteId}
    [HttpGet("{noteId:guid}")]
    public async Task<ActionResult<NoteDataResponse>> GetNote(Guid noteId)
    {
        var noteModel = await _noteDataService.GetNoteByIdAsync(noteId);
        if (noteModel == null)
        {
            return NotFound();
        }
        var response = _mapper.Map<NoteDataResponse>(noteModel);
        return Ok(response);
    }

    // POST: api/Notes
    [HttpPost]
    public async Task<ActionResult<NoteDataResponse>> CreateNote([FromBody] NoteDataCreateRequest request)
    {
        var createModel = _mapper.Map<NoteDataCreateModel>(request);
        var noteModel = await _noteDataService.CreateNoteAsync(createModel);
        var response = _mapper.Map<NoteDataResponse>(noteModel);
        return CreatedAtAction(nameof(GetNote), new { noteId = response.Uid }, response);
    }

    // PUT: api/Notes/{noteId}
    [HttpPut("{noteId:guid}")]
    public async Task<IActionResult> UpdateNote(Guid noteId, [FromBody] NoteDataUpdateRequest request)
    {
        var updateModel = _mapper.Map<NoteDataUpdateModel>(request);
        await _noteDataService.UpdateNoteAsync(noteId, updateModel);
        return NoContent();
    }

    // DELETE: api/Notes/{noteId}
    [HttpDelete("{noteId:guid}")]
    public async Task<IActionResult> DeleteNote(Guid noteId)
    {
        await _noteDataService.DeleteNoteAsync(noteId);
        return NoContent();
    }
}

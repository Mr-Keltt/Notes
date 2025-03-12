using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services.NoteData;

public interface INoteDataService
{
    /// <summary>
    /// Get all the notes for a given user.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <returns>Collection of note models.</returns>
    Task<IEnumerable<NoteDataModel>> GetNotesByUserIdAsync(Guid userId);

    /// <summary>
    /// Get a note by its ID.
    /// </summary>
    /// <param name="noteId">The ID of the note.</param>
    /// <returns>The note model, or null if the note is not found.</returns>
    Task<NoteDataModel?> GetNoteByIdAsync(Guid noteId);

    /// <summary>
    /// Create a new note.
    /// </summary>
    /// <param name="model">The note creation model.</param>
    /// <returns>The created note is in the form of a model.</returns>
    Task<NoteDataModel> CreateNoteAsync(NoteDataCreateModel model);

    /// <summary>
    /// Update an existing note.
    /// </summary>
    /// <param name="noteId">The ID of the note.</param>
    /// <param name="model">The note update model.</param>
    Task UpdateNoteAsync(Guid noteId, NoteDataUpdateModel model);

    /// <summary>
    /// Delete a note by ID.
    /// </summary>
    /// <param name="noteId">ID of the note to delete.</param>
    Task DeleteNoteAsync(Guid noteId);
}

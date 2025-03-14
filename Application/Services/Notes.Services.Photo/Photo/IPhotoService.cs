using Notes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services.Photo;

public interface IPhotoService
{
    /// <summary>
    /// Creates a photo.
    /// </summary>
    /// <param name="model">A model for creating a photo.</param>
    /// <returns>The created photo model.</returns>
    Task<PhotoModel> CreatePhotoAsync(PhotoCreateModel model);

    /// <summary>
    /// Gets all the photos associated with the specified note.
    /// </summary>
    /// <param name="noteId">The ID of the note.</param>
    /// <returns>Collection of photo models.</returns>
    Task<IEnumerable<PhotoModel>> GetPhotosByNoteIdAsync(Guid noteId);

    /// <summary>
    /// Gets a photo by its ID.
    /// </summary>
    /// <param name="photoId">ID of the photo.</param>
    /// <returns>The photo model, or null if not found.</returns>
    Task<PhotoModel?> GetPhotoByIdAsync(Guid photoId);

    /// <summary>
    /// Deletes photos by ID.
    /// </summary>
    /// <param name="photoId">ID of the photo to delete.</param>
    Task DeletePhotoAsync(Guid photoId);
}

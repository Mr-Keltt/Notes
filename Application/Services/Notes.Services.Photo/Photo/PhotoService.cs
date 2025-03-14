using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Context;
using Notes.Context.Entities;
using Notes.Models;
using Notes.Services.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Services.Photo
{
    /// <summary>
    /// Provides methods for creating, retrieving, and deleting photo records, handling
    /// the mapping between photo entities and models, and logging the corresponding events.
    /// </summary>
    public class PhotoService : IPhotoService
    {
        // Factory to create instances of the main database context.
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        // Mapper to convert between data entities and business models.
        private readonly IMapper _mapper;
        // Logger to record information, warnings, and errors.
        private readonly IAppLogger _logger;

        /// <summary>
        /// Initializes a new instance of the PhotoService class with required dependencies.
        /// </summary>
        /// <param name="dbContextFactory">The factory used to create database context instances.</param>
        /// <param name="mapper">The AutoMapper instance for mapping between entities and models.</param>
        /// <param name="logger">The application logger for logging events.</param>
        public PhotoService(IDbContextFactory<MainDbContext> dbContextFactory, IMapper mapper, IAppLogger logger)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new photo record based on the provided photo creation model.
        /// </summary>
        /// <param name="model">A model containing the data required to create a new photo.</param>
        /// <returns>A task representing the asynchronous operation, containing the created photo model.</returns>
        public async Task<PhotoModel> CreatePhotoAsync(PhotoCreateModel model)
        {
            // Create a new database context instance.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Map the photo creation model to a photo entity.
            var entity = _mapper.Map<PhotoEntity>(model);
            // Add the new photo entity to the database.
            context.Photos.Add(entity);
            // Save changes asynchronously to persist the new photo record.
            await context.SaveChangesAsync();
            // Log the successful creation of the photo with its identifier and associated note identifier.
            _logger.Information("Created photo with id {PhotoId} for note {NoteId}", entity.Uid, model.NoteDataId);
            // Map and return the created photo entity as a photo model.
            return _mapper.Map<PhotoModel>(entity);
        }

        /// <summary>
        /// Retrieves all photo records associated with a specific note.
        /// </summary>
        /// <param name="noteId">The unique identifier of the note whose photos are to be retrieved.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a collection of photo models for the specified note.
        /// </returns>
        public async Task<IEnumerable<PhotoModel>> GetPhotosByNoteIdAsync(Guid noteId)
        {
            // Create a new database context instance.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Query for photo entities where the NoteDataId matches the provided noteId.
            var entities = await context.Photos.Where(p => p.NoteDataId == noteId).ToListAsync();
            // Log the retrieval operation including the count of photos found.
            _logger.Information("Retrieved {Count} photos for note {NoteId}", entities.Count, noteId);
            // Map and return the photo entities to the corresponding photo models.
            return _mapper.Map<IEnumerable<PhotoModel>>(entities);
        }

        /// <summary>
        /// Retrieves a single photo record based on its unique identifier.
        /// </summary>
        /// <param name="photoId">The unique identifier of the photo to be retrieved.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the photo model if found; otherwise, null.
        /// </returns>
        public async Task<PhotoModel?> GetPhotoByIdAsync(Guid photoId)
        {
            // Create a new database context instance.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Attempt to find the photo entity by its unique identifier.
            var entity = await context.Photos.FindAsync(photoId);
            // If the photo is not found, log a warning and return null.
            if (entity == null)
            {
                _logger.Warning("Photo with id {PhotoId} not found", photoId);
                return null;
            }
            // Log successful retrieval of the photo.
            _logger.Information("Retrieved photo with id {PhotoId}", photoId);
            // Map and return the photo entity as a photo model.
            return _mapper.Map<PhotoModel>(entity);
        }

        /// <summary>
        /// Deletes an existing photo record identified by its unique identifier.
        /// </summary>
        /// <param name="photoId">The unique identifier of the photo to delete.</param>
        /// <returns>A task representing the asynchronous deletion operation.</returns>
        public async Task DeletePhotoAsync(Guid photoId)
        {
            // Create a new database context instance.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Attempt to locate the photo entity by its unique identifier.
            var entity = await context.Photos.FindAsync(photoId);
            // If the photo entity is not found, log a warning and exit the method.
            if (entity == null)
            {
                _logger.Warning("Photo with id {PhotoId} not found", photoId);
                return;
            }
            // Remove the photo entity from the context.
            context.Photos.Remove(entity);
            // Persist the deletion to the database.
            await context.SaveChangesAsync();
            // Log the successful deletion of the photo.
            _logger.Information("Deleted photo with id {PhotoId}", photoId);
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Context.Entities;
using Notes.Context;
using Notes.Models;
using Notes.Services.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Services.NoteData
{
    /// <summary>
    /// Provides methods to perform CRUD operations on note data, including mapping between data entities and models,
    /// and logging key events during processing.
    /// </summary>
    public class NoteDataService : INoteDataService
    {
        // Factory for creating database context instances.
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        // Mapper for converting between data entities and business models.
        private readonly IMapper _mapper;
        // Logger instance for recording information, warnings, and errors.
        private readonly IAppLogger _logger;

        /// <summary>
        /// Initializes a new instance of the NoteDataService class with required dependencies.
        /// </summary>
        /// <param name="dbContextFactory">A factory to create instances of the main database context.</param>
        /// <param name="mapper">An instance of AutoMapper to map between entities and models.</param>
        /// <param name="logger">An application logger to log operational messages.</param>
        public NoteDataService(IDbContextFactory<MainDbContext> dbContextFactory, IMapper mapper, IAppLogger logger)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a collection of note models associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose notes are to be retrieved.</param>
        /// <returns>A collection of <see cref="NoteDataModel"/> instances representing the user's notes.</returns>
        public async Task<IEnumerable<NoteDataModel>> GetNotesByUserIdAsync(Guid userId)
        {
            // Create a new database context instance.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Query for note entities filtered by the specified user ID, including associated photos.
            var notes = await context.NotesDatas
                .Where(n => n.UserId == userId)
                .Include(n => n.Photos)
                .ToListAsync();

            // Log the retrieval operation including the count of notes found.
            _logger.Information("Retrieved {Count} notes for user {UserId}", notes.Count, userId);
            // Map and return the note entities to the corresponding note data models.
            return _mapper.Map<IEnumerable<NoteDataModel>>(notes);
        }

        /// <summary>
        /// Retrieves a single note model based on its unique identifier.
        /// </summary>
        /// <param name="noteId">The unique identifier of the note to be retrieved.</param>
        /// <returns>
        /// A <see cref="NoteDataModel"/> representing the note if found; otherwise, <c>null</c> if the note does not exist.
        /// </returns>
        public async Task<NoteDataModel?> GetNoteByIdAsync(Guid noteId)
        {
            // Create a new database context instance.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Query for the note entity with the specified note ID including its photos.
            var note = await context.NotesDatas
                .Include(n => n.Photos)
                .FirstOrDefaultAsync(n => n.Uid == noteId);

            // If no note is found, log a warning and return null.
            if (note == null)
            {
                _logger.Warning("Note with id {NoteId} not found", noteId);
                return null;
            }

            // Log successful retrieval and map the entity to a note data model.
            _logger.Information("Retrieved note with id {NoteId}", noteId);
            return _mapper.Map<NoteDataModel>(note);
        }

        /// <summary>
        /// Creates a new note based on the provided creation model.
        /// </summary>
        /// <param name="model">The note creation model containing the data for the new note.</param>
        /// <returns>A <see cref="NoteDataModel"/> representing the newly created note.</returns>
        public async Task<NoteDataModel> CreateNoteAsync(NoteDataCreateModel model)
        {
            // Create a new database context instance.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Map the creation model to a note entity.
            var noteEntity = _mapper.Map<NoteDataEntity>(model);
            // Add the new note entity to the database context.
            context.NotesDatas.Add(noteEntity);
            // Persist changes to the database.
            await context.SaveChangesAsync();
            // Log the creation event with note and user identifiers.
            _logger.Information("Created note with id {NoteId} for user {UserId}", noteEntity.Uid, model.UserId);
            // Map and return the created note entity as a note data model.
            return _mapper.Map<NoteDataModel>(noteEntity);
        }

        /// <summary>
        /// Updates an existing note identified by its unique identifier using the provided update model.
        /// </summary>
        /// <param name="noteId">The unique identifier of the note to update.</param>
        /// <param name="model">The note update model containing updated data.</param>
        public async Task UpdateNoteAsync(Guid noteId, NoteDataUpdateModel model)
        {
            // Create a new database context instance.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Retrieve the note entity including its photos.
            var noteEntity = await context.NotesDatas
                .Include(n => n.Photos)
                .FirstOrDefaultAsync(n => n.Uid == noteId);

            // If the note is not found, log a warning and exit.
            if (noteEntity == null)
            {
                _logger.Warning("Note with id {NoteId} not found for update", noteId);
                return;
            }

            // Map the update model onto the existing note entity.
            _mapper.Map(model, noteEntity);
            // Persist the updated note entity to the database.
            await context.SaveChangesAsync();
            // Log the successful update operation.
            _logger.Information("Updated note with id {NoteId}", noteId);
        }

        /// <summary>
        /// Deletes an existing note based on its unique identifier.
        /// </summary>
        /// <param name="noteId">The unique identifier of the note to delete.</param>
        public async Task DeleteNoteAsync(Guid noteId)
        {
            // Create a new database context instance.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Attempt to locate the note entity by its unique identifier.
            var noteEntity = await context.NotesDatas.FindAsync(noteId);

            // If the note entity is not found, log a warning and exit.
            if (noteEntity == null)
            {
                _logger.Warning("Note with id {NoteId} not found for deletion", noteId);
                return;
            }

            // Remove the note entity from the context.
            context.NotesDatas.Remove(noteEntity);
            // Persist the deletion to the database.
            await context.SaveChangesAsync();
            // Log the successful deletion operation.
            _logger.Information("Deleted note with id {NoteId}", noteId);
        }
    }
}

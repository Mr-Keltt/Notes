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
    public class PhotoService : IPhotoService
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly IMapper _mapper;
        private readonly IAppLogger _logger;

        public PhotoService(IDbContextFactory<MainDbContext> dbContextFactory, IMapper mapper, IAppLogger logger)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PhotoModel> CreatePhotoAsync(PhotoCreateModel model)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var entity = _mapper.Map<PhotoEntity>(model);
            context.Photos.Add(entity);
            await context.SaveChangesAsync();
            _logger.Information("Created photo with id {PhotoId} for note {NoteId}", entity.Uid, model.NoteDataId);
            return _mapper.Map<PhotoModel>(entity);
        }

        public async Task<IEnumerable<PhotoModel>> GetPhotosByNoteIdAsync(Guid noteId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var entities = await context.Photos.Where(p => p.NoteDataId == noteId).ToListAsync();
            _logger.Information("Retrieved {Count} photos for note {NoteId}", entities.Count, noteId);
            return _mapper.Map<IEnumerable<PhotoModel>>(entities);
        }

        public async Task<PhotoModel?> GetPhotoByIdAsync(Guid photoId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var entity = await context.Photos.FindAsync(photoId);
            if (entity == null)
            {
                _logger.Warning("Photo with id {PhotoId} not found", photoId);
                return null;
            }
            _logger.Information("Retrieved photo with id {PhotoId}", photoId);
            return _mapper.Map<PhotoModel>(entity);
        }

        public async Task DeletePhotoAsync(Guid photoId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var entity = await context.Photos.FindAsync(photoId);
            if (entity == null)
            {
                _logger.Warning("Photo with id {PhotoId} not found", photoId);
                return;
            }
            context.Photos.Remove(entity);
            await context.SaveChangesAsync();
            _logger.Information("Deleted photo with id {PhotoId}", photoId);
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Context.Entities;
using Notes.Context;
using Notes.Models;
using Notes.Services.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services.NoteData;

public class NoteDataService : INoteDataService
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly IAppLogger _logger;

    public NoteDataService(IDbContextFactory<MainDbContext> dbContextFactory, IMapper mapper, IAppLogger logger)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<NoteDataModel>> GetNotesByUserIdAsync(Guid userId)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        var notes = await context.NotesDatas
            .Where(n => n.UserId == userId)
            .Include(n => n.Photos)
            .ToListAsync();
        _logger.Information("Retrieved {Count} notes for user {UserId}", notes.Count, userId);
        return _mapper.Map<IEnumerable<NoteDataModel>>(notes);
    }

    public async Task<NoteDataModel?> GetNoteByIdAsync(Guid noteId)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        var note = await context.NotesDatas
            .Include(n => n.Photos)
            .FirstOrDefaultAsync(n => n.Uid == noteId);
        if (note == null)
        {
            _logger.Warning("Note with id {NoteId} not found", noteId);
            return null;
        }
        _logger.Information("Retrieved note with id {NoteId}", noteId);
        return _mapper.Map<NoteDataModel>(note);
    }

    public async Task<NoteDataModel> CreateNoteAsync(NoteDataCreateModel model)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        var noteEntity = _mapper.Map<NoteDataEntity>(model);
        context.NotesDatas.Add(noteEntity);
        await context.SaveChangesAsync();
        _logger.Information("Created note with id {NoteId} for user {UserId}", noteEntity.Uid, model.UserId);
        return _mapper.Map<NoteDataModel>(noteEntity);
    }


    public async Task UpdateNoteAsync(Guid noteId, NoteDataUpdateModel model)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        var noteEntity = await context.NotesDatas
            .Include(n => n.Photos)
            .FirstOrDefaultAsync(n => n.Uid == noteId);
        if (noteEntity == null)
        {
            _logger.Warning("Note with id {NoteId} not found for update", noteId);
            return;
        }
        
        _mapper.Map(model, noteEntity);
        await context.SaveChangesAsync();
        _logger.Information("Updated note with id {NoteId}", noteId);
    }

    public async Task DeleteNoteAsync(Guid noteId)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        var noteEntity = await context.NotesDatas.FindAsync(noteId);
        if (noteEntity == null)
        {
            _logger.Warning("Note with id {NoteId} not found for deletion", noteId);
            return;
        }
        context.NotesDatas.Remove(noteEntity);
        await context.SaveChangesAsync();
        _logger.Information("Deleted note with id {NoteId}", noteId);
    }
}
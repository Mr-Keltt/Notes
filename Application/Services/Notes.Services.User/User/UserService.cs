using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Context;
using Notes.Context.Entities;
using Notes.Models;
using Notes.Services.Logger;

namespace Notes.Services.User;

public class UserService : IUserService
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IMapper _mapper;
    private readonly IAppLogger _logger;

    public UserService(
        IDbContextFactory<MainDbContext> dbContextFactory,
        IMapper mapper,
        IAppLogger logger)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserModel> CreateAsync()
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();

        var userEntity = new UserEntity
        {
            NotesDatas = new List<NoteDataEntity>()
        };

        context.Users.Add(userEntity);
        await context.SaveChangesAsync();
        _logger.Information("Created new user with id {UserId}", userEntity.Uid);
        return _mapper.Map<UserModel>(userEntity);
    }

    public async Task<UserModel?> GetByIdAsync(Guid userId)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        var userEntity = await context.Users.FindAsync(userId);
        if (userEntity == null)
        {
            _logger.Warning("User with id {UserId} not found", userId);
            return null;
        }

        _logger.Information("Retrieved user with id {UserId}", userId);
        return _mapper.Map<UserModel>(userEntity);
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        var users = await context.Users.ToListAsync();
        _logger.Information("Retrieved {Count} users", users.Count);
        return _mapper.Map<IEnumerable<UserModel>>(users);
    }

    public async Task DeleteAsync(Guid userId)
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        var userEntity = await context.Users.FindAsync(userId);
        if (userEntity == null)
        {
            _logger.Warning("Attempted to delete user with id {UserId}, but user was not found", userId);
            return;
        }

        context.Users.Remove(userEntity);
        await context.SaveChangesAsync();
        _logger.Information("Deleted user with id {UserId}", userId);
    }
}

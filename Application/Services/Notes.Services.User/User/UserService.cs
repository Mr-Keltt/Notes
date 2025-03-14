using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Context;
using Notes.Context.Entities;
using Notes.Models;
using Notes.Services.Logger;

namespace Notes.Services.User
{
    /// <summary>
    /// Provides operations for managing users, including creation, retrieval, and deletion.
    /// </summary>
    public class UserService : IUserService
    {
        // Factory for creating instances of the main database context.
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        // AutoMapper instance for mapping between data entities and business models.
        private readonly IMapper _mapper;
        // Logger instance for recording operations and events.
        private readonly IAppLogger _logger;

        /// <summary>
        /// Initializes a new instance of the UserService class with required dependencies.
        /// </summary>
        /// <param name="dbContextFactory">The factory to create database context instances.</param>
        /// <param name="mapper">The AutoMapper instance for converting entities to models and vice versa.</param>
        /// <param name="logger">The application logger for tracking operational events.</param>
        public UserService(
            IDbContextFactory<MainDbContext> dbContextFactory,
            IMapper mapper,
            IAppLogger logger)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user and returns the corresponding user model.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing the created user model.</returns>
        public async Task<UserModel> CreateAsync()
        {
            // Create a new instance of the database context.
            using var context = await _dbContextFactory.CreateDbContextAsync();

            // Create a new user entity with an empty list of notes.
            var userEntity = new UserEntity
            {
                NotesDatas = new List<NoteDataEntity>()
            };

            // Add the new user entity to the database context.
            context.Users.Add(userEntity);
            // Save the changes to persist the new user record.
            await context.SaveChangesAsync();

            // Log the successful creation of the user.
            _logger.Information("Created new user with id {UserId}", userEntity.Uid);
            // Map and return the newly created user entity as a user model.
            return _mapper.Map<UserModel>(userEntity);
        }

        /// <summary>
        /// Retrieves a user by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be retrieved.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the user model if found; otherwise, null.
        /// </returns>
        public async Task<UserModel?> GetByIdAsync(Guid userId)
        {
            // Create a new instance of the database context.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Attempt to find the user entity by its unique identifier.
            var userEntity = await context.Users.FindAsync(userId);

            // If the user entity is not found, log a warning and return null.
            if (userEntity == null)
            {
                _logger.Warning("User with id {UserId} not found", userId);
                return null;
            }

            // Log successful retrieval of the user.
            _logger.Information("Retrieved user with id {UserId}", userId);
            // Map and return the user entity as a user model.
            return _mapper.Map<UserModel>(userEntity);
        }

        /// <summary>
        /// Retrieves all users as a collection of user models.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a collection of user models.
        /// </returns>
        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            // Create a new instance of the database context.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Retrieve all user entities from the database.
            var users = await context.Users.ToListAsync();
            // Log the number of users retrieved.
            _logger.Information("Retrieved {Count} users", users.Count);
            // Map and return the collection of user entities as user models.
            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        /// <summary>
        /// Deletes a user identified by its unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to be deleted.</param>
        /// <returns>A task that represents the asynchronous deletion operation.</returns>
        public async Task DeleteAsync(Guid userId)
        {
            // Create a new instance of the database context.
            using var context = await _dbContextFactory.CreateDbContextAsync();
            // Attempt to locate the user entity by its unique identifier.
            var userEntity = await context.Users.FindAsync(userId);

            // If the user entity is not found, log a warning and exit the method.
            if (userEntity == null)
            {
                _logger.Warning("Attempted to delete user with id {UserId}, but user was not found", userId);
                return;
            }

            // Remove the user entity from the database context.
            context.Users.Remove(userEntity);
            // Save changes to persist the deletion.
            await context.SaveChangesAsync();
            // Log the successful deletion of the user.
            _logger.Information("Deleted user with id {UserId}", userId);
        }
    }
}

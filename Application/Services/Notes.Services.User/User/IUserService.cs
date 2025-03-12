using Notes.Models;

namespace Notes.Services.User;

public interface IUserService
{
    /// <summary>
    /// Gets the user by their ID.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <returns>The user's model, or null if the user is not found.</returns>
    Task<UserModel?> GetByIdAsync(Guid userId);

    /// <summary>
    /// Gets a list of all users.
    /// </summary>
    /// <returns>Collection of user models.</returns>
    Task<IEnumerable<UserModel>> GetAllAsync();

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    Task DeleteAsync(Guid userId);
}

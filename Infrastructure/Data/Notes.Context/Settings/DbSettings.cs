namespace Notes.Context
{
    /// <summary>
    /// Represents the configuration settings for the database, including its type, connection string, and initialization settings.
    /// </summary>
    public class DbSettings
    {
        /// <summary>
        /// Gets the type of the database (e.g., PostgreSQL).
        /// </summary>
        public DbType Type { get; private set; }

        /// <summary>
        /// Gets the connection string used to connect to the database.
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Gets the database initialization settings.
        /// </summary>
        public DbInitSettings Init { get; private set; }
    }

    /// <summary>
    /// Represents the initialization settings for the database, including options to add demo data and an administrator.
    /// </summary>
    public class DbInitSettings
    {
        /// <summary>
        /// Gets a value indicating whether demo data should be added to the database.
        /// </summary>
        public bool AddDemoData { get; private set; }

        /// <summary>
        /// Gets a value indicating whether an administrator account should be created.
        /// </summary>
        public bool AddAdministrator { get; private set; }

        /// <summary>
        /// Gets the credentials for the administrator account to be added.
        /// </summary>
        public UserCredentials Administrator { get; private set; }
    }

    /// <summary>
    /// Represents user credentials including username, email, and password.
    /// </summary>
    public class UserCredentials
    {
        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the email address of the user.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the password of the user.
        /// </summary>
        public string Password { get; private set; }
    }
}

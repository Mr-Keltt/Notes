namespace Notes.Common.Helpers
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;

    /// <summary>
    /// Provides helper methods for registering AutoMapper profiles from relevant assemblies.
    /// </summary>
    public static class AutoMappersRegisterHelper
    {
        /// <summary>
        /// Registers AutoMapper profiles by scanning all assemblies whose names start with "Notes.".
        /// </summary>
        /// <param name="services">The service collection to which the AutoMapper profiles will be added.</param>
        public static void Register(IServiceCollection services)
        {
            // Retrieve all assemblies in the current application domain that have a full name starting with "Notes."
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(s => s.FullName != null && s.FullName.StartsWith("Notes."));

            // Register AutoMapper and scan the filtered assemblies for any AutoMapper profiles.
            services.AddAutoMapper(assemblies);
        }
    }
}

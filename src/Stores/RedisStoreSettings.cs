namespace Talegen.AspNetCore.Multitenant.Stores
{
    /// <summary>
    /// This class implements the storage settings interface for Redis storage.
    /// </summary>
    /// <seealso cref="Talegen.AspNetCore.Multitenant.IStorageSettings" />
    public class RedisStoreSettings : IStorageSettings
    {
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string ApplicationName { get; set; }
    }
}
namespace Talegen.AspNetCore.Multitenant
{
    /// <summary>
    /// This interface defines the minimum implementation of a storage settings class.
    /// </summary>
    public interface IStorageSettings
    {
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        string ApplicationName { get; set; }
    }
}
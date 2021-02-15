//-----------------------------------------------------------------------
// <copyright file="TenantExtensions.cs" company="GlobalLink Vasont">
//     Copyright (c) GlobalLink Vasont. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Talegen.AspNetCore.Multitenant
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// This class contains tenant related extension methods
    /// </summary>
    public static class TenantExtensions
    {
        /// <summary>
        /// The tenant connection string key.
        /// </summary>
        public const string TenantConnectionString = "TenantConnectionString";

        #region Constructor

        /// <summary>
        /// Initializes static members of the <see cref="TenantExtensions" /> class.
        /// </summary>
        static TenantExtensions()
        {
            ConnectionStrings = new ConcurrentDictionary<string, string>();
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets or sets the collection of ConnectionStrings for the tenant model objects.
        /// </summary>
        public static ConcurrentDictionary<string, string> ConnectionStrings { get; set; }

        #endregion

        /// <summary>
        /// Creates the data repository.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <returns>Returns a new <see cref="IDataRepository" /> implementation for the specified data type in the tenant record.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IDataRepository CreateDataRepository(this ITenant tenant, ApplicationSettings settings = null, ILoggerFactory loggerFactory = null)
        {
            IDataRepository repository;
            RepositoryStorageType repositoryType = tenant.GetRepositoryType();
            string connectionString = tenant.GetConnectionString(settings);

            switch (repositoryType)
            {
                case RepositoryStorageType.SqlServer:
                    repository = RepositoryFactory<SqlDataRepository>.Create(connectionString, loggerFactory);
                    break;

                case RepositoryStorageType.Memory:
                default:
                    throw new NotImplementedException();
            }

            return repository;
        }

        /// <summary>
        /// Gets the type of the repository for the specified tenant.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <returns>Returns the <see cref="RepositoryStorageType" /> value assigned to the tenant.</returns>
        public static RepositoryStorageType GetRepositoryType(this ITenant tenant)
        {
            string key = nameof(RepositoryStorageType);
            return tenant.Properties.ContainsKey(key) ? tenant.Properties[key].ToEnum<RepositoryStorageType>() : RepositoryStorageType.SqlServer;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        public static string GetConnectionString(this ITenant tenant, ApplicationSettings settings = null)
        {
            string value = tenant.Properties.ContainsKey(TenantConnectionString) ? tenant.Properties[TenantConnectionString] : string.Empty;

            if (string.IsNullOrWhiteSpace(value) && settings != null)
            {
                value = tenant.BuildConnectionString(settings);
            }

            return value;
        }

        /// <summary>
        /// This extension method is used to build a connection string from the specified tenant model object.
        /// </summary>
        /// <param name="tenant">Contains the tenant model object to retrieve connection string values from.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>Returns a connection string for the specified tenant object.</returns>
        /// <exception cref="ArgumentNullException">tenant</exception>
        public static string BuildConnectionString(this ITenant tenant, ApplicationSettings settings)
        {
            if (tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant));
            }

            string connectionString = tenant.Properties.ContainsKey(TenantConnectionString) && !string.IsNullOrWhiteSpace(tenant.Properties[TenantConnectionString]) ? tenant.Properties[TenantConnectionString] : string.Empty;

            // if no connection string exists in tenant...
            if (string.IsNullOrEmpty(connectionString))
            {
                // first lookup to see if it exists in cache already...
                if (ConnectionStrings.ContainsKey(tenant.Identifier))
                {
                    // found, so we're done.
                    connectionString = ConnectionStrings[tenant.Identifier];
                }

                // if still no connection string...
                if (string.IsNullOrEmpty(connectionString) &&
                    !string.IsNullOrWhiteSpace(settings.Security.AppKey))
                {
                    // get properties for database related information.
                    const string databaseTypeKey = nameof(RepositoryStorageType);
                    const string databaseNameKey = nameof(BackchannelSubscriptionModel.DatabaseName);
                    const string databaseUserIdKey = nameof(BackchannelSubscriptionModel.DatabaseUserId);
                    const string databasePassKey = nameof(BackchannelSubscriptionModel.DatabasePassword);
                    const string databaseSourceKey = nameof(BackchannelSubscriptionModel.DataSource);
                    const string extendedStringKey = nameof(BackchannelSubscriptionModel.ExtendedConnectionString);

                    // retrieve values from tenant properties...
                    RepositoryStorageType storageType = tenant.Properties.ContainsKey(databaseTypeKey) ? tenant.Properties[databaseTypeKey].ToEnum<RepositoryStorageType>() : RepositoryStorageType.SqlServer;
                    string password = tenant.Properties.ContainsKey(databasePassKey) ? tenant.Properties[databasePassKey] : string.Empty;
                    string catalog = tenant.Properties.ContainsKey(databaseNameKey) ? tenant.Properties[databaseNameKey] : string.Empty;
                    string dataSource = tenant.Properties.ContainsKey(databaseSourceKey) ? tenant.Properties[databaseSourceKey] : string.Empty;
                    string userNameId = tenant.Properties.ContainsKey(databaseUserIdKey) ? tenant.Properties[databaseUserIdKey] : string.Empty;
                    string extendedString = tenant.Properties.ContainsKey(extendedStringKey) ? tenant.Properties[extendedStringKey] : string.Empty;

                    // build the connection string
                    connectionString = ConnectionFactory.BuildConnectionString(
                    storageType,
                    dataSource,
                    catalog,
                    userNameId,
                    !string.IsNullOrWhiteSpace(password) ? password.Decrypt(settings.Security.AppKey) : string.Empty,
                    extendedString);

                    // add the connection string to the memory cache dictionary for next request.
                    ConnectionStrings.TryAdd(tenant.Identifier, connectionString);

                    // add connection string to the tenant properties
                    tenant.Properties.Add(TenantConnectionString, connectionString);
                }
            }

            return connectionString;
        }
    }
}
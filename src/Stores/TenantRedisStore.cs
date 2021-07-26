/*
 *
 * (c) Copyright Talegen, LLC.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/

namespace Talegen.AspNetCore.Multitenant.Stores
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Distributed;
    using Talegen.AspNetCore.Multitenant.Properties;
    using Vasont.AspnetCore.RedisClient;

    /// <summary>
    /// Implements a tenant store utilizing <see cref="IAdvancedDistributedCache" /> storage.
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant model stored.</typeparam>
    /// <seealso cref="Talegen.AspNetCore.Multitenant.ITenantStore{TTenant}" />
    public class TenantRedisStore<TTenant> : ITenantStore<TTenant>
        where TTenant : class, ITenant, new()
    {
        /// <summary>
        /// Contains the cache key template.
        /// </summary>
        private const string CacheKeyTemplate = "Tenants:{0}:{1}";

        /// <summary>
        /// Contains an instance of the distributed cache to use.
        /// </summary>
        private readonly IAdvancedDistributedCache cache;

        /// <summary>
        /// Contains the application name.
        /// </summary>
        private readonly string applicationName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantRedisStore{TTenant}" /> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        public TenantRedisStore(IAdvancedDistributedCache cache)
        {
            this.cache = cache;
            this.applicationName = Resources.ApplicationName;
        }

        /// <summary>
        /// Returns all of the tenants from the cache storage.
        /// </summary>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a list of all tenants found in the cache storage.</returns>
        public async Task<List<TTenant>> AllTenantsAsync(CancellationToken cancellationToken = default)
        {
            List<TTenant> tenants = new List<TTenant>();
            string allTenantsPattern = string.Format(CacheKeyTemplate, this.applicationName, "*");
            var keys = await this.cache.FindKeysAsync(allTenantsPattern, cancellationToken);

            foreach (string key in keys)
            {
                tenants.Add(await this.cache.GetJsonAsync<TTenant>(key, cancellationToken));
            }

            return tenants;
        }

        /// <summary>
        /// Tries to add a new Tenant record to the cache store.
        /// </summary>
        /// <param name="tenantInfo">Contains the tenant information to add.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a value indicating whether the record was added.</returns>
        /// <exception cref="ArgumentNullException">Thrown if tenantInfo is not specified.</exception>
        /// <exception cref="NullReferenceException">Thrown if tenant identifier is not specified.</exception>
        public async Task<bool> TryAddAsync(TTenant tenantInfo, CancellationToken cancellationToken = default)
        {
            if (tenantInfo == null)
            {
                throw new ArgumentNullException(nameof(tenantInfo));
            }

            if (string.IsNullOrWhiteSpace(tenantInfo.Identifier))
            {
                throw new NullReferenceException(string.Format("{0} must have a value.", nameof(tenantInfo.Identifier)));
            }

            var cacheOptions = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(600) };
            string tenantKey = string.Format(CacheKeyTemplate, this.applicationName, tenantInfo.Identifier);
            await this.cache.SetJsonAsync(tenantKey, tenantInfo, cacheOptions, cancellationToken);
            return true;
        }

        /// <summary>
        /// Tries the get the tenant record by the tenant identifier asynchronously.
        /// </summary>
        /// <param name="identifier">The tenant identifier to find.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns The tenant record if found.</returns>
        public async Task<TTenant> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
        {
            string tenantKey = string.Format(CacheKeyTemplate, this.applicationName, identifier);
            return await this.cache.GetJsonAsync<TTenant>(tenantKey, cancellationToken);
        }
    }
}
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
    using Microsoft.Extensions.Caching.Memory;
    using Talegen.AspNetCore.Multitenant.Properties;

    /// <summary>
    /// Implements a tenant store utilizing <see cref="IMemoryCache" /> storage.
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant model stored.</typeparam>
    /// <seealso cref="Talegen.AspNetCore.Multitenant.ITenantStore{TTenant}" />
    public class TenantMemoryStore<TTenant> : ITenantStore<TTenant>
        where TTenant : class, ITenant, new()
    {
        /// <summary>
        /// Contains the cache key template.
        /// </summary>
        private const string CacheKeyTemplate = "Tenants:{0}:{1}";

        /// <summary>
        /// Contains an instance of the distributed cache to use.
        /// </summary>
        private readonly IMemoryCache cache;

        /// <summary>
        /// Contains the application name.
        /// </summary>
        private readonly string applicationName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantMemoryStore{TTenant}" /> class.
        /// </summary>
        /// <param name="cache">Contains an instance of the <see cref="IMemoryCache" /> cache object.</param>
        public TenantMemoryStore(IMemoryCache cache)
        {
            this.cache = cache;
            this.applicationName = Resources.ApplicationName;
        }

        /// <summary>
        /// This method is used to return all tenants.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Normally, returns a list of all tenants available.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<TTenant>> AllTenantsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries to add a new Tenant to the cache storage.
        /// </summary>
        /// <param name="tenantInfo">The tenant information to add.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a value indicating whether the record was added.</returns>
        /// <exception cref="ArgumentNullException">tenantInfo</exception>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<bool> TryAddAsync(TTenant tenantInfo, CancellationToken cancellationToken = default)
        {
            if (tenantInfo == null)
            {
                throw new ArgumentNullException(nameof(tenantInfo));
            }

            if (string.IsNullOrWhiteSpace(tenantInfo.Identifier))
            {
                throw new NullReferenceException(string.Format(Resources.ErrorNoCacheValueText, nameof(tenantInfo.Identifier)));
            }

            string tenantKey = string.Format(CacheKeyTemplate, this.applicationName, tenantInfo.Identifier);
            return await Task.FromResult(this.cache.Set(tenantKey, tenantInfo) != null);
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
            return await Task.FromResult(this.cache.Get<TTenant>(tenantKey));
        }
    }
}
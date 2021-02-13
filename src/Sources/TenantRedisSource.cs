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

namespace Talegen.AspNetCore.Multitenant.Sources
{
    using System.Threading;
    using System.Threading.Tasks;
    using Vasont.AspnetCore.RedisClient;

    /// <summary>
    /// This class implements the <see cref="ITenantSource{TTenant}" /> interface to search and retrieve tenant application information from an
    /// <see cref="IAdvancedDistributedCache" /> implementation (Redis).
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant.</typeparam>
    /// <seealso cref="Talegen.AspNetCore.Multitenant.ITenantSource{TTenant}" />
    public class TenantRedisSource<TTenant> : ITenantSource<TTenant>
        where TTenant : class, ITenant, new()
    {
        /// <summary>
        /// Contains the cache key template.
        /// </summary>
        private const string CacheKeyTemplate = "Tenants:Source:";

        /// <summary>
        /// Contains an instance of the distributed cache to use.
        /// </summary>
        private readonly IAdvancedDistributedCache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantRedisSource{TTenant}" /> class.
        /// </summary>
        /// <param name="cache">The cache store to utilize.</param>
        public TenantRedisSource(IAdvancedDistributedCache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// This method implements the tenant search logic for an external tenant source.
        /// </summary>
        /// <param name="tenantIdentifier">The tenant identifier to find.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the <see cref="T:Talegen.AspNetCore.Multitenant.ITenant" /> object if found.</returns>
        public async Task<TTenant> FindTenantAsync(string tenantIdentifier, CancellationToken cancellationToken = default)
        {
            string tenantKey = CacheKeyTemplate + tenantIdentifier;
            return await this.cache.GetJsonAsync<TTenant>(tenantKey, cancellationToken);
        }
    }
}
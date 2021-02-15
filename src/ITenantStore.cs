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

namespace Talegen.AspNetCore.Multitenant
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface defines the minimum implementation of a tenant store.
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant.</typeparam>
    public interface ITenantStore<TTenant> where TTenant : class, ITenant, new()
    {
        /// <summary>
        /// Returns all of the tenants from the cache storage.
        /// </summary>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a list of all tenants found in the cache storage.</returns>
        Task<List<TTenant>> AllTenantsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Tries the get the tenant record by the tenant identifier asynchronously.
        /// </summary>
        /// <param name="identifier">The tenant identifier to find.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns The tenant record if found.</returns>
        Task<TTenant> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tries to add a new Tenant to the cache storage.
        /// </summary>
        /// <param name="tenantInfo">The tenant information to add.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a value indicating whether the record was added.</returns>
        Task<bool> TryAddAsync(TTenant tenantInfo, CancellationToken cancellationToken = default);
    }
}
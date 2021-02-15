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
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Talegen.Backchannel;

    /// <summary>
    /// This interface defines the minimum service used to interact with an API resource for tenant information purposes.
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant.</typeparam>
    public interface ITenantApiServerService<TTenant>
        where TTenant : class, ITenant, new()
    {
        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>The client.</value>
        BackchannelClient Client { get; }

        /// <summary>
        /// Finds the tenant from license server.
        /// </summary>
        /// <param name="tenantIdentifier">Contains the tenant identifier to find.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the Publisher Tenant record found on the license server.</returns>
        Task<TTenant> FindTenantFromApiServerAsync(string tenantIdentifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds all tenants from license server asynchronous.
        /// </summary>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a list of tenant objects.</returns>
        Task<List<TTenant>> FindAllTenantsFromApiServerAsync(CancellationToken cancellationToken = default);
    }
}
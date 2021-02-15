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

namespace MultiTenantWebApp.Common
{
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Talegen.AspNetCore.Multitenant;

    /// <summary>
    /// This class implements a <see cref="ITenantSource{TTenant}" /> object to demonstrate an example of a tenant source retriever used as a backer to the
    /// Tenant Store. This class simply pulls from a dictionary, but in other cases could communicate with a REST API through a client_credentials backchannel
    /// call. In fact, <see cref="Talegen.AspNetCore.Multitenant.Sources.TenantBackchannelApiSource{TTenant, TService}" /> is a base class, that provides a
    /// client_credential type OAuth client where you can override the provided methods to implement your request to return a <see cref="ITenant" /> object record.
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant.</typeparam>
    /// <seealso cref="Talegen.AspNetCore.Multitenant.ITenantSource{TTenant}" />
    public class ExampleMemorySource<TTenant> : ITenantSource<TTenant>
        where TTenant : class, ITenant, new()
    {
        /// <summary>
        /// Provides a convienient static storage location for our example source records.
        /// </summary>
        public static readonly ConcurrentDictionary<string, TTenant> SourceStorage = new ConcurrentDictionary<string, TTenant>();

        /// <summary>
        /// Contains the cache key template.
        /// </summary>
        private const string CacheKeyTemplate = "Tenants:Source:";

        /// <summary>
        /// This method implements the tenant search logic for an external tenant source.
        /// </summary>
        /// <param name="tenantIdentifier">The tenant identifier to find.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the <see cref="T:Talegen.AspNetCore.Multitenant.ITenant" /> object if found.</returns>
        public async Task<TTenant> FindTenantAsync(string tenantIdentifier, CancellationToken cancellationToken = default)
        {
            string tenantKey = CacheKeyTemplate + tenantIdentifier;
            SourceStorage.TryGetValue(tenantKey, out TTenant result);
            return await Task.FromResult(result);
        }
    }
}
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
    using Talegen.Common.Core.Errors;

    /// <summary>
    /// This class implements the <see cref="ITenantSource{TTenant}" /> interface to search and retrieve tenant application information from an a REST API using
    /// a Resource to Resource backchannel client.
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant.</typeparam>
    /// <seealso cref="Talegen.AspNetCore.Multitenant.ITenantSource{TTenant}" />
    public class TenantBackchannelApiSource<TTenant, TService> : ITenantSource<TTenant>
        where TTenant : class, ITenant, new()
        where TService : TenantApiServerService<TTenant>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantBackchannelApiSource{TTenant}" /> class.
        /// </summary>
        /// <param name="tenantSettings">Contains the tenant settings.</param>
        /// <param name="errorManager">Contains an instance of an <see cref="IErrorManager" /> implementation.</param>
        public TenantBackchannelApiSource(TService service)
        {
            this.Service = service;
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>The service.</value>
        public TService Service { get; }

        /// <summary>
        /// This method implements the tenant search logic for an external tenant source.
        /// </summary>
        /// <param name="tenantIdentifier">The tenant identifier to find.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the <see cref="T:Talegen.AspNetCore.Multitenant.ITenant" /> object if found.</returns>
        public async Task<TTenant> FindTenantAsync(string tenantIdentifier, CancellationToken cancellationToken = default)
        {
            // retrieve and return the tenant found from the license server (if any)
            return await this.Service.FindTenantFromApiServerAsync(tenantIdentifier, cancellationToken);
        }
    }
}
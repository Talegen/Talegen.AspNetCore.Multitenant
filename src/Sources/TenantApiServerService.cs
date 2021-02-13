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
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Talegen.AspNetCore.Multitenant.Configuration;
    using Talegen.AspNetCore.Multitenant.Properties;
    using Talegen.Backchannel;

    /// <summary>
    /// This class impliments the tenant license server service usedto interact with the license server for tenant information purposes.
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant.</typeparam>
    public class TenantApiServerService<TTenant>
        where TTenant : class, ITenant, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantApiServerService{TTenant}" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException">settings</exception>
        public TenantApiServerService(BackchannelConfig settings)
        {
            this.Client = new BackchannelClient(settings ?? throw new ArgumentNullException(nameof(settings)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantApiServerService{TTenant}" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException">settings</exception>
        public TenantApiServerService(TenantSettings settings)
        {
            this.Client = new BackchannelClient(settings.ToBackchannelConfig() ?? throw new ArgumentNullException(nameof(settings)));
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>The client.</value>
        protected BackchannelClient Client { get; }

        /// <summary>
        /// Finds the tenant from license server.
        /// </summary>
        /// <returns>Returns the Publisher Tenant record found on the license server.</returns>
        public virtual Task<TTenant> FindTenantFromApiServerAsync(string tenantIdentifier, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException(Resources.ErrorApiNotImplementedText);
        }

        /// <summary>
        /// Finds all tenants from license server asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns a list of tenant objects.</returns>
        public virtual Task<List<TTenant>> FindAllTenantsFromApiServerAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException(Resources.ErrorApiNotImplementedText);
        }
    }
}
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
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// This class implements the multi-tenant middleware processor for retrieving and storing the tenant information into the current request context.
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant.</typeparam>
    public class MultiTenantMiddleware<TTenant> where TTenant : class, ITenant, new()
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Contains the next request
        /// </summary>
        private readonly RequestDelegate nextRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTenantMiddleware{TTenant}" /> class.
        /// </summary>
        /// <param name="next">The next request delegate.</param>
        /// <param name="logger">Contains an optional logger instance.</param>
        public MultiTenantMiddleware(RequestDelegate next, ILogger logger = null)
        {
            this.nextRequest = next;
            this.logger = logger;
        }

        /// <summary>
        /// Invokes the specified context request to retrieve and apply the tenant context.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <remarks>
        /// For every request, the HttpContext is modified here to get the tenant context. The Tenant context property is then updated with tenant lookup results.
        /// </remarks>
        public async Task Invoke(HttpContext context)
        {
            ITenantContextAccessor<TTenant> accessor = context.RequestServices.GetRequiredService<ITenantContextAccessor<TTenant>>();

            if (accessor.TenantContext == null)
            {
                ITenantResolverStrategy strategy = context.RequestServices.GetRequiredService<ITenantResolverStrategy>();
                string identifier = await strategy.GetTenantIdentifierAsync(context);
                ITenantStore<TTenant> store = context.RequestServices.GetRequiredService<ITenantStore<TTenant>>();

                if (this.logger != null)
                {
                    this.logger.LogDebug("Tenant Identifier: \"{0}\"", identifier);
                    this.logger.LogDebug("Tenant Strategy [{0}]", strategy.GetType());
                    this.logger.LogDebug("Tenant Store [{0}]", store.GetType());
                }

                // if a tenant identifier was found in the route...
                if (!string.IsNullOrWhiteSpace(identifier))
                {
                    var tenantFound = await store.GetByIdentifierAsync(identifier);

                    this.logger?.LogDebug("Tenant {0}", tenantFound != null ? "Found" : "Not found");

                    // tenant wasn't found... let's go ask an external source...
                    if (tenantFound == null)
                    {
                        // get a tenant source...
                        ITenantSource<TTenant> source = context.RequestServices.GetRequiredService<ITenantSource<TTenant>>();

                        this.logger?.LogDebug($"Tenant Source [\"{source.GetType()}\"]");

                        // if a source was registered, ask it for the tenant.
                        tenantFound = await source?.FindTenantAsync(identifier, context.RequestAborted);

                        this.logger?.LogDebug("Tenant \"{0}\" in Source {1}", identifier, tenantFound != null ? "Found" : "Not found");

                        // if a tenant was found from the external source...
                        if (tenantFound != null)
                        {
                            // add to the store for easier access later on.
                            await store.TryAddAsync(tenantFound);
                        }
                    }

                    // setup the context and pass in the tenant found (if any).
                    accessor.TenantContext = new TenantContext<TTenant>()
                    {
                        Tenant = tenantFound
                    };
                }
            }

            if (this.nextRequest != null)
            {
                await this.nextRequest(context);
            }
        }
    }
}
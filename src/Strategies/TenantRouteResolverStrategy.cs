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

namespace Talegen.AspNetCore.Multitenant.Strategies
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// This class implements a header resolver strategy for retrieving the tenant id from a request header.
    /// </summary>
    /// <seealso cref="Talegen.AspNetCore.Multitenant.ITenantResolverStrategy" />
    public class TenantRouteResolverStrategy : ITenantResolverStrategy
    {
        /// <summary>
        /// Gets the tenant identifier from the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Returns the tenant identifier if found.</returns>
        /// <remarks>This assumes a route contains the route value "tenant" in the configuration. For example "{tenant}/{controller=Home}/{action=Index}/{id?}"</remarks>
        /// <exception cref="MultiTenantException">new ArgumentException($""{ nameof(context) }, type must be of type HttpContext, nameof(context)</exception>
        public async Task<string> GetTenantIdentifierAsync(HttpContext context)
        {
            string result = string.Empty;

            // First things first, we'll always try to get tenant from the header first.
            TenantHeaderResolverStrategy headerFirstStrategy = new TenantHeaderResolverStrategy();
            result = await headerFirstStrategy.GetTenantIdentifierAsync(context);

            // if the header didn't contain the tenant, then check the route.
            if (string.IsNullOrWhiteSpace(result))
            {
                // trim leading / from path if any found.
                string path = context.Request.Path.HasValue ? context.Request.Path.Value.TrimStart('/') : string.Empty;

                // exclude NodeJS Express route/paths
                string[] excludedPaths = { "fetch-data", "sockjs-node" };

                // if the path has a value and path doesn't contain a file with an extension ... hacky way to determine this is not a "/js/script.js" call.
                if (!string.IsNullOrWhiteSpace(path) && string.IsNullOrWhiteSpace(System.IO.Path.GetExtension(context.Request.Path)))
                {
                    // remove leading / character
                    int nextSlash = path.IndexOf('/');
                    path = nextSlash > 0 ? path.Substring(0, nextSlash) : path;

                    // it's not in excluded list
                    if (!excludedPaths.Contains(path))
                    {
                        result = path;
                    }
                }
                else if (context.User.Identity.IsAuthenticated)
                {
                    // if there was no path specified, last ditch attempt to find the current user's tenant is to look for a tenant claim. hitting the root so
                    // the only way we can identify the tenant is from the authenticated user's claim, if one exists.
                    result = context.User.FindFirst(TenantDefaults.TenantClaimType)?.Value;
                }
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine("{0} {1} Found {2} Tenant Identifier: \"{3}\"", nameof(TenantRouteResolverStrategy), context.Request.Path, string.IsNullOrEmpty(result) ? "no" : string.Empty, result);
#endif
            return await Task.FromResult(result);
        }
    }
}
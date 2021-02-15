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
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// This class implements a header resolver strategy for retrieving the tenant id from a request header.
    /// </summary>
    /// <seealso cref="Talegen.AspNetCore.Multitenant.ITenantResolverStrategy" />
    public class TenantHeaderResolverStrategy : ITenantResolverStrategy
    {
        /// <summary>
        /// Gets the tenant identifier from the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Returns the tenant identifier if found.</returns>
        public async Task<string> GetTenantIdentifierAsync(HttpContext context)
        {
            string result = string.Empty;

            if (context.Request.Headers.TryGetValue(TenantDefaults.TenantHeaderName, out StringValues values))
            {
                result = values.FirstOrDefault();
#if DEBUG
                System.Diagnostics.Debug.WriteLine("{0} Resolved Tenant Identifier: \"{1}\"", nameof(TenantHostResolverStrategy), result);
#endif
            }

            return await Task.FromResult(result);
        }
    }
}
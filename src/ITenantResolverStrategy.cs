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

    /// <summary>
    /// This interface defines a minimum implementation for a tenant resolver strategy class.
    /// </summary>
    public interface ITenantResolverStrategy
    {
        /// <summary>
        /// Gets the tenant identifier from the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Returns the tenant identifier if found.</returns>
        Task<string> GetTenantIdentifierAsync(HttpContext context);
    }
}
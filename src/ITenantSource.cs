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
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface defines the minimum implementation of an external tenant source store.
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant model.</typeparam>
    public interface ITenantSource<TTenant>
        where TTenant : class, ITenant, new()
    {
        /// <summary>
        /// This method implements the tenant search logic for an external tenant source.
        /// </summary>
        /// <param name="tenantIdentifier">The tenant identifier to find.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the <see cref="ITenant" /> object if found.</returns>
        Task<TTenant> FindTenantAsync(string tenantIdentifier, CancellationToken cancellationToken = default);
    }
}
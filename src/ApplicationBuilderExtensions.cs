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
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// This class contains application builder extensions for implementing multi-tenant context loader in the startup of the application.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Uses the multi tenancy middleware class.
        /// </summary>
        /// <typeparam name="TTenant">The type of the tenant.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="logger">Contains a logger instance.</param>
        /// <returns>Returns the application builder.</returns>
        public static IApplicationBuilder UseMultiTenancy<TTenant>(this IApplicationBuilder builder, ILogger logger = null)
            where TTenant : class, ITenant, new()
            => builder.UseMiddleware<MultiTenantMiddleware<TTenant>>(logger);
    }
}
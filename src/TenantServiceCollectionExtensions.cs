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
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// This class contains service collection extensions for creating a tenant resolver.
    /// </summary>
    public static class TenantServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the multi tenancy.
        /// </summary>
        /// <typeparam name="TTenant">The type of the tenant.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>Returns the tenant builder.</returns>
        public static TenantBuilder<TTenant> AddMultiTenancy<TTenant>(this IServiceCollection services)
            where TTenant : class, ITenant, new()
        {
            services.AddScoped<ITenantContext<TTenant>, TenantContext<TTenant>>();
            services.AddScoped<ITenant, TTenant>();
            services.AddSingleton<ITenantContextAccessor<TTenant>, TenantContextAccessor<TTenant>>();

            return new TenantBuilder<TTenant>(services);
        }
    }
}
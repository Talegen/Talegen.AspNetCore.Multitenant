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
    /// This class implements the tenant builder logic for tenant and store strategy.
    /// </summary>
    /// <typeparam name="TTenant">The type of the tenant.</typeparam>
    public class TenantBuilder<TTenant> where TTenant : class, ITenant, new()
    {
        /// <summary>
        /// Contains an instance of the services collection.
        /// </summary>
        private readonly IServiceCollection services;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantBuilder{TTenant}" /> class.
        /// </summary>
        /// <param name="services">The services collection.</param>
        public TenantBuilder(IServiceCollection services)
        {
            this.services = services;
        }

        /// <summary>
        /// Implements the specific tenant resolution strategy.
        /// </summary>
        /// <typeparam name="TStrategy">The type of the strategy.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <returns>Returns the tenant builder.</returns>
        public TenantBuilder<TTenant> WithResolutionStrategy<TStrategy>(ServiceLifetime lifetime = ServiceLifetime.Transient) where TStrategy : class, ITenantResolverStrategy
        {
            this.services.Add(ServiceDescriptor.Describe(typeof(ITenantResolverStrategy), typeof(TStrategy), lifetime));
            return this;
        }

        /// <summary>
        /// Implements the specific tenant storage settings for a given type.
        /// </summary>
        /// <typeparam name="TStorageType">The type of the storage type to implement.</typeparam>
        /// <param name="settings">The settings.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <returns>Returns the tenant builder.</returns>
        public TenantBuilder<TTenant> WithStorageSettings<TStorageType>(TStorageType settings, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            this.services.Add(ServiceDescriptor.Describe(typeof(TStorageType), (provider) => { return settings; }, lifetime));
            return this;
        }

        /// <summary>
        /// Implements the specific tenant store store method.
        /// </summary>
        /// <typeparam name="TStore">The type of the store.</typeparam>
        /// <param name="storageSettings">Contains an optional storage settings.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <returns>Returns the tenant builder.</returns>
        public TenantBuilder<TTenant> WithStore<TStore>(IStorageSettings storageSettings = null, ServiceLifetime lifetime = ServiceLifetime.Transient) where TStore : class, ITenantStore<TTenant>
        {
            // if storage settings are declared, use them here.
            if (storageSettings != null)
            {
                this.services.AddSingleton(storageSettings);
            }

            this.services.Add(ServiceDescriptor.Describe(typeof(ITenantStore<TTenant>), typeof(TStore), lifetime));
            return this;
        }

        /// <summary>
        /// Implements the specific tenant source for retrieval of non-cached tenant requests.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="lifetime">The lifetime.</param>
        /// <returns>Returns the tenant builder.</returns>
        public TenantBuilder<TTenant> WithSource<TSource>(ServiceLifetime lifetime = ServiceLifetime.Transient) where TSource : class, ITenantSource<TTenant>
        {
            this.services.Add(ServiceDescriptor.Describe(typeof(ITenantSource<TTenant>), typeof(TSource), lifetime));
            return this;
        }
    }
}
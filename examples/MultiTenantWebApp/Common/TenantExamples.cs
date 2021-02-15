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

namespace MultiTenantWebApp.Common
{
    using System;
    using System.Collections.Generic;
    using Talegen.AspNetCore.Multitenant;

    /// <summary>
    /// Contains extensions for loading tenant examples to a distributed cache source.
    /// </summary>
    public static class TenantExamples
    {
        /// <summary>
        /// Contains the cache key template.
        /// </summary>
        private const string CacheKeyTemplate = "Tenants:Source:";

        /// <summary>
        /// The tenant1 identifier
        /// </summary>
        public const string Tenant1Id = "tenant1";

        /// <summary>
        /// The tenant2 identifier
        /// </summary>
        public const string Tenant2Id = "tenant2";

        /// <summary>
        /// The address key
        /// </summary>
        public const string AddressKey = "Address";

        /// <summary>
        /// The name key
        /// </summary>
        public const string NameKey = "Name";

        /// <summary>
        /// Adds the sample tenants to source to the sample memory store.
        /// </summary>
        public static void AddSampleTenantsToSource<TTenant>()
            where TTenant : class, ITenant, new()
        {
            // create a tenant1 object
            TTenant tenant1Object = new TTenant
            {
                Id = Guid.NewGuid(),
                Identifier = Tenant1Id,
                Properties = new Dictionary<string, string>
                {
                    { NameKey, "Contoso Co." },
                    { AddressKey, "123 Contoso Circle" }
                }
            };

            // create a tenant2 object
            TTenant tenant2Object = new TTenant
            {
                Id = Guid.NewGuid(),
                Identifier = Tenant2Id,
                Properties = new Dictionary<string, string>
                {
                    { NameKey, "Acme Warehouse" },
                    { AddressKey, "12 Logistics Way" }
                }
            };

            ExampleMemorySource<TTenant>.SourceStorage.TryAdd(CacheKeyTemplate + Tenant1Id, tenant1Object);
            ExampleMemorySource<TTenant>.SourceStorage.TryAdd(CacheKeyTemplate + Tenant2Id, tenant2Object);
        }
    }
}
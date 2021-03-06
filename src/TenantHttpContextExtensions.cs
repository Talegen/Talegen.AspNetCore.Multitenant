﻿/*
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
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// This class contains extension methods for the HTTP context.
    /// </summary>
    public static class TenantHttpContextExtensions
    {
        /// <summary>
        /// Gets the tenant context from the current Http context.
        /// </summary>
        /// <typeparam name="TTenant">The type of the tenant.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>Returns the tenant context if found.</returns>
        public static ITenantContext<TTenant> GetTenantContext<TTenant>(this HttpContext context) where TTenant : class, ITenant, new()
        {
            return context?.RequestServices.GetRequiredService<ITenantContextAccessor<TTenant>>()?.TenantContext;
        }
    }
}
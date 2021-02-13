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
    /// <summary>
    /// This class contains static constants for use by the tenant library.
    /// </summary>
    public static class TenantDefaults
    {
        /// <summary>
        /// The tenant header name.
        /// </summary>
        public const string TenantHeaderName = "x-tenant";

        /// <summary>
        /// The tenant claim type.
        /// </summary>
        public const string TenantClaimType = "tenant";
    }
}
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

namespace MultiTenantWebApp.Models
{
    using MultiTenantWebApp.Common;
    using Talegen.AspNetCore.Multitenant;

    /// <summary>
    /// This will contain our tenant info to display in the example.
    /// </summary>
    /// <seealso cref="Talegen.AspNetCore.Multitenant.SimpleTenant" />
    public class HomeViewModel : SimpleTenant
    {
        /// <summary>
        /// Gets the name from the tenant properties dictionary.
        /// </summary>
        /// <value>The name of the tenant.</value>
        public string Name => this.Properties.ContainsKey(TenantExamples.NameKey) ? this.Properties[TenantExamples.NameKey] : string.Empty;

        /// <summary>
        /// Gets the address from the tenant properties dictionary.
        /// </summary>
        /// <value>The address of the tenant.</value>
        public string Address => this.Properties.ContainsKey(TenantExamples.AddressKey) ? this.Properties[TenantExamples.AddressKey] : string.Empty;
    }
}
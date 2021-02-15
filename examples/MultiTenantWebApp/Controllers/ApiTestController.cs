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

namespace MultiTenantWebApp.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using MultiTenantWebApp.Models;
    using Talegen.AspNetCore.Multitenant;

    /// <summary>
    /// This controller provides a simple rest example for determinining tenant from the route or web header request.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("/api/{tenant}/[controller]")]
    [Route("/api/[controller]")]
    public class ApiTestController : ControllerBase
    {
        /// <summary>
        /// The lazy tenant.
        /// </summary>
        private readonly Lazy<ITenant> lazyTenant;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTestController" /> class.
        /// </summary>
        public ApiTestController()
        {
            // set tht tenant object found on request.
            this.lazyTenant = new Lazy<ITenant>(() => this.HttpContext.GetTenantContext<SimpleTenant>()?.Tenant);
        }

        /// <summary>
        /// Gets the tenant.
        /// </summary>
        /// <value>The tenant.</value>
        public ITenant Tenant => this.lazyTenant.Value;

        /// <summary>
        /// Gets the tenant information from an API call.
        /// </summary>
        /// <returns>Returns the tenant information from the REST API call.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            // but we already have it processed and ready to go here. Convert to a view model for effect.
            var viewModel = this.Tenant != null ? new HomeViewModel
            {
                Id = this.Tenant.Id,
                Identifier = this.Tenant.Identifier,
                Properties = this.Tenant.Properties
            } : new HomeViewModel();

            // return to the view.
            return this.Ok(viewModel);
        }
    }
}
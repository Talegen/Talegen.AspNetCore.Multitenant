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
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MultiTenantWebApp.Models;
    using Talegen.AspNetCore.Multitenant;

    /// <summary>
    /// The Home Controller class.
    /// </summary>
    /// <remarks>
    /// Nothing fancy, here is where we need to know what tenant we're using. In this example, we're using the route. In others you can use the x-tenant header
    /// to a REST API call. In either case, the middleware will attach the tenant context to the current HttpContext. As you see below, the tenant is loaded
    /// when needed, by calling the GetTenantContext method for the context, and returning the Tenant object if found.
    /// </remarks>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class HomeController : Controller
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<HomeController> logger;

        /// <summary>
        /// The lazy tenant
        /// </summary>
        private readonly Lazy<ITenant> lazyTenant;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;

            // set tht tenant object found on request.
            this.lazyTenant = new Lazy<ITenant>(() => this.HttpContext.GetTenantContext<SimpleTenant>()?.Tenant);
        }

        /// <summary>
        /// Gets the tenant.
        /// </summary>
        /// <value>The tenant.</value>
        public ITenant Tenant => this.lazyTenant.Value;

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Returns the view with the model.</returns>
        /// <remarks>
        /// Because we're using the route method, we can play with this a little by adding the tenant string to the list of parameters. ASP.net Core will pass
        /// the /{tenant}/ route into this value. However, it's a bit inconvienient to have to process this for every route request. below, we use the
        /// lazy-loaded Tenant property to retrieve the tenant found with the given identifier passed in the route, intercepted by the middleware, and already
        /// pre-processed and loaded into the tenant Context.
        /// </remarks>
        public IActionResult Index(string tenant)
        {
            // just to show tenant can be retrieved through a parameter here.
            this.logger.LogInformation($"Index(\"{tenant}\")");

            // but we already have it processed and ready to go here. Convert to a view model for effect.
            var viewModel = this.Tenant as HomeViewModel;

            // return to the view.
            return View(viewModel);
        }

        /// <summary>
        /// Privacies this instance.
        /// </summary>
        /// <returns>Returns the action result view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Errors this instance.
        /// </summary>
        /// <returns>Returns the action result view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This interface represents the minimum implementation of a tenant information model.
    /// </summary>
    public interface ITenant
    {
        /// <summary>
        /// Gets or sets the durable tenant identifier that will never change.
        /// </summary>
        /// <value>The identifier.</value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the tenant identifier name.
        /// </summary>
        /// <value>The name.</value>
        string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        Dictionary<string, string> Properties { get; set; }
    }
}
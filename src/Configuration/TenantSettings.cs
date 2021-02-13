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

namespace Talegen.AspNetCore.Multitenant.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// This class implements the backchannel client configuration settings necessary for communicating with a resource to resource tenant source request.
    /// </summary>
    /// <seealso cref="Talegen.Backchannel.BackchannelConfig" />
    public class TenantSettings : Dictionary<string, string>
    {
    }
}
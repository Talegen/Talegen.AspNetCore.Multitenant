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

namespace Talegen.AspNetCore.Multitenant.Sources
{
    using System;
    using System.Collections.Generic;
    using Talegen.AspNetCore.Multitenant.Configuration;
    using Talegen.AspNetCore.Multitenant.Properties;
    using Talegen.Backchannel;
    using Talegen.Common.Core.Extensions;

    /// <summary>
    /// This class contains extension methods for the Tenant API source backchannel client.
    /// </summary>
    public static class TenantApiSourceBackchannelExtensions
    {
        /// <summary>
        /// Converts a <see cref="TenantSettings" /> dictionary into a <see cref="BackchannelConfig" /> object used to setup the backchannel client for resource
        /// to resource client API communication via a client credentials REST call.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>
        /// Returns a new <see cref="BackchannelConfig" /> with properties populated from the required values stored in the <see cref="TenantSettings" /> dictionary.
        /// </returns>
        /// <exception cref="ArgumentException">AuthorityUri or ClientId or Scopes or Secret or ResourceUri</exception>
        public static BackchannelConfig ToBackchannelConfig(this TenantSettings config)
        {
            return new BackchannelConfig
            {
                AuthenticationMethod = ClientAuthenticationMethods.ClientCredentials,
                AuthorityUri = config.ContainsKey(nameof(BackchannelConfig.AuthorityUri)) ? new Uri(config[nameof(BackchannelConfig.AuthorityUri)]) : throw new ArgumentException(string.Format(Resources.ErrorInvalidConfigText, nameof(BackchannelConfig.AuthorityUri)), nameof(BackchannelConfig.AuthorityUri)),
                ClientId = config.ContainsKey(nameof(BackchannelConfig.AuthorityUri)) ? config[nameof(BackchannelConfig.ClientId)] : throw new ArgumentException(string.Format(Resources.ErrorInvalidConfigText, nameof(BackchannelConfig.ClientId)), nameof(BackchannelConfig.ClientId)),
                Scopes = config.ContainsKey(nameof(BackchannelConfig.Scopes)) ? new List<string>(config[nameof(BackchannelConfig.Scopes)].Split(' ')) : throw new ArgumentException(string.Format(Resources.ErrorInvalidConfigText, nameof(BackchannelConfig.Scopes)), nameof(BackchannelConfig.Scopes)),
                Secret = config.ContainsKey(nameof(BackchannelConfig.Secret)) ? config[nameof(BackchannelConfig.Secret)] : throw new ArgumentException(string.Format(Resources.ErrorInvalidConfigText, nameof(BackchannelConfig.Secret)), nameof(BackchannelConfig.Secret)),
                ResourceUri = config.ContainsKey(nameof(BackchannelConfig.ResourceUri)) ? new Uri(config[nameof(BackchannelConfig.ResourceUri)]) : throw new ArgumentException(string.Format(Resources.ErrorInvalidConfigText, nameof(BackchannelConfig.ResourceUri)), nameof(BackchannelConfig.ResourceUri)),
                UseDiscoveryForEndpoint = config.ContainsKey(nameof(BackchannelConfig.UseDiscoveryForEndpoint)) ? config[nameof(BackchannelConfig.UseDiscoveryForEndpoint)].ToBoolean() : true
            };
        }
    }
}
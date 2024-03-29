// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.IdentityClient.Models
{
    /// <summary> Model factory for models. </summary>
    public static partial class EryphIdentityClientModelFactory
    {
        /// <summary> Initializes a new instance of ClientWithSecret. </summary>
        /// <param name="id">
        /// Unique identifier for a eryph client
        /// Only characters a-z, A-Z, numbers 0-9 and hyphens are allowed.
        /// </param>
        /// <param name="name"> human readable name of client, for example email address of owner. </param>
        /// <param name="allowedScopes"> allowed scopes of client. </param>
        /// <param name="roles"> Roles of client. </param>
        /// <param name="tenantId"> Tenant of client. </param>
        /// <param name="key"> private Key of client. </param>
        /// <returns> A new <see cref="Models.ClientWithSecret"/> instance for mocking. </returns>
        public static ClientWithSecret ClientWithSecret(string id = null, string name = null, IEnumerable<string> allowedScopes = null, IEnumerable<Guid> roles = null, Guid? tenantId = null, string key = null)
        {
            allowedScopes ??= new List<string>();
            roles ??= new List<Guid>();

            return new ClientWithSecret(id, name, allowedScopes?.ToList(), roles?.ToList(), tenantId, key);
        }
    }
}

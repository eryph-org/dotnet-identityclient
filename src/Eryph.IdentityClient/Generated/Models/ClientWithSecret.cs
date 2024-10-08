// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.IdentityClient.Models
{
    /// <summary> The ClientWithSecret. </summary>
    public partial class ClientWithSecret
    {
        /// <summary> Initializes a new instance of <see cref="ClientWithSecret"/>. </summary>
        /// <param name="id">
        /// The Unique identifier of the eryph client.
        /// Only characters a-z, A-Z, numbers 0-9 and hyphens are allowed.
        /// </param>
        /// <param name="name"> Human-readable name of the client, for example email address of owner. </param>
        /// <param name="allowedScopes"></param>
        /// <param name="roles"> The roles of the client,. </param>
        /// <param name="tenantId"> The ID of the tenant to which the client belongs. </param>
        /// <param name="key"> The private key or shared secret of the client. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/>, <paramref name="name"/>, <paramref name="allowedScopes"/>, <paramref name="roles"/>, <paramref name="tenantId"/> or <paramref name="key"/> is null. </exception>
        internal ClientWithSecret(string id, string name, IEnumerable<string> allowedScopes, IEnumerable<string> roles, string tenantId, string key)
        {
            Argument.AssertNotNull(id, nameof(id));
            Argument.AssertNotNull(name, nameof(name));
            Argument.AssertNotNull(allowedScopes, nameof(allowedScopes));
            Argument.AssertNotNull(roles, nameof(roles));
            Argument.AssertNotNull(tenantId, nameof(tenantId));
            Argument.AssertNotNull(key, nameof(key));

            Id = id;
            Name = name;
            AllowedScopes = allowedScopes.ToList();
            Roles = roles.ToList();
            TenantId = tenantId;
            Key = key;
        }

        /// <summary> Initializes a new instance of <see cref="ClientWithSecret"/>. </summary>
        /// <param name="id">
        /// The Unique identifier of the eryph client.
        /// Only characters a-z, A-Z, numbers 0-9 and hyphens are allowed.
        /// </param>
        /// <param name="name"> Human-readable name of the client, for example email address of owner. </param>
        /// <param name="allowedScopes"></param>
        /// <param name="roles"> The roles of the client,. </param>
        /// <param name="tenantId"> The ID of the tenant to which the client belongs. </param>
        /// <param name="key"> The private key or shared secret of the client. </param>
        internal ClientWithSecret(string id, string name, IReadOnlyList<string> allowedScopes, IReadOnlyList<string> roles, string tenantId, string key)
        {
            Id = id;
            Name = name;
            AllowedScopes = allowedScopes;
            Roles = roles;
            TenantId = tenantId;
            Key = key;
        }

        /// <summary>
        /// The Unique identifier of the eryph client.
        /// Only characters a-z, A-Z, numbers 0-9 and hyphens are allowed.
        /// </summary>
        public string Id { get; }
        /// <summary> Human-readable name of the client, for example email address of owner. </summary>
        public string Name { get; }
        /// <summary> Gets the allowed scopes. </summary>
        public IReadOnlyList<string> AllowedScopes { get; }
        /// <summary> The roles of the client,. </summary>
        public IReadOnlyList<string> Roles { get; }
        /// <summary> The ID of the tenant to which the client belongs. </summary>
        public string TenantId { get; }
        /// <summary> The private key or shared secret of the client. </summary>
        public string Key { get; }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace Eryph.IdentityClient.Models
{
    /// <summary> The Client. </summary>
    public partial class Client
    {
        /// <summary> Initializes a new instance of Client. </summary>
        public Client()
        {
            AllowedScopes = new ChangeTrackingList<string>();
            Roles = new ChangeTrackingList<Guid>();
        }

        /// <summary> Initializes a new instance of Client. </summary>
        /// <param name="id">
        /// Unique identifier for a eryph client
        /// Only characters a-z, A-Z, numbers 0-9 and hyphens are allowed.
        /// </param>
        /// <param name="name"> human readable name of client, for example email address of owner. </param>
        /// <param name="allowedScopes"> allowed scopes of client. </param>
        /// <param name="roles"> Roles of client. </param>
        /// <param name="tenantId"> Tenant of client. </param>
        internal Client(string id, string name, IList<string> allowedScopes, IList<Guid> roles, Guid? tenantId)
        {
            Id = id;
            Name = name;
            AllowedScopes = allowedScopes;
            Roles = roles;
            TenantId = tenantId;
        }

        /// <summary>
        /// Unique identifier for a eryph client
        /// Only characters a-z, A-Z, numbers 0-9 and hyphens are allowed.
        /// </summary>
        public string Id { get; set; }
        /// <summary> human readable name of client, for example email address of owner. </summary>
        public string Name { get; set; }
        /// <summary> allowed scopes of client. </summary>
        public IList<string> AllowedScopes { get; set; }
        /// <summary> Roles of client. </summary>
        public IList<Guid> Roles { get; set; }
        /// <summary> Tenant of client. </summary>
        public Guid? TenantId { get; set; }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eryph.IdentityClient.Models
{
    /// <summary> The UpdateClientRequestBody. </summary>
    public partial class UpdateClientRequestBody
    {
        /// <summary> Initializes a new instance of <see cref="UpdateClientRequestBody"/>. </summary>
        /// <param name="name"> Human-readable name of the client, for example email address of owner. </param>
        /// <param name="allowedScopes"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> or <paramref name="allowedScopes"/> is null. </exception>
        public UpdateClientRequestBody(string name, IEnumerable<string> allowedScopes)
        {
            Argument.AssertNotNull(name, nameof(name));
            Argument.AssertNotNull(allowedScopes, nameof(allowedScopes));

            Name = name;
            AllowedScopes = allowedScopes.ToList();
        }

        /// <summary> Initializes a new instance of <see cref="UpdateClientRequestBody"/>. </summary>
        /// <param name="name"> Human-readable name of the client, for example email address of owner. </param>
        /// <param name="allowedScopes"></param>
        internal UpdateClientRequestBody(string name, IList<string> allowedScopes)
        {
            Name = name;
            AllowedScopes = allowedScopes;
        }

        /// <summary> Human-readable name of the client, for example email address of owner. </summary>
        public string Name { get; }
        /// <summary> Gets the allowed scopes. </summary>
        public IList<string> AllowedScopes { get; }
    }
}

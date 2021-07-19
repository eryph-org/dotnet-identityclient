using System;
using JetBrains.Annotations;

namespace Eryph.IdentityClient.Commands
{
    [PublicAPI]
    public class CreatedClient
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Uri IdentityProvider { get; set; }

        public string[] AllowedScopes { get; set; }

        public string PrivateKey { get; set; }

    }
}
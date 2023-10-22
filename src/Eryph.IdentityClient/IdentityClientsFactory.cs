using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Azure.Core;
using Azure.Core.Pipeline;
using Eryph.ClientRuntime.Authentication;

namespace Eryph.IdentityClient
{
    public class IdentityClientsFactory
    {
        private readonly EryphIdentityClientOptions _options;
        private readonly Uri _endpoint;

        public IdentityClientsFactory(EryphIdentityClientOptions options, Uri endpoint)
        {
            _options = options;
            _endpoint = endpoint;
        }

        public ClientsClient CreateClientsClient(string? scopes = null) =>
            new(new ClientDiagnostics(_options), _options.BuildHttpPipeline(_options.ClientCredentials, scopes), _endpoint);


    }
}

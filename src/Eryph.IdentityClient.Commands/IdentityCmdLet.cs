using System;
using Eryph.ClientRuntime.Configuration;
using Eryph.ClientRuntime.Powershell;
using JetBrains.Annotations;
using Microsoft.Rest;

namespace Eryph.IdentityClient.Commands
{
    [PublicAPI]
    public abstract class IdentityCmdLet : EryphCmdLet
    {

        protected ServiceClientCredentials GetCredentials(params string[] scopes)
        {
            return ServiceClientCredentialsCache.Instance.GetServiceCredentials(GetClientCredentials(), scopes).GetAwaiter().GetResult();
        }

        protected Uri GetEndpointUri()
        {

            var credentials = GetClientCredentials();
            var endpointLookup = new EndpointLookup(new PowershellEnvironment(SessionState));
            return endpointLookup.GetEndpoint("identity", credentials.Configuration);

        }
    }
}
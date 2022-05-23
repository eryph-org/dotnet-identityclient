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


    }
}
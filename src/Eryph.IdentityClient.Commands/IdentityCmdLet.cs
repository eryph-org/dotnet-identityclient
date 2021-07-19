using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Eryph.IdentityModel.Clients;
using JetBrains.Annotations;
using Microsoft.Rest;

namespace Eryph.IdentityClient.Commands
{
    [PublicAPI]
    public abstract class IdentityCmdLet : PSCmdlet
    {
        [Parameter]
        public ClientCredentials Credentials { get; set; }


        protected ClientCredentials GetClientCredentials()
        {
            var clientCredentials = Credentials;
            if (clientCredentials != null) return clientCredentials;


            var obj = SessionState.InvokeCommand.InvokeScript("Get-EryphClientCredentials").FirstOrDefault();
            if (obj?.BaseObject is ClientCredentials credentials)
                clientCredentials = credentials;

            if (clientCredentials == null)
            {
                throw new InvalidOperationException(@"Could not find credentials for eryph.
You can use the parameter Credentials to set the eryph credentials. If not set, the credentials from CmdLet Get-EryphClientCredentials will be used.
In this case the credentials will be searched in your local configuration. 
If there is no default eryph client in your configuration the command will try to access the default system-client of a local running eryph zero or identity server.
To access the system-client you will have to run this command as Administrator (Windows) or root (Linux).
");
            }

            return clientCredentials;
        }

        protected ServiceClientCredentials GetCredentials(params string[] scopes)
        {
            return ServiceClientCredentialsCache.Instance.GetServiceCredentials(GetClientCredentials(), scopes).GetAwaiter().GetResult();
        }


    }
}
using System;
using System.Management.Automation;
using Eryph.ClientRuntime.Configuration;
using Eryph.ClientRuntime.Powershell;
using JetBrains.Annotations;

namespace Eryph.IdentityClient.Commands
{
    [PublicAPI]
    public abstract class IdentityCmdLet : EryphCmdLet
    {

        protected IdentityClientsFactory Factory;

        protected bool IsDebugEnabled
        {
            get
            {
                bool debug;
                var containsDebug = MyInvocation.BoundParameters.ContainsKey("Debug");
                if (containsDebug)
                    debug = ((SwitchParameter)MyInvocation.BoundParameters["Debug"]).ToBool();
                else
                    debug = (ActionPreference)GetVariableValue("DebugPreference") != ActionPreference.SilentlyContinue;

                return debug;
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            var options = new EryphIdentityClientOptions(GetClientCredentials())
            {
                Diagnostics =
                {
                    IsDistributedTracingEnabled = true,
                    IsLoggingEnabled = IsDebugEnabled,
                    IsLoggingContentEnabled = IsDebugEnabled,
                    LoggedHeaderNames = { "api-supported-versions" },
                }
            };


            Factory = new IdentityClientsFactory(
                options, GetEndpointUri());

        }


        protected Uri GetEndpointUri()
        {

            var credentials = GetClientCredentials();
            var endpointLookup = new EndpointLookup(new PowershellEnvironment(SessionState));
            return endpointLookup.GetEndpoint("identity", credentials.Configuration);

        }
    }
}
using System.Management.Automation;
using Eryph.ClientRuntime.OData;
using Eryph.IdentityClient.Models;
using JetBrains.Annotations;

namespace Eryph.IdentityClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Get,"EryphClient", DefaultParameterSetName = "get client")]
    [OutputType(typeof(Client))]
    public class GetEryphClientCommand : IdentityCmdLet
    {
        [Parameter(
            ParameterSetName = "get client",
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter(
            Position = 0,
            ParameterSetName = "list clients",
            ValueFromPipelineByPropertyName = true)]
        public string[] Name { get; set; }


        protected override void ProcessRecord()
        {
            using (var identityClient = new EryphIdentityClient(GetEndpointUri(), GetCredentials("identity:clients:read:all")))
            {
                if (Id != null)
                {
                    foreach (var id in Id)
                    {
                        WriteObject(identityClient.Clients.Get(id));
                    }

                    return;
                }

                if (Name != null)
                {
                    foreach (var name in Name)
                    {
                        WriteObject(identityClient.Clients.List(), true);
                    }

                    return;
                }

                WriteObject(identityClient.Clients.List(), true);

            }

        }

    }
}
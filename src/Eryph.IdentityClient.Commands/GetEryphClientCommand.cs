using System.Linq;
using System.Management.Automation;
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
            var identityClient = Factory.CreateClientsClient();

            if (Id != null)
            {
                foreach (var id in Id)
                {
                    var response = identityClient.Get(id);
                    if(response.HasValue)
                        WriteObject(response.Value);
                }

                return;
            }

            foreach (var client in identityClient.List())
            {
                if (Stopping) break;

                if(Name != null)
                {
                    if (Name.Contains(client.Name))
                        WriteObject(client);
                }

                WriteObject(client);

            }

        }

    }
}
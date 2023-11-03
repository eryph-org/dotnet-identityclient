using System.Management.Automation;
using Eryph.IdentityClient.Models;
using JetBrains.Annotations;

namespace Eryph.IdentityClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "EryphClientKey")]
    [OutputType(typeof(ClientWithSecret))]
    public class NewEryphClientKeyCommand : IdentityCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }


        [Parameter()]
        public SwitchParameter SharedKey { get; set; }

        protected override void ProcessRecord()
        {
            var identityClient = Factory.CreateClientsClient();
            {
                foreach (var id in Id)
                {
                    var client =
                        identityClient.Get(id)?.Value;

                    if (client == null)
                        return;

                    var response = identityClient.NewKey(id, new NewClientKeyRequestBody
                    {
                        SharedSecret = SharedKey
                    });

                    WriteObject(response.Value);

                }

            }
        }

    }
}
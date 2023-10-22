using System.Management.Automation;
using Eryph.IdentityClient.Models;
using JetBrains.Annotations;

namespace Eryph.IdentityClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Remove,"EryphClient")]
    [OutputType(typeof(Client))]
    public class RemoveEryphClientCommand : IdentityCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        protected override void ProcessRecord()
        {
            var identityClient = Factory.CreateClientsClient();
            foreach (var id in Id)
            {
                identityClient.Delete(id);

            }
        }

    }
}
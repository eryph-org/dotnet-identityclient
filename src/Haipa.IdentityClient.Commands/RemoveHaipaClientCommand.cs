using System.Management.Automation;
using Haipa.IdentityClient.Models;
using JetBrains.Annotations;

namespace Haipa.IdentityClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Remove,"HaipaClient")]
    [OutputType(typeof(Client))]
    public class RemoveHaipaClientCommand : IdentityCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        protected override void ProcessRecord()
        {
            using (var identityClient = new HaipaIdentityClient(GetCredentials("identity:clients:write:all")))
            {
                foreach (var id in Id)
                {
                    identityClient.Clients.Delete(id);

                }

            }
        }

    }
}
using System.Management.Automation;
using Haipa.IdentityClient.Models;

namespace Haipa.IdentityClient.Commands
{
    [Cmdlet(VerbsCommon.Remove,"HaipaClient")]
    [OutputType(typeof(Client))]
    public class RemoveHaipaClientCommand : IdentityCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            using (var identityClient = new HaipaIdentityClient(GetCredentials()))
            {
                foreach (var id in Id)
                {
                    identityClient.Clients.Delete(id);

                }

            }
        }

    }
}
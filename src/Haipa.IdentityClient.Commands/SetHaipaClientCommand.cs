using System.Collections.Generic;
using System.Management.Automation;
using Haipa.IdentityClient.Models;

namespace Haipa.IdentityClient.Commands
{
    [Cmdlet(VerbsCommon.Set,"HaipaClient")]
    [OutputType(typeof(Client))]
    public class SetHaipaClientCommand : IdentityCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] Id { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true)]
        public string[] AllowedScopes { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true)]
        public string Description { get; set; }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            using (var identityClient = new HaipaIdentityClient(GetCredentials()))
            {
                foreach (var id in Id)
                {

                    WriteObject(identityClient.Clients.ChangeWithHttpMessagesAsync(id, 
                        new Client(null, Name, Description, AllowedScopes), 
                        new Dictionary<string, List<string>>{
                            { "prefer", new List<string>{ "return=representation" } }
                        }).GetAwaiter().GetResult().Body);
                    
                }

            }
        }

    }
}
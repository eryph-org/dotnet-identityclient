using System.Collections.Generic;
using System.Management.Automation;
using Haipa.IdentityClient.Models;
using JetBrains.Annotations;

namespace Haipa.IdentityClient.Commands
{
    [PublicAPI]
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

        protected override void ProcessRecord()
        {
            using (var identityClient = new HaipaIdentityClient(GetCredentials("identity:clients:write:all")))
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


    // This class controls our dependency resolution
}
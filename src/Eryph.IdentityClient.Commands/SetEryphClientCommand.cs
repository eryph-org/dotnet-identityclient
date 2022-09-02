using System;
using System.Management.Automation;
using Eryph.IdentityClient.Models;
using JetBrains.Annotations;

namespace Eryph.IdentityClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.Set,"EryphClient")]
    [OutputType(typeof(Client))]
    public class SetEryphClientCommand : IdentityCmdLet
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
            using (var identityClient = new EryphIdentityClient(GetEndpointUri(),GetCredentials("identity:clients:write:all")))
            {
                foreach (var id in Id)
                {
                    var client =
                        identityClient.Clients.Get(id);

                    if(AllowedScopes != null)
                        client.AllowedScopes = AllowedScopes;

                    if(Name != null)
                        client.Name = Name;

                    if (Description != null)
                        client.Description = Description;

                    WriteObject(identityClient.Clients.Update(id, client));

                }

            }
        }

    }


    // This class controls our dependency resolution
}
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
            var identityClient = Factory.CreateClientsClient();
            {
                foreach (var id in Id)
                {
                    var response = identityClient.Update(
                        id,
                        new UpdateClientRequestBody(Name, AllowedScopes));
                    WriteObject(response.Value);
                }
            }
        }
    }
}

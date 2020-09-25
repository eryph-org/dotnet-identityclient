using System.Management.Automation;
using Haipa.ClientRuntime.OData;
using Haipa.IdentityClient.Models;

namespace Haipa.IdentityClient.Commands
{
    [Cmdlet(VerbsCommon.Get,"HaipaClient", DefaultParameterSetName = "get client")]
    [OutputType(typeof(Client))]
    public class GetHaipaClientCommand : IdentityCmdLet
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


        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            using (var identityClient = new HaipaIdentityClient(GetCredentials()))
            {
                if (Id != null)
                {
                    foreach (var id in Id)
                    {
                        WriteObject(identityClient.Clients.Get(id.ToString()));
                    }

                    return;
                }

                if (Name != null)
                {
                    foreach (var name in Name)
                    {
                        WriteObject(identityClient.Clients.List(new ODataQuery<Client>(x=>x.Name == name)).Value, true);
                    }

                    return;
                }

                WriteObject(identityClient.Clients.List().Value, true);

            }

        }

    }
}
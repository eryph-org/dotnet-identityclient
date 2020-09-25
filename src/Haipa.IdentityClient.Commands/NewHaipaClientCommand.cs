using System.Management.Automation;
using Haipa.IdentityClient.Models;

namespace Haipa.IdentityClient.Commands
{
    [Cmdlet(VerbsCommon.New,"HaipaClient", DefaultParameterSetName = "create")]
    [OutputType(typeof(ClientWithSecrets), typeof(Client), ParameterSetName = new []{"create", "createAndSave"})]
    public class NewHaipaClientCommand : IdentityCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            ParameterSetName = "create",
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            ParameterSetName = "createAndSave",
            ValueFromPipelineByPropertyName = true)]
        public string[] Name { get; set; }


        [Parameter(
            Position = 1,
            ParameterSetName = "create",
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            Position = 1,
            ParameterSetName = "createAndSave",
            ValueFromPipelineByPropertyName = true)]
        public string[] AllowedScopes { get; set; }

        [Parameter(
            ParameterSetName = "create",
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            ParameterSetName = "createAndSave",
            ValueFromPipelineByPropertyName = true)]
        public string Description { get; set; }


        [Parameter(
            ParameterSetName = "createAndSave")]
        public SwitchParameter AddToConfiguration { get; set; }

        [Parameter(
            ParameterSetName = "createAndSave")]
        public SwitchParameter AsDefault { get; set; }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            var clientCredentials = GetClientCredentials();
            using (var identityClient = new HaipaIdentityClient(GetCredentials()))
            {
                foreach (var name in Name)
                {
                    var result = identityClient.Clients.Create(new Client
                    {
                        Name = name,
                        Description = Description,
                        AllowedScopes = AllowedScopes
                    });

                    if (AddToConfiguration)
                    {
                        var asDefault = !AsDefault ? "" : " -AsDefault";
                        InvokeCommand.InvokeScript(
                            $@"$args[0] | New-HaipaClientCredentials -Id ""{result.Id}"" -IdentityEndpoint ""{clientCredentials.IdentityProvider}"" -Configuration ""{clientCredentials.Configuration}""
                            | Add-HaipaClientConfiguration -Name ""{result.Name}""{asDefault}", result.Key);

                        WriteObject(new Client(result.Id, result.Name, result.Description, result.AllowedScopes));
                    }
                    else
                        WriteObject(result);



                }

            }
        }

    }
}
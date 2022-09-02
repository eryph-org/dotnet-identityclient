using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Eryph.IdentityClient.Models;
using JetBrains.Annotations;

namespace Eryph.IdentityClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "EryphClient", DefaultParameterSetName = "create")]
    [OutputType(typeof(CreatedClient), typeof(Client), ParameterSetName = new[] {"create", "createAndSave"})]
    public class NewEryphClientCommand : IdentityCmdLet
    {
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            Mandatory = true,
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
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        [Parameter(
            Position = 1,
            Mandatory = true,
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

        protected override void ProcessRecord()
        {
            var clientCredentials = GetClientCredentials();
            using (var identityClient = new EryphIdentityClient(GetEndpointUri(),GetCredentials("identity:clients:write:all")))
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

                        var cmd =
                            $@"$args[0] | New-EryphClientCredentials -Id ""{result.Id}"" -IdentityEndpoint ""{clientCredentials.IdentityProvider}"" -Configuration ""{clientCredentials.Configuration}""| Add-EryphClientConfiguration -Name ""{result.Name}""{asDefault}";

                        var script = InvokeCommand.NewScriptBlock(cmd);
                        script.Invoke(result.Key);
                        
                        WriteObject(new Client(result.Id, result.Name, result.Description, result.AllowedScopes));
                    }
                    else
                        WriteObject(new CreatedClient
                        {
                            Id = result.Id,
                            Name = result.Name,
                            AllowedScopes = result.AllowedScopes.ToArray(),
                            Description = result.Description,
                            IdentityProvider = clientCredentials.IdentityProvider,
                            PrivateKey = result.Key
                        });



                }

            }
        }

    }
}
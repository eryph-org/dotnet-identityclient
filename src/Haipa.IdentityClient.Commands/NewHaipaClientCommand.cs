using System.Linq;
using System.Management.Automation;
using System.Runtime.Serialization;
using System.Security;
using Haipa.IdentityClient.Models;
using JetBrains.Annotations;

namespace Haipa.IdentityClient.Commands
{
    [PublicAPI]
    [Cmdlet(VerbsCommon.New, "HaipaClient", DefaultParameterSetName = "create")]
    [OutputType(typeof(CreatedClient), typeof(Client), ParameterSetName = new[] {"create", "createAndSave"})]
    public class NewHaipaClientCommand : IdentityCmdLet
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
            using (var identityClient = new HaipaIdentityClient(GetCredentials("identity:clients:write:all")))
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
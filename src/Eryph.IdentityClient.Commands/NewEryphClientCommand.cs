using System.Linq;
using System.Management.Automation;
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
            var identityClient = Factory.CreateClientsClient();
            foreach (var name in Name)
            {
                var eryphClient = new Client
                {
                    Name = name,
                    Description = Description
                };
                foreach (var allowedScope in AllowedScopes)
                {
                    eryphClient.AllowedScopes.Add(allowedScope);

                }

                var response = identityClient.Create(eryphClient);
                if (!response.HasValue)
                    return;

                var result = response.Value;

                if (AddToConfiguration)
                {
                    var asDefault = !AsDefault ? "" : " -AsDefault";

                    var cmd =
                        $@"$args[0] | New-EryphClientCredentials -Id ""{result.Id}"" -IdentityEndpoint ""{clientCredentials.IdentityProvider}"" -Configuration ""{clientCredentials.Configuration}""| Add-EryphClientConfiguration -Name ""{result.Name}""{asDefault}";

                    var script = InvokeCommand.NewScriptBlock(cmd);
                    script.Invoke(result.Key);

                    eryphClient = new Client
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Description = result.Description
                    };
                    foreach (var allowedScope in result.AllowedScopes)
                    {
                        eryphClient.AllowedScopes.Add(allowedScope);

                    }

                    WriteObject(eryphClient);
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
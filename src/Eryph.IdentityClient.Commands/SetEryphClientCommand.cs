using System;
using System.Management.Automation;
using Eryph.IdentityClient.Models;
using JetBrains.Annotations;

namespace Eryph.IdentityClient.Commands;

[PublicAPI]
[Cmdlet(VerbsCommon.Set,"EryphClient")]
[OutputType(typeof(Client))]
public class SetEryphClientCommand : IdentityCmdLet
{
    [Parameter(
        Position = 0,
        Mandatory = true,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    [ValidateNotNullOrEmpty]
    public string Id { get; set; }

    [Parameter(
        Mandatory = true,
        ValueFromPipelineByPropertyName = true)]
    public string Name { get; set; }

    [Parameter(
        Mandatory = true,
        ValueFromPipelineByPropertyName = true)]
    public string[] AllowedScopes { get; set; }

    protected override void ProcessRecord()
    {
        var identityClient = Factory.CreateClientsClient();
        try
        {
            _ = identityClient.Get(Id);
        }
        catch (Exception ex)
        {
            WriteError(new ErrorRecord(ex, "EryphClientNotFound", ErrorCategory.ObjectNotFound, Id));
            return;
        }

        var response = identityClient.Update(
            Id,
            new UpdateClientRequestBody(Name, AllowedScopes));

        WriteObject(response.Value);
    }
}

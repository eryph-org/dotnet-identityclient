using Eryph.IdentityClient.Models;
using JetBrains.Annotations;
using System;
using System.Management.Automation;

namespace Eryph.IdentityClient.Commands;

[PublicAPI]
[Cmdlet(VerbsCommon.Remove,"EryphClient")]
[OutputType(typeof(Client))]
public class RemoveEryphClientCommand : IdentityCmdLet
{
    [Parameter(
        Position = 0,
        Mandatory = true,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    public string[] Id { get; set; }

    /// <summary>
    /// This parameter overrides the ShouldContinue call to force
    /// the deletion of the eryph client. This parameter should always
    /// be used with caution.
    /// </summary>
    [Parameter]
    public SwitchParameter Force { get; set; }

    private bool _yesToAll;
    private bool _noToAll;

    protected override void ProcessRecord()
    {
        var identityClient = Factory.CreateClientsClient();
        foreach (var id in Id)
        {
            Client client;
            try
            {
                client = identityClient.Get(id);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "EryphClientNotFound", ErrorCategory.ObjectNotFound, id));
                continue;
            }

            if (!Force && !ShouldContinue($"Eryph client '{client.Name}' (ID: {id}) will be deleted!", "Warning!", ref _yesToAll, ref _noToAll))
                continue;

            identityClient.Delete(id);
        }
    }
}

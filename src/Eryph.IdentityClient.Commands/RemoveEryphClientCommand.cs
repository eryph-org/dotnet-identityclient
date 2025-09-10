using System.Management.Automation;
using Eryph.IdentityClient.Models;
using JetBrains.Annotations;

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
            if (!Force && !ShouldContinue($"Eryph client {id} will be deleted!", "Warning!", ref _yesToAll, ref _noToAll))
                continue;

            identityClient.Delete(id);
        }
    }
}

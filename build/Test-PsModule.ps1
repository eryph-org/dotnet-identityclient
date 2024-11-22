#Requires -Version 7.4
<#
    .SYNOPSIS
        Test the Powershell module.
    .DESCRIPTION
        This script tests the fully packaged Powershell module.
        It is intended to be called by MSBuild during the normal build
        process.
#>
[CmdletBinding()]
param(
    [Parameter()]
    [string]
    [ValidateScript({ $_ -match '[a-zA-Z\.]+' }, ErrorMessage = "The module name '{0}' is invalid.")]
    $ModuleName,
    [Parameter()]
    [string]
    [ValidateScript({ Test-Path $_ }, ErrorMessage = "The path '{0}' is invalid.")]
    $OutputDirectory,
    [Parameter()]
    [string]
    $NuGetPreReleaseTag
)

$PSNativeCommandUseErrorActionPreference = $true
$ErrorActionPreference = 'Stop'

$modulePath = Join-Path $OutputDirectory "PsModule" $ModuleName
$isPrerelease = -not [string]::IsNullOrWhiteSpace($NuGetPreReleaseTag)

# This Powershell module requires the module Eryph.ClientRuntime.Configuration.
# We download that module first to ensure that it is available. Otherwise,
# the import during the test below would fail.
$clientRuntimeModule = Find-Module Eryph.ClientRuntime.Configuration -AllowPrerelease:$isPrerelease
$clientRuntimeVersion = $clientRuntimeModule.Version
# Use a dedicated directory per module version to ensure that we load the correct module.
$dependenciesPath = Join-Path $OutputDirectory "PsModuleDependencies" $clientRuntimeVersion
if (-not (Test-Path $dependenciesPath)) {
    $null = New-Item -ItemType Directory -Path $dependenciesPath
}

$clientRuntimeModulePath = Join-Path $dependenciesPath "Eryph.ClientRuntime.Configuration"
if (-not (Test-Path $clientRuntimeModulePath)) {
    Save-Module -Path $dependenciesPath -Name 'Eryph.ClientRuntime.Configuration' -AllowPrerelease:$isPrerelease
}

# Verify that all Cmdlets are exposed in the manifest. We must load the modules
# in separate Powershell processes to avoid conflicts.
$moduleCmdlets = (powershell.exe -Command "Import-Module $clientRuntimeModulePath; [array](Import-Module -Scope Local $modulePath -PassThru).ExportedCmdlets.Keys -join ','") -split ','
$assemblyCmdlets = (powershell.exe -Command "[array](Import-Module -Scope Local $(Join-Path $modulePath "desktop" "$ModuleName.Commands.dll") -PassThru).ExportedCmdlets.Keys -join ','") -split ','
$missingCmdlets = [Linq.Enumerable]::Except($assemblyCmdlets, $moduleCmdlets)
if ($missingCmdlets.Count -gt 0) {
    throw "The following Cmdlets are not exposed in the module manifest when checking with Windows Powershell: $($missingCmdlets -join ', ')"
}

$moduleCmdlets = (pwsh.exe -Command "Import-Module $clientRuntimeModulePath; [array](Import-Module -Scope Local $modulePath -PassThru).ExportedCmdlets.Keys -join ','") -split ','
$assemblyCmdlets = (pwsh.exe -Command "[array](Import-Module -Scope Local $(Join-Path $modulePath "coreclr" "$ModuleName.Commands.dll") -PassThru).ExportedCmdlets.Keys -join ','") -split ','
$missingCmdlets = [Linq.Enumerable]::Except($assemblyCmdlets, $moduleCmdlets)
if ($missingCmdlets.Count -gt 0) {
    throw "The following Cmdlets are not exposed in the module manifest when checking with Powershell 7: $($missingCmdlets -join ', ')"
}

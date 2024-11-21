#Requires -Version 7.4
<#
    .SYNOPSIS
        Package the Powershell module.
    .DESCRIPTION
        This script packages the Powershell module for distribution.
        It is intended to be called by MSBuild during the normal build
        process. The script will be called once for each target framework.
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
    $TargetPath,
    [Parameter()]
    [string]
    [ValidateScript({ $_ -match 'net\d+\.?\d+' }, ErrorMessage = "The target framework '{0}' is invalid.")]
    $TargetFramework,
    [Parameter()]
    [string]
    [ValidateScript({ Test-Path $_ }, ErrorMessage = "The path '{0}' is invalid.")]
    $OutputDirectory,
    [Parameter()]
    [string]
    [ValidateScript({ $_ -match '\d+\.\d+\.\d+' }, ErrorMessage = "The version '{0}' is invalid.")]
    $MajorMinorPatch,
    [Parameter()]
    [string]
    $NuGetPreReleaseTag
)

$PSNativeCommandUseErrorActionPreference = $true
$ErrorActionPreference = 'Stop'

$excludedFiles = @("System.Management.Automation.dll", "JetBrains.Annotations.dll")

$modulePath = Join-Path $OutputDirectory "PsModule" $ModuleName
$isWindowsPowershell = $TargetFramework -like 'net4*'
$isPrerelease = -not [string]::IsNullOrWhiteSpace($NuGetPreReleaseTag)
$moduleAssemblyPath = Join-Path $modulePath ($isWindowsPowershell  ? 'desktop' : 'coreclr')

# Prepare the output directory
if (-not (Test-Path $modulePath)) {
    $null = New-Item -ItemType Directory -Path $modulePath -ErrorAction SilentlyContinue
}

# Copy the build output
if (Test-Path $moduleAssemblyPath) {
    Remove-Item -Path $moduleAssemblyPath -Force -Recurse
}
$null = New-Item -ItemType Directory -Path $moduleAssemblyPath
$targetDirectory = (Get-Item $TargetPath).Directory.FullName
Copy-Item -Path (Join-Path $targetDirectory "*") -Destination $moduleAssemblyPath -Exclude $excludedFiles -Recurse

# Prepare the module manifest
$config = Get-Content (Join-Path $PSScriptRoot "$ModuleName.psd1") -Raw
$config = $config.Replace("ModuleVersion = '0.1'", "ModuleVersion = '$MajorMinorPatch'");
if ($isPrerelease) {
    $config = $config.Replace("# Prerelease = ''", "Prerelease = '$NuGetPreReleaseTag'");
}
Set-Content -Path (Join-Path $modulePath "$ModuleName.psd1") -Value $config
Copy-Item -Path (Join-Path $PSScriptRoot "$ModuleName.psm1") -Destination $modulePath

# This Powershell module requires the module Eryph.ClientRuntime.Configuration.
# We download that module first to ensure that it is available. Otherwise,
# the import during the test below would fail.
$clientRuntimeModule = Find-Module Eryph.ClientRuntime.Configuration -AllowPrerelease:$isPrerelease
$clientRuntimeVersion = $clientRuntimeModule.Version
$null = New-Item -ItemType Directory -Path (Join-Path $OutputDirectory "PsModuleDependencies" $clientRuntimeVersion) -ErrorAction SilentlyContinue
$clientRuntimeModulePath = Join-Path $OutputDirectory "PsModuleDependencies" $clientRuntimeVersion "Eryph.ClientRuntime.Configuration"
if (-not (Test-Path $clientRuntimeModulePath)) {
    Save-Module -Path (Join-Path $OutputDirectory "PsModuleDependencies" $clientRuntimeVersion) -Name 'Eryph.ClientRuntime.Configuration' -AllowPrerelease:$isPrerelease
}

# Verify that all Cmdlets are exposed in the manifest. We must load the modules
# in separate Powershell processes to avoid conflicts.
$powershell = $isWindowsPowershell ? 'powershell.exe' : 'pwsh.exe'
$moduleCmdlets = (& $powershell -Command "Import-Module $clientRuntimeModulePath; [array](Import-Module -Scope Local $modulePath -PassThru).ExportedCmdlets.Keys -join ','") -split ','
$assemblyCmdlets = (& $powershell -Command "[array](Import-Module -Scope Local $TargetPath -PassThru).ExportedCmdlets.Keys -join ','") -split ','
$missingCmdlets = [Linq.Enumerable]::Except($assemblyCmdlets, $moduleCmdlets)
if ($missingCmdlets.Count -gt 0) {
    throw "The following Cmdlets are not exposed in the module manifest: $($missingCmdlets -join ', ')"
}

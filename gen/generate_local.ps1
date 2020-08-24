Push-Location $PSScriptRoot
$settings = Get-Content -Raw -Path "config.json" | ConvertFrom-Json
cd ..
$location = Get-Location
$tag = $settings.tag
$spec = $settings.spec
npm install autorest@v2
autorest  ..\haipa-api-spec\specification\$spec\ --tag=$tag --csharp-src-folder=$location --use=..\autorest.csharp --csharp --debug
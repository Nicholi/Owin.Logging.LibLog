$psake_version="4.4.2"
nuget.exe install psake -OutputDirectory .\packages -Version "$psake_version" -Source "nuget.org" -Verbosity quiet

Import-Module .\packages\psake.$psake_version\tools\psake.psm1

Invoke-Psake .\default.ps1 "Clean" -framework "4.6x64"

$packageConfigs = Get-ChildItem . -Recurse | where{$_.Name -like "packages.*config"}
foreach($packageConfig in $packageConfigs){
    Write-Host "Restoring" $packageConfig.FullName
    nuget.exe install $packageConfig.FullName -o .\packages
}

Invoke-Psake .\default.ps1 default -framework "4.6x64"

Remove-Module psake

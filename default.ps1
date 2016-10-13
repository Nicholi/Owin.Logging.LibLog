properties {
    $projectName = "Owin.Logging.LibLog"
    $rootDir  = Resolve-Path .\
    $buildOutputDir = "$rootDir\build"
    $solutionFilePath = "$rootDir\$projectName.sln"
    $nuget_path = "nuget.exe"
}

task default -depends CreateNuGetPackage

task Clean {
    Remove-Item $buildOutputDir -Force -Recurse -ErrorAction SilentlyContinue
    exec { msbuild /nologo /verbosity:quiet $solutionFilePath /t:Clean }
}

task Compile {
    exec { msbuild /nologo /verbosity:quiet $solutionFilePath /p:Configuration=Release }
}

task CreatePP {
    if (-Not (Test-Path $buildOutputDir)) {
        New-Item -ItemType Directory -Force -Path $buildOutputDir
    }

    (Get-Content $rootDir\$projectName\$projectName.cs) | Foreach-Object {
        $_ -replace 'YourRootNamespace\.', '$rootnamespace$.'
        } | Set-Content $buildOutputDir\$projectName.cs.pp -Encoding UTF8
}

task CreateNuGetPackage -depends CreatePP {
    if (-Not (Test-Path $buildOutputDir)) {
        New-Item -ItemType Directory -Force -Path $buildOutputDir
    }

    $nuspecFilePath = "$buildOutputDir\$projectName.nuspec"
    Copy-Item $rootDir\$projectName\$projectName.nuspec $nuspecFilePath

    . "$nuget_path" pack $nuspecFilePath -o $buildOutputDir -Verbosity detailed
}

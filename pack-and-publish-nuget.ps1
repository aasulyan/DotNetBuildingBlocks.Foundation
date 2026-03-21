param(
    [string]$SolutionPath = ".\DotNetBuildingBlocks.Foundation.sln",
    [string]$Configuration = "Release",
    [string]$PackageFolder = ".\artifacts\packages",
    [string]$Source = "https://api.nuget.org/v3/index.json",
    [string]$ApiKey = $env:NUGET_API_KEY,
    [switch]$NoBuild,
    [switch]$SkipSymbols,
    [switch]$CleanPackageFolder
)

$ErrorActionPreference = "Stop"

function Test-ApiKey {
    param([string]$Key)

    if ([string]::IsNullOrWhiteSpace($Key)) {
        throw "NuGet API key was not provided. Pass -ApiKey or set the NUGET_API_KEY environment variable."
    }
}

function Test-Solution {
    param([string]$Path)

    if (-not (Test-Path $Path)) {
        throw "Solution file not found: $Path"
    }
}

function Clear-PackageFolder {
    param([string]$Folder)

    if (Test-Path $Folder) {
        Write-Host "Cleaning package folder: $Folder"
        Remove-Item -Path $Folder -Recurse -Force
    }

    New-Item -ItemType Directory -Path $Folder -Force | Out-Null
}

function Pack-Solution {
    param(
        [string]$Path,
        [string]$Config,
        [string]$Folder,
        [bool]$SkipBuild
    )

    Write-Host "Packing solution: $Path"

    $arguments = @(
        "pack"
        $Path
        "-c"
        $Config
        "-o"
        $Folder
    )

    if ($SkipBuild) {
        $arguments += "--no-build"
    }

    dotnet @arguments

    if ($LASTEXITCODE -ne 0) {
        throw "dotnet pack failed."
    }
}

function Push-Packages {
    param(
        [string]$Folder,
        [string]$PackageSource,
        [string]$Key,
        [bool]$PublishSymbols
    )

    $nupkgs = Get-ChildItem -Path $Folder -Filter *.nupkg -File |
        Where-Object { $_.Name -notlike "*.snupkg" } |
        Sort-Object Name

    if (-not $nupkgs) {
        throw "No .nupkg files found in: $Folder"
    }

    Write-Host "Publishing NuGet packages..."
    foreach ($pkg in $nupkgs) {
        Write-Host "Pushing package: $($pkg.Name)"
        dotnet nuget push $pkg.FullName `
            --api-key $Key `
            --source $PackageSource `
            --skip-duplicate

        if ($LASTEXITCODE -ne 0) {
            throw "Failed to push package: $($pkg.Name)"
        }
    }

    if ($PublishSymbols) {
        $snupkgs = Get-ChildItem -Path $Folder -Filter *.snupkg -File |
            Sort-Object Name

        if ($snupkgs) {
            Write-Host "Publishing symbol packages..."
            foreach ($spkg in $snupkgs) {
                Write-Host "Pushing symbol package: $($spkg.Name)"
                dotnet nuget push $spkg.FullName `
                    --api-key $Key `
                    --source $PackageSource `
                    --skip-duplicate

                if ($LASTEXITCODE -ne 0) {
                    throw "Failed to push symbol package: $($spkg.Name)"
                }
            }
        }
        else {
            Write-Host "No .snupkg files found. Skipping symbol publish."
        }
    }
    else {
        Write-Host "Skipping symbol packages."
    }

    Write-Host "Pack and publish completed successfully."
}

Test-ApiKey -Key $ApiKey
Test-Solution -Path $SolutionPath

if ($CleanPackageFolder) {
    Clear-PackageFolder -Folder $PackageFolder
}
elseif (-not (Test-Path $PackageFolder)) {
    New-Item -ItemType Directory -Path $PackageFolder -Force | Out-Null
}

Pack-Solution -Path $SolutionPath -Config $Configuration -Folder $PackageFolder -SkipBuild $NoBuild.IsPresent
Push-Packages -Folder $PackageFolder -PackageSource $Source -Key $ApiKey -PublishSymbols (-not $SkipSymbols.IsPresent)

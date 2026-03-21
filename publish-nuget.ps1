param(
    [string]$PackageFolder = ".\artifacts\packages",
    [string]$Source = "https://api.nuget.org/v3/index.json",
    [string]$ApiKey = $env:NUGET_API_KEY,
    [switch]$SkipSymbols
)

$ErrorActionPreference = "Stop"

function Test-ApiKey {
    param([string]$Key)

    if ([string]::IsNullOrWhiteSpace($Key)) {
        throw "NuGet API key was not provided. Pass -ApiKey or set the NUGET_API_KEY environment variable."
    }
}

function Push-Packages {
    param(
        [string]$Folder,
        [string]$PackageSource,
        [string]$Key,
        [bool]$PublishSymbols
    )

    if (-not (Test-Path $Folder)) {
        throw "Package folder not found: $Folder"
    }

    $nupkgs = Get-ChildItem -Path $Folder -Filter *.nupkg -File |
        Where-Object { $_.Name -notlike "*.snupkg" } |
        Sort-Object Name

    if (-not $nupkgs) {
        throw "No .nupkg files found in: $Folder"
    }

    Write-Host "Publishing NuGet packages from: $Folder"
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
            Write-Host "Publishing symbol packages from: $Folder"
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

    Write-Host "Publish completed successfully."
}

Test-ApiKey -Key $ApiKey
Push-Packages -Folder $PackageFolder -PackageSource $Source -Key $ApiKey -PublishSymbols (-not $SkipSymbols.IsPresent)

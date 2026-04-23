<#
Runs tests inside Docker (requires Docker installed)
#>
param()

$image = "bike-mania-tests:local"
Write-Host "Building Docker image..."
docker build -t $image -f "$(Split-Path -Path $PSScriptRoot -Parent)\ci\Dockerfile" ..

Write-Host "Running tests inside container..."
docker run --rm $image

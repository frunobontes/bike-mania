param(
    [string]$RepoName = "bike-mania",
    [string]$BranchName = "feature/ci-tests-and-tests",
    [string]$BaseBranch = "main",
    [string]$Description = "Bike Mania — core library, tests, CI and Unity glue"
)

# This script uses the environment variable GITHUB_TOKEN for authentication.
if (-not $env:GITHUB_TOKEN) { Write-Error "GITHUB_TOKEN not set in environment."; exit 1 }
$token = $env:GITHUB_TOKEN

$headers = @{ Authorization = "token $token"; "User-Agent" = "opencode-script" }

Write-Host "Querying authenticated user..."
$user = Invoke-RestMethod -Headers $headers -Uri https://api.github.com/user -Method Get
$owner = $user.login
Write-Host "Authenticated as: $owner"

Write-Host "Creating repository '$RepoName' (private)..."
$body = @{ name = $RepoName; private = $true; description = $Description; auto_init = $true } | ConvertTo-Json
try {
    $repo = Invoke-RestMethod -Headers $headers -Uri https://api.github.com/user/repos -Method Post -Body $body
} catch {
    Write-Error "Failed to create repo: $_.Exception.Message"
    exit 1
}

Write-Host "Repository created: $($repo.html_url)"

# Get main branch commit SHA
Write-Host "Retrieving base branch '$BaseBranch' SHA..."
$ref = Invoke-RestMethod -Headers $headers -Uri "https://api.github.com/repos/$owner/$RepoName/git/refs/heads/$BaseBranch" -Method Get
$baseSha = $ref.object.sha

Write-Host "Creating new branch '$BranchName'..."
$newRefBody = @{ ref = "refs/heads/$BranchName"; sha = $baseSha } | ConvertTo-Json
try {
    Invoke-RestMethod -Headers $headers -Uri "https://api.github.com/repos/$owner/$RepoName/git/refs" -Method Post -Body $newRefBody
} catch {
    Write-Warning "Could not create ref (may already exist): $_.Exception.Message"
}

Write-Host "Uploading files to branch '$BranchName'..."

$root = Get-Location
Get-ChildItem -Recurse -File | Where-Object { $_.FullName -notmatch '\.git' } | ForEach-Object {
    $file = $_
    $rel = $file.FullName.Substring($root.Path.Length+1).Replace('\','/')
    Write-Host "Uploading $rel ..."
    $bytes = [System.IO.File]::ReadAllBytes($file.FullName)
    $content = [System.Convert]::ToBase64String($bytes)
    $msg = "Add $rel"
    $putBody = @{ message = $msg; content = $content; branch = $BranchName } | ConvertTo-Json
    $uri = "https://api.github.com/repos/$owner/$RepoName/contents/$rel"
    try {
        Invoke-RestMethod -Headers $headers -Uri $uri -Method Put -Body $putBody
    } catch {
        Write-Warning "Failed to upload $rel: $_.Exception.Message"
    }
}

Write-Host "Creating Pull Request from $BranchName to $BaseBranch..."
$prBody = @{ title = "Add CI workflow, docker test runner and extended tests"; head = "$owner:$BranchName"; base = $BaseBranch; body = "This PR adds CI workflow, docker test runner and extended unit tests, and helper scripts." } | ConvertTo-Json
$pr = Invoke-RestMethod -Headers $headers -Uri "https://api.github.com/repos/$owner/$RepoName/pulls" -Method Post -Body $prBody
Write-Host "Pull Request created: $($pr.html_url)"

Write-Host "Done. Repository: $($repo.html_url)";

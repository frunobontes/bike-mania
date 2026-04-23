param(
    [string]$RepoName = "bike-mania",
    [string]$BranchName = "feature/ci-tests-and-tests",
    [string]$BaseBranch = "main",
    [string]$CommitMessage = "chore(ci): add CI workflow, docker test runner and extended unit tests",
    [string]$PrTitle = "Add CI workflow, docker test runner and extended tests",
    [string]$PrBody = "This PR adds a GitHub Actions workflow to run dotnet tests, extended unit tests, a Docker test runner and helper scripts."
)

function ExitWith($msg) { Write-Host $msg -ForegroundColor Red; exit 1 }

# Check gh
try { gh --version > $null } catch { ExitWith "GitHub CLI 'gh' not found. Install it (winget install --id GitHub.cli -e) and run 'gh auth login' before proceeding." }

Write-Host "Ensure you're authenticated with GitHub CLI (gh). If not, run: gh auth login" -ForegroundColor Yellow

# Ensure running in repository folder (script expects to be run from Bike Mania folder)
$cwd = Get-Location
Write-Host "Working directory: $cwd"

# Initialize git if needed
if (-not (Test-Path .git)) {
    Write-Host "Initializing git repository..."
    git init
}

Write-Host "Creating and switching to branch $BranchName"
git checkout -b $BranchName 2>$null

Write-Host "Staging changes..."
git add -A

Write-Host "Committing..."
try {
    git commit -m "$CommitMessage" -q
} catch {
    Write-Host "No changes to commit or commit failed; continuing..." -ForegroundColor Yellow
}

Write-Host "Creating remote repository and pushing..."
try {
    gh repo create $RepoName --private --source . --remote origin --push --confirm
} catch {
    Write-Host "gh repo create failed; maybe repository already exists or network error. Attempting to add remote and push." -ForegroundColor Yellow
    # Try to determine remote URL
    $userInfo = gh api user --jq .login 2>$null
    if ($userInfo) { $remoteUrl = "https://github.com/$userInfo/$RepoName.git" } else { ExitWith "Cannot determine GitHub user. Run 'gh auth login' and retry." }
    git remote add origin $remoteUrl 2>$null
    git push -u origin $BranchName
}

Write-Host "Creating Pull Request..."
try {
    gh pr create --title "$PrTitle" --body "$PrBody" --base $BaseBranch --head $BranchName
    Write-Host "Pull request created. Open it with: gh pr view --web" -ForegroundColor Green
} catch {
    Write-Host "Failed to create PR via gh. You can create it manually or check 'gh auth status'." -ForegroundColor Red
}

Write-Host "Done. If everything succeeded, check the repository on GitHub and the PR created." -ForegroundColor Green

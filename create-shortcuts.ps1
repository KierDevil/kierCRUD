$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$publishDir = Join-Path $root "publish\KierCRUD"
$launcherPath = Join-Path $publishDir "Kier CRUD.vbs"
$iconPath = Join-Path $publishDir "kier_crud.ico"

if (-not (Test-Path -LiteralPath $launcherPath)) {
    throw "Launcher not found. Run publish-windows.cmd first."
}

$shell = New-Object -ComObject WScript.Shell
$desktopPath = [Environment]::GetFolderPath("DesktopDirectory")
$programsPath = [Environment]::GetFolderPath("Programs")

if ([string]::IsNullOrWhiteSpace($desktopPath)) {
    $desktopPath = Join-Path $env:USERPROFILE "Desktop"
}

if ([string]::IsNullOrWhiteSpace($programsPath)) {
    $programsPath = Join-Path $env:APPDATA "Microsoft\Windows\Start Menu\Programs"
}

New-Item -ItemType Directory -Force -Path $desktopPath | Out-Null
New-Item -ItemType Directory -Force -Path $programsPath | Out-Null

$shortcuts = @(
    @{
        Path = Join-Path $desktopPath "Kier CRUD.lnk"
    },
    @{
        Path = Join-Path $programsPath "Kier CRUD.lnk"
    }
)

foreach ($item in $shortcuts) {
    $shortcut = $shell.CreateShortcut($item.Path)
    $shortcut.TargetPath = Join-Path $env:WINDIR "System32\wscript.exe"
    $shortcut.Arguments = "`"$launcherPath`""
    $shortcut.WorkingDirectory = $publishDir
    $shortcut.Description = "Kier CRUD"

    if (Test-Path -LiteralPath $iconPath) {
        $shortcut.IconLocation = "$iconPath,0"
    }

    $shortcut.Save()
}

Write-Host "Shortcuts created:"
Write-Host "- Desktop: Kier CRUD"
Write-Host "- Start Menu: Kier CRUD"

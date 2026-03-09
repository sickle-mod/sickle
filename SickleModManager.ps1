Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

# --- HELPERS ---------------------------------------------------------------

function Get-ScythePath {
    $steamPath = "C:\Program Files (x86)\Steam\steamapps\common\Scythe Digital Edition\Scythe_Data\Managed"
    $gogPath   = "C:\Program Files (x86)\GOG Galaxy\Games\Scythe Digital Edition\Scythe_Data\Managed"

    if (Test-Path "$steamPath\Assembly-CSharp.dll") { return $steamPath }
    if (Test-Path "$gogPath\Assembly-CSharp.dll")   { return $gogPath }

    return $null
}

function Get-InstalledVersion {
    $versionFile = "$env:LOCALAPPDATA\SickleMod\installed_version.txt"
    if (Test-Path $versionFile) {
        return (Get-Content $versionFile -ErrorAction SilentlyContinue).Trim()
    }
    return $null
}

function Get-LatestVersion {
    param($ApiUrl)

    try {
        $release = Invoke-RestMethod -Uri $ApiUrl -Headers @{ "User-Agent" = "PowerShell" }
        return $release.tag_name
    }
    catch {
        return $null
    }
}

function Compare-Versions {
    param($installed, $latest)

    if (-not $installed) { return "No mod installed" }
    if (-not $latest)    { return "Unable to check latest version" }

    if ($installed -eq $latest) { return "Up to date" }
    return "Update available"
}

# --- CONSTANTS -------------------------------------------------------------

$LocalModDir = "$env:LOCALAPPDATA\SickleMod"
$BackupDll   = "$LocalModDir\Assembly-CSharp.dll"
$LogDir      = "$LocalModDir\logs"
$DownloadApi = "https://api.github.com/repos/sickle-mod/sickle/releases/latest"
$InstalledVersionFile = "$LocalModDir\installed_version.txt"

# --- GUI SETUP -------------------------------------------------------------

$form = New-Object System.Windows.Forms.Form
$form.Text = "Sickle Mod Manager"
$form.Size = New-Object System.Drawing.Size(480,230)
$form.StartPosition = "CenterScreen"

$status = New-Object System.Windows.Forms.Label
$status.AutoSize = $true
$status.Location = New-Object System.Drawing.Point(20,140)
$status.Text = "Ready."
$form.Controls.Add($status)

$versionLabel = New-Object System.Windows.Forms.Label
$versionLabel.AutoSize = $true
$versionLabel.Location = New-Object System.Drawing.Point(20,165)
$versionLabel.Text = "Checking versions..."
$form.Controls.Add($versionLabel)

# --- BROWSE BUTTON ---------------------------------------------------------

$browseBtn = New-Object System.Windows.Forms.Button
$browseBtn.Text = "Browse to Game Folder"
$browseBtn.Size = New-Object System.Drawing.Size(150,30)
$browseBtn.Location = New-Object System.Drawing.Point(20,90)

$browseBtn.Add_Click({
    $dialog = New-Object System.Windows.Forms.FolderBrowserDialog
    $dialog.Description = "Select Scythe_Data\Managed folder"
    if ($dialog.ShowDialog() -eq "OK") {
        $script:ManualGamePath = $dialog.SelectedPath
        $status.Text = "Manual path set: $($script:ManualGamePath)"
    }
})

$form.Controls.Add($browseBtn)

# --- INSTALL / UPDATE BUTTON ----------------------------------------------

$installBtn = New-Object System.Windows.Forms.Button
$installBtn.Text = "Install / Update"
$installBtn.Size = New-Object System.Drawing.Size(150,40)
$installBtn.Location = New-Object System.Drawing.Point(20,30)

$installBtn.Add_Click({

    # Determine game path
    $GamePath = Get-ScythePath
    if (-not $GamePath -and $script:ManualGamePath) { $GamePath = $script:ManualGamePath }
    if (-not $GamePath) {
        [System.Windows.Forms.MessageBox]::Show("Game path not found. Use 'Browse Game Folder'.")
        return
    }

    $GameDll = "$GamePath\Assembly-CSharp.dll"

    if (-not (Test-Path $GameDll)) {
        [System.Windows.Forms.MessageBox]::Show("Assembly-CSharp.dll not found in selected folder.")
        return
    }

    # 1. Create mod folder
    if (!(Test-Path $LocalModDir)) { New-Item -ItemType Directory -Path $LocalModDir | Out-Null }

    # 2. Backup original DLL (only once)
    if (!(Test-Path $BackupDll)) {
        Copy-Item $GameDll $BackupDll -ErrorAction SilentlyContinue
    }

    # 3. Download latest release asset
    try {
        $release = Invoke-RestMethod -Uri $DownloadApi -Headers @{ "User-Agent" = "PowerShell" }
        $asset = $release.assets | Where-Object { $_.name -eq "Assembly-CSharp.dll" }

        if (-not $asset) {
            [System.Windows.Forms.MessageBox]::Show("Release asset 'Assembly-CSharp.dll' not found in latest release.")
            return
        }

        Invoke-WebRequest -Uri $asset.browser_download_url -OutFile $GameDll -UseBasicParsing

        # Save installed version
        Set-Content -Path $InstalledVersionFile -Value $release.tag_name
    }
    catch {
        [System.Windows.Forms.MessageBox]::Show("Failed to download latest release: $($_.Exception.Message)")
        return
    }

    # 4. Create logs folder
    if (!(Test-Path $LogDir)) { New-Item -ItemType Directory -Path $LogDir | Out-Null }

    # 5. Create desktop shortcut to Sickle mod folder
    $ShortcutPath = "$([Environment]::GetFolderPath('Desktop'))\SickleMod.lnk"
    if (!(Test-Path $ShortcutPath)) {
        $WScriptShell = New-Object -ComObject WScript.Shell
        $Shortcut = $WScriptShell.CreateShortcut($ShortcutPath)
        $Shortcut.TargetPath = $LocalModDir
        $Shortcut.Save()
    }

    $status.Text = "Mod installed/updated successfully."

    # Refresh version info
    $installed = Get-InstalledVersion
    $latest    = Get-LatestVersion -ApiUrl $DownloadApi
    $result    = Compare-Versions -installed $installed -latest $latest
    $versionLabel.Text = "Installed: $installed   Latest: $latest   Status: $result"
})

$form.Controls.Add($installBtn)

# --- UNINSTALL BUTTON ------------------------------------------------------

$uninstallBtn = New-Object System.Windows.Forms.Button
$uninstallBtn.Text = "Uninstall"
$uninstallBtn.Size = New-Object System.Drawing.Size(150,40)
$uninstallBtn.Location = New-Object System.Drawing.Point(200,30)

$uninstallBtn.Add_Click({

    $GamePath = Get-ScythePath
    if (-not $GamePath -and $script:ManualGamePath) { $GamePath = $script:ManualGamePath }
    if (-not $GamePath) {
        [System.Windows.Forms.MessageBox]::Show("Game path not found. Use 'Browse Game Folder'.")
        return
    }

    $GameDll = "$GamePath\Assembly-CSharp.dll"

    if (Test-Path $BackupDll) {
        Copy-Item $BackupDll $GameDll -Force
        $status.Text = "Original DLL restored. Mod uninstalled."

        # Clear installed version info
        if (Test-Path $InstalledVersionFile) { Remove-Item $InstalledVersionFile -Force }

        $installed = Get-InstalledVersion
        $latest    = Get-LatestVersion -ApiUrl $DownloadApi
        $result    = Compare-Versions -installed $installed -latest $latest
        $versionLabel.Text = "Installed: $installed   Latest: $latest   Status: $result"
    } else {
        [System.Windows.Forms.MessageBox]::Show("Backup not found. Cannot restore original DLL.")
    }
})

$form.Controls.Add($uninstallBtn)

# --- INITIAL VERSION CHECK -------------------------------------------------

$form.Add_Shown({
    $installed = Get-InstalledVersion
    $latest    = Get-LatestVersion -ApiUrl $DownloadApi
    $result    = Compare-Versions -installed $installed -latest $latest
    $versionLabel.Text = "Installed: $installed   Latest: $latest   Status: $result"
})

# --- RUN GUI ---------------------------------------------------------------

$form.Topmost = $true
$form.Add_Shown({ $form.Activate() })

[void]$form.ShowDialog()

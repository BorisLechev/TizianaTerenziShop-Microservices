$count = 0
$started = $false

do {
    $count++
    Write-Output "[$env:STAGE_NAME] Attempt: $count"

    try {
        $response = Invoke-WebRequest -Uri http://localhost:5001 -UseBasicParsing

        if ($response.StatusCode -eq 200) {
            $started = $true
        }
    } catch {
        Start-Sleep -Seconds 3
    }

} until ($started -or ($count -eq 20))

if (!$started) {
    Write-Error "Service did not start"
    exit 1
}
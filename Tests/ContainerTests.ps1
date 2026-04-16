function Wait-ForService {
    param (
        [string]$url,
        [int]$maxAttempts = 20
    )

    $count = 0

    do {
        $count++
        Write-Output "Waiting for $url (Attempt $count)"

        try {
            $response = Invoke-WebRequest -Uri $url -UseBasicParsing
            if ($response.StatusCode -eq 200) {
                return $true
            }
        } catch {
			if ($_.Exception.Response) {
				$statusCode = $_.Exception.Response.StatusCode.value__
				Write-Output "StatusCode: $statusCode"
			}
		
			Write-Output $_.Exception.Message
			Write-Error "Service not started"
		}

        Start-Sleep -Seconds 3

    } until ($count -ge $maxAttempts)

    return $false
}

function Test-Login-User-Process {
    $body = @{
        emailOrUserName = "admin@admin.com"
        password = "123456"
    } | ConvertTo-Json

    try {
        $response = Invoke-RestMethod `
            -Uri "http://localhost:5003/Identity/Login" `
            -Method Post `
            -Body $body `
            -ContentType "application/json"

        if ($response.data.token) {
            Write-Host "Login successful :)"
            return $response.data.token
        } else {
            throw "No token returned :("
        }

    } catch {
        Write-Error "Login failed :("
        exit 1
    }
}

function Test-AuthorizedEndpoint {
    param (
        [string]$token
    )

    try {
        $response = Invoke-RestMethod `
            -Uri "http://localhost:5007/Carts/Index" `
            -Headers @{ Authorization = "Bearer $token" } `
            -Method Get

        Write-Output "Carts endpoint OK"
    } catch {
        if ($_.Exception.Response) {
			$statusCode = $_.Exception.Response.StatusCode.value__
			Write-Output "StatusCode: $statusCode"
		}
		
		Write-Output $_.Exception.Message
		Write-Error "Carts endpoint failed"
		exit 1
    }
}

if (-not (Wait-ForService "http://localhost:5001")) {
    Write-Error "WebClient not started"
    exit 1
}

if (-not (Wait-ForService "http://localhost:5003/health")) {
    Write-Error "Identity not started"
    exit 1
}

$token = Test-Login-User-Process

Test-AuthorizedEndpoint -token $token

Write-Output "All integration tests passed"
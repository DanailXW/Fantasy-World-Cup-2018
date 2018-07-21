#Import-Module "C:\Users\Danail\Downloads\htmlagilitypack.1.8.4\lib\Net45\HtmlAgilityPack.dll"
Add-Type -Path "C:\Users\Danail\Downloads\htmlagilitypack.1.8.4\lib\Net45\HtmlAgilityPack.dll"
$url = "https://www.flashscore.com/match/external_id/#match-summary"
$url2 = "https://d.flashscore.com/x/feed/d_su_external_id_en_1"
#$cred = Get-AutomationPSCredential -Name "SQLlogin"
#$Result = Invoke-Sqlcmd -Query "SELECT * FROM dbo.vw_Game_Started" -ServerInstance "hoodahelll.database.windows.net" -UserName $Cred.UserName -Password $Cred.GetNetworkCredential().Password -Database "FantasyCup"
$Result = Invoke-Sqlcmd -Query "SELECT * FROM dbo.vw_Game_Started" -ServerInstance "HOODAHELL-PC\MSSQL16" -Database "FantasyCup"

foreach ($row in $Result){ 
    
    Write-Output $row.ExternalId
    $htmlWeb = New-Object -TypeName HtmlAgilityPack.HtmlWeb
    $doc = $htmlWeb.Load($url.replace("external_id", $row.ExternalId))
    $game_state = $doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'info-status')]").InnerText.Trim()
    Write-Output $game_state
    
    If ($game_state -eq 'Finished' -or $game_state -eq 'After Extra Time' -or $game_state -eq 'After Penalties')
    {
        If ($game_state -eq 'Finished')
        {
            $result_stage = "REGULAR"
        }
        Elseif ($game_state -eq 'After Extra Time')
        {
            $result_stage = "AET"
        }
        Elseif ($game_state -eq 'After Penalties')
        {
            $result_stage = "PENALTIES"
        }

        $match_details = $doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'detailMS')]")
        If (!$match_details)
        {
            $web = New-Object System.Net.WebClient
            $web.Headers.Add("X-Fsign", "SW9D1eZo")
            $htmlDoc2 = $web.DownloadString($url2.replace("external_id", $row.ExternalId))
            $doc2 = New-Object HtmlAgilityPack.HtmlDocument
            $doc2.LoadHtml($htmlDoc2)
            $match_details = $doc2.DocumentNode.SelectSingleNode("//div[contains(@class, 'detailMS')]")
        }

        $match_lines = $match_details.SelectNodes("div[contains(@class, 'detailMS__incidentsHeader')]")
        $scores = $match_details.SelectSingleNode("div[contains(@class, 'detailMS__incidentsHeader') and ./div[text() = '1st Half']]").SelectSingleNode("div[@class='detailMS__headerScore']")
        $scoreA = $scores.SelectSingleNode("span[@class = 'p1_home']").InnerText.Trim()
        $scoreB = $scores.SelectSingleNode("span[@class = 'p1_away']").InnerText.Trim()
        $scores = $match_details.SelectSingleNode("div[contains(@class, 'detailMS__incidentsHeader') and ./div[text() = '2nd Half']]").SelectSingleNode("div[@class='detailMS__headerScore']")
        $scoreA = [int]$scoreA + $scores.SelectSingleNode("span[@class = 'p2_home']").InnerText.Trim()
        $scoreB = [int]$scoreB + $scores.SelectSingleNode("span[@class = 'p2_away']").InnerText.Trim()
        
        Invoke-Sqlcmd -Query "EXEC dbo.usp_ImportGameResult $($row.id), $($scoreA), $($scoreB), 'REGULAR', $($result_stage)" -ServerInstance "HOODAHELL-PC\MSSQL16" -Database "FantasyCup"

        If ($game_state -eq 'After Extra Time' -or $game_state -eq 'After Penalties')
        {
            $scores = $match_details.SelectSingleNode("div[contains(@class, 'detailMS__incidentsHeader') and ./div[text() = 'Extra Time']]").SelectSingleNode("div[@class='detailMS__headerScore']")
            $scoreA = $scores.SelectSingleNode("span[@class = 'p3_home']").InnerText.Trim()
            $scoreB = $scores.SelectSingleNode("span[@class = 'p3_away']").InnerText.Trim()
    
            Invoke-Sqlcmd -Query "EXEC dbo.usp_ImportGameResult $($row.id), $($scoreA), $($scoreB), 'AET', $($result_stage)" -ServerInstance "HOODAHELL-PC\MSSQL16" -Database "FantasyCup"

        }

        If ($game_state -eq 'After Penalties')
        {
            $scores = $match_details.SelectSingleNode("div[contains(@class, 'detailMS__incidentsHeader') and ./div[text() = 'Penalties']]").SelectSingleNode("div[@class='detailMS__headerScore']")
            $scoreA = $scores.SelectSingleNode("span[@class = 'p4_home']").InnerText.Trim()
            $scoreB = $scores.SelectSingleNode("span[@class = 'p4_away']").InnerText.Trim()
    
            Invoke-Sqlcmd -Query "EXEC dbo.usp_ImportGameResult $($row.id), $($scoreA), $($scoreB), 'PENALTIES', $($result_stage)" -ServerInstance "HOODAHELL-PC\MSSQL16" -Database "FantasyCup"

        }
        

	}
	
}
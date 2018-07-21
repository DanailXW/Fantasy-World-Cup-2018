Import-Module "C:\Modules\User\HtmlAgilityPack\HtmlAgilityPack.dll"
$url = "https://www.flashscore.com/match/external_id/#match-summary"
$cred = Get-AutomationPSCredential -Name "SQLlogin"
$Result = Invoke-Sqlcmd -Query "SELECT * FROM dbo.vw_Game_Started" -ServerInstance "hoodahelll.database.windows.net" -UserName $Cred.UserName -Password $Cred.GetNetworkCredential().Password -Database "FantasyCup"
foreach ($row in $Result){ 
    
    Write-Output $row.ExternalId
    $htmlWeb = New-Object -TypeName HtmlAgilityPack.HtmlWeb
    $doc = $htmlWeb.Load($url.replace("external_id", $row.ExternalId))    
    $game_state = $doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'info-status')]").InnerText.Trim();
    Write-Output $game_state
    
    If ($game_state -eq 'Finished')
    {
        $scores = $doc.DocumentNode.SelectNodes("//span[@class = 'scoreboard']")
        $scoreA = $scores[0].InnerText.Trim()
        $scoreB = $scores[1].InnerText.Trim()
        Write-Output $scoreA
        Write-Output $scoreB
        Invoke-Sqlcmd -Query "EXEC dbo.usp_ImportGameResult $($row.id), $($scoreA), $($scoreB)" -ServerInstance "hoodahelll.database.windows.net" -UserName "hood_root" -Password "h00d_r00t" -Database "FantasyCup"
    }
}

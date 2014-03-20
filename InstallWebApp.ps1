Import-Module WebAdministration;

function UpdateHostsFile($webSiteName)
{
    $hostFile = 'c:\Windows\System32\drivers\etc\hosts';
    $hostFileContent = Get-Content $hostFile;
    if(-not ($hostFileContent -match $webSiteName) )
    {
        $hostFileContent += "127.0.0.1       " + $webSiteName;
        $hostFileContent | Out-File $hostFile -Encoding ascii;
    }
}

function CreateApplicationPool($appPoolName)
{
    $appPool = Get-ChildItem IIS:\AppPools | Where-Object {$_.Name -eq $appPoolName };
    if( $appPool -eq $null)
    {
        $appPool = New-Item iis:\AppPools\$appPoolName;
    }
    Set-ItemProperty iis:\AppPools\$appPoolName -name managedRuntimeVersion -Value 'v4.0';
    Set-ItemProperty iis:\AppPools\$appPoolName -name processModel -value @{identitytype=2};

    return $appPool;
}

function CreateWebSite($sbWebSiteName, $sbAppPoolName, $webSiteDir, $binding)
{
    $webSite = Get-ChildItem IIS:\Sites | Where-Object { $_.Name -eq $sbWebSiteName };
    if( $webSite -eq $null)
    {
        $webSite = New-Item iis:\Sites\$sbWebSiteName -physicalPath $webSiteDir -bindings @{protocol="http";bindingInformation=$binding};
    }

    Set-ItemProperty IIS:\Sites\$sbWebSiteName -name applicationPool -value $sbAppPoolName;

    return $webSite;
}

function InstallWebSite($webSiteName, $webSiteDir)
{
	UpdateHostsFile $webSiteName;

	#Install Application pool
	$webSitePoolName = $webSiteName;
	$webAppPool = CreateApplicationPool $webSitePoolName;

	#Install WebSite
	$webBinding = "*:80:" + $webSiteName;

	$sbWebSite = CreateWebSite $webSiteName $webSitePoolName $webSiteDir $webBinding;
}

#Get solution folder
$dirPath = (get-item $MyInvocation.MyCommand.Path).DirectoryName + '\WinDbg';

InstallWebSite 'windbg.local' $dirPath
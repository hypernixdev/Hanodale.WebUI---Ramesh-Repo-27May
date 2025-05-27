param($installPath, $toolsPath, $package, $project)

function GetFullFunctionName($projectItem, $functionName) {
	Write-Host $functionName
	$namespace = $projectItem.FileCodeModel.CodeElements | where {$_.Kind -eq 5}
Write-Host $namespace
	$methods = $namespace.Children | % {$_.Children}
	$registerRoutesFunction = $methods | where {$_.Name -eq $functionName};
	Write-Host $registerRoutesFunction.FullName
	$registerRoutesFunction.FullName;
}
function FindRegisterRouteFunction($project)
{
Write-Host "1"
	$AppStart = $project.ProjectItems | where {$_.Name -eq 'App_Start'}
Write-Host "2"
	if ($AppStart -ne $null) {
	Write-Host "3"
		$routeConfig = $AppStart.ProjectItems | where {$_.Name -eq 'RouteConfig.cs'}
		Write-Host "4"
		if ($routeConfig -ne $null) {
		Write-Host "5"
		$result = GetFullFunctionName $routeConfig 'RegisterRoutes'
		Write-Host "__"+$result
			return $result
		}
	}
			Write-Host "1.1"	
	$asax = $project.ProjectItems | where {$_.Name -eq 'Global.asax'}
	$asax = $asax.ProjectItems | where {$_.Name -eq 'Global.asax.cs'}
	Write-Host "1.3"	
	return GetFullFunctionName($asax, 'RegisterRoutes')
}

function ReplaceFileContents($project, $filename, $toReplace, $replaceWith) {
	$projectDir = [io.path]::GetDirectoryName($project.FullName)
	$filePath = [io.path]::Combine($projectDir, "Scripts\T4MvcJs\" + $filename)
	$contents = [io.file]::ReadAllText($filePath)
	$contents = $contents.Replace($toReplace, $replaceWith)

	[io.file]::WriteAllText($filePath, $contents)
}
Write-Host "00"
$registerRouteFunction = FindRegisterRouteFunction($project)
Write-Host "01"
Write-Host $registerRouteFunction 
if (![string]::IsNullOrEmpty($registerRouteFunction)) {
Write-Host "012"
	$registerRouteFunction = $registerRouteFunction + "(routes);"
}
Write-Host "02"
ReplaceFileContents $project "T4Proxy.cs" "//#RouteConfig.RegisterRoutes(routes);#" $registerRouteFunction

$packageFolder = $package.ToString().Replace(' ', '.')
Write-Host "04"

$dllPath = '$(SolutionDir)packages\' + $packageFolder + '\tools\T4MvcJs.RoutesHandler.dll'
Write-Host "06"
ReplaceFileContents $project 'T4MvcJs.tt' '$(ProjectDir)$(OutDir)T4MvcJs.RoutesHandler.dll' $dllPath
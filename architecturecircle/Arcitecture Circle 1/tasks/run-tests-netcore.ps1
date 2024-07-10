$env:CoreLibPath = '..\..\..\.application\net-core\core-bin'
$env:TestCoreLibPath = '..\..\.application\net-core\core-bin'
$env:RelativePkgFolderPath = '..\..\..\.application\net-core\packages'
$env:CoreTargetFramework = 'netstandard2.0'
$today = Get-Date -Format "yyyy-MM-dd"

dotnet build-server shutdown;
dotnet test --configuration Release `
	.\..\tests\AtfRecordBoost\AtfRecordBoost.Tests.csproj `
	/p:CollectCoverage=true `
	/p:CoverletOutputFormat=opencover `
	/p:Exclude="[Terrasoft*]*" `
	/p:CoverletOutput=..\Results\net-core\$today\AtfRecordBoost.opencover.xml;

reportgenerator -reports:.\..\tests\Results\net-core\$today\AtfRecordBoost.opencover.xml `
	-targetdir:.\..\TestResults\unit-tests\net-core\$today\AtfRecordBoost `
	-historydir:.\..\TestResults\unit-tests\net-core `
	"-assemblyfilters:+AtfRecordBoost;-Terrasoft.Configuration.*";

LiveReloadServer .\..\TestResults\unit-tests\net-core\$today\AtfRecordBoost `
	--LiveReloadEnabled True `
	--OpenBrowser;
$env:CoreLibPath = '..\..\..\.application\net-framework\core-bin'
$env:TestCoreLibPath = '..\..\.application\net-framework\core-bin'
$env:RelativePkgFolderPath = '..\..\..\.application\net-framework\packages'
$env:CoreTargetFramework = 'net472'
$today = Get-Date -Format "yyyy-MM-dd"

dotnet build-server shutdown;
dotnet test --configuration Release `
	.\..\tests\AtfRecordBoost\AtfRecordBoost.Tests.csproj `
	/p:CollectCoverage=true `
	/p:CoverletOutputFormat=opencover `
	/p:Exclude="[Terrasoft*]*" `
	/p:CoverletOutput=..\Results\net-framework\$today\AtfRecordBoost.opencover.xml;


reportgenerator -reports:.\..\tests\Results\net-framework\$today\AtfRecordBoost.opencover.xml `
	 -targetdir:.\..\TestResults\unit-tests\net-framework\$today\AtfRecordBoost `
	 -historydir:.\..\TestResults\unit-tests\net-framework "-assemblyfilters:+AtfRecordBoost;-Terrasoft.Configuration.*";

LiveReloadServer .\..\TestResults\unit-tests\net-framework\$today\AtfRecordBoost `
	--LiveReloadEnabled True `
	--OpenBrowser;
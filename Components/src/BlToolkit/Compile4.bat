..\Tools\SvnRevision\SvnRevision.exe .. Templates\BLToolkitConstants.Revision.cs.template Properties\Revision.generated.cs

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /target:Clean BLToolkit.4.csproj /property:Configuration=Debug
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /target:Clean BLToolkit.4.csproj /property:Configuration=Release
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe BLToolkit.4.csproj /property:Configuration=Debug
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe BLToolkit.4.csproj /property:Configuration=Release

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /target:Clean BLToolkit.SL.4.csproj /property:Configuration=Debug
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /target:Clean BLToolkit.SL.4.csproj /property:Configuration=Release
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe BLToolkit.SL.4.csproj /property:Configuration=Debug
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe BLToolkit.SL.4.csproj /property:Configuration=Release

pause
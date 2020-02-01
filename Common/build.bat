dotnet restore
dotnet build Shipwreck.sln --configuration Release
REM dotnet test Shipwreck.Test/Shipwreck.Test.csproj
dotnet publish Shipwreck.sln --configuration Release
copy Shipwreck\bin\Release\netstandard2.0\publish\*.dll ..\Shipwreck\Assets\Libraries

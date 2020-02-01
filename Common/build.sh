#!/bin/sh

dotnet restore
dotnet build Shipwreck.sln --configuration Release
# dotnet test Shipwreck.Test/Shipwreck.Test.csproj
dotnet publish Shipwreck.sln --configuration Release
cp Shipwreck/bin/Release/netstandard2.0/publish/*.dll ../Shipwreck/Assets/Libraries/

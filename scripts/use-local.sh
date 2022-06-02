#!/bin/sh -e
if ! dotnet nuget list source --format Short | grep -q '/home/.*/.nuget/local'; then
	dotnet nuget add source ~/.nuget/local -n local
fi
cd `dirname $0`/../../sourceafis-net
dotnet pack -c Release
rm -rf ~/.nuget/local
mkdir -p ~/.nuget/local
dotnet nuget push */bin/Release/*.nupkg -s ~/.nuget/local
rm -rf ~/.nuget/packages/sourceafis


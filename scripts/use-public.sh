#!/bin/sh -e
rm -rf ~/.nuget/local
mkdir -p ~/.nuget/local
rm -rf ~/.nuget/packages/sourceafis
cd `dirname $0`/..
dotnet restore


#!/bin/sh -e
# Bin directories are normally under individual projects, but we need location that is easy to discover for build tools.
ARCHIVE=SourceAFIS.Cli/bin/archive
mkdir -p $ARCHIVE
# Test that the CLI works at all.
EXE="dotnet run --project SourceAFIS.Cli -c Release"
$EXE version
# Check fast output first.
$EXE >$ARCHIVE/help.txt
$EXE version >$ARCHIVE/version.txt
# This will produce progress messages from the CLI.
$EXE benchmark
# Run again to get clean benchmark output.
$EXE benchmark >$ARCHIVE/benchmark.txt


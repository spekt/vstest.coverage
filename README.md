# vstest.coverage

[![Build Status](https://travis-ci.com/spekt/vstest.coverage.svg?branch=master)](https://travis-ci.com/spekt/vstest.coverage)
[![Build status](https://ci.appveyor.com/api/projects/status/gds2rdqvixk5q6q5?svg=true)](https://ci.appveyor.com/project/spekt/vstest-coverage)
![Maintenance](https://img.shields.io/maintenance/no/2018)

## Packages
| Logger | Stable Package | Pre-release Package |
| ------ | -------------- | ------------------- |
| Spekt.VSTest.Coverage.Report | [![NuGet](https://img.shields.io/nuget/v/Spekt.VSTest.Coverage.Report.svg)](https://www.nuget.org/packages/Spekt.VSTest.Coverage.Report/) | [![MyGet Pre Release](https://img.shields.io/myget/spekt/vpre/Spekt.VSTest.Coverage.Report.svg)](https://www.myget.org/feed/spekt/package/nuget/Spekt.VSTest.Coverage.Report) |

## Vision
`Stop .NET(any) developer from shipping a bug`. If we have to say nicely `Let's not ship another bug` because [software failures cost the worldwide economy $1.1 trillion in 2016](https://crossbrowsertesting.com/blog/development/software-bug-cost/) and everybody hates bug.

## Development
Steps to build/test this repo. Use these commands in root directory of repo.

### Build
```sh
> dotnet pack
```

### Test
```sh
> dotnet test src/CodeCoverageToLcovConverterTests/CodeCoverageReaderTests.csproj
> dotnet test src/Coverage.Report.Tests/Coverage.Report.Tests.csproj

# Windows only test
> dotnet test src/Coverage.TestLogger.Tests/Coverage.TestLogger.Tests.csproj
```

### Integration
Use these steps to validate the task wire up.

```sh
> rm -rf packages/spekt.vstest.coverage.report
> dotnet pack
> cd src/Coverage.Report.Tests/TestAssets/sampletest
> rm -rf obj

# Run tests, note expected output below
> dotnet test

Build started, please wait...
Build completed.

Test run for /home/arun/src/gh/spekt/vstest.coverage/src/Coverage.Report.Tests/TestAssets/sampletest/bin/Debug/netcoreapp2.1/sampletest.dll(.NETCoreApp,Version=v2.1)
Microsoft (R) Test Execution Command Line Tool Version 15.3.0-preview-20170628-02
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

Total tests: 1. Passed: 1. Failed: 0. Skipped: 0.
Test Run Successful.
Test execution time: 0.9290 Seconds
=================================================
|   Module                     |  Coverage      |
=================================================
| Spekt.Vstest.Coverage.Report |  99.99%        |
=================================================
```

## Roadmap
Provide awesome experience to view code coverage data in VS Code which is collected by "dotnet test"

Idea inspired by  [Scott Hanselman blog](https://www.hanselman.com/blog/AutomaticUnitTestingInNETCorePlusCodeCoverageInVisualStudioCode.aspx)  and .NET Core developers( special thanks to @sesispla for bringing blog to our attention)  ask from [vstest issue](https://github.com/Microsoft/vstest/issues/981)

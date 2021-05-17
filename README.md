# Mutation Tests Example

This is a sample project that demonstrates how [Stryker .Net](https://github.com/stryker-mutator/stryker-net) can be used to run mutation tests in .Net Core.

To get more information about Stryker, you can view on [official website](https://stryker-mutator.io/).

## Project Dependencies 

- [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)

## Generate coverage report
1. Colect coverage
```
dotnet test --verbosity minimal --collect:"XPlat Code Coverage"
```

2. Open folder with Guid inside TestResults
```
reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html
```

3. Open coveragereport folder
- open index.html file

### Code coverage report example
![code coversage report](images/code-coverage.png)

## Local Run

Go to the tests folder location: 

```
cd <repository location>\MutationTestsExample\src\MutationTestsExample.Tests
```

Run the mutation tests:

```
dotnet stryker
```

Run the mutation tests:

```
dotnet stryker --diff
```

### Stryker .NET running
![code coversage report](images/dotnet-stryker.png)


Go to the stryker report location: 

```
cd <repository location>\MutationTestsExample\src\MutationTestsExample.Tests\StrykerOutput\<date run report>\reports\mutation-report.html
```
### Stryker mutation score report
![code coversage report](images/stryker-report.png)
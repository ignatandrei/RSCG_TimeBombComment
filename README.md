# RSCG_TimeBombComment

Time Bomb comment for technical debt

Just add :

//TB: 2021-09-13 this is a comment transformed into an error

and you will see the error!

The general form is

//TB: yyyy-MM-dd whatever here

[![.NET](https://github.com/ignatandrei/RSCG_TimeBombComment/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ignatandrei/RSCG_TimeBombComment/actions/workflows/dotnet.yml)


[![Nuget](https://img.shields.io/nuget/v/RSCG_TimeBombComment)](https://www.nuget.org/packages/RSCG_TimeBombComment/)

## Examples

    
```cs
//TB: 2020-09-13 this is a comment transformed into an error
```

will produce an error

[![Error](docs/error.png)](docs/error.png)

## Usage for technical debt 

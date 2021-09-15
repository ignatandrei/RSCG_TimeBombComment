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

When you have a 

//TODO

comment in your code, you can transform it into an error time bomb by adding the following line in your project file

//TB: yyyy-MM-dd whatever here
and on the date will produce an error when compiling the project

## Usage for obsolete methods
Imagine you have a method that is obsolete and you want to remember that you have to remove it.
Just put the following line in your project file


```cs
[Obsolete("should be deleted on the date on the right", TB_20210915)]
static string Test1()
{
    return "asdasd";
}
```

THen RSCG will create a static const boolean TB_20210915 that will be true if the date is less than 2021-09-15






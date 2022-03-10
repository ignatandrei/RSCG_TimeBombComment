# RSCG_TimeBombComment
Time Bomb comment for technical debt

Just add :

//TB: 2021-09-13 this is a comment transformed into an error

and you will see the error!

The general form is

```csharp
//TB: yyyy-MM-dd whatever here


Also,you can have this on methods
[Obsolete("should be deleted", TB_20210915)]
static string Test1()
{
    return "asdasd";
}

```

Also, when you want to test something in your code, but give error if compiled with release

```csharp
//Just for debug: if(args.length>0) throw new ArgumentException();
//JFD: test
```

will raise error if compiled with 

dotnet build -c release
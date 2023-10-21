using System;
namespace Console_TimeBombComment;
partial class Program
{    
    static void Main(string[] args)
    {
        //Just for debug: if(args.length>0) throw new ArgumentException();
        //JFD: test
        //TB: 2025-09-23 this is a comment transformed into an error if you put 2021
        //TB: and this is a warning
        //TB: 9999-12-30 and this will not appear
        //TODO this is just appearing in task list and as a warning
        //TODO 2025-09-23 and this is going to warning
        Console.WriteLine("See the TB comment above ? ");

        //and here Test1 with Obsolete
        Console.WriteLine(Test1());
    }
    [Obsolete("should be deleted", TB_20290915)]
    static string Test1()
    {
        return "asdasd";
    }
}

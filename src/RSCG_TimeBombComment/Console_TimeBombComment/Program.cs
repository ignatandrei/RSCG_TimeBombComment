﻿using System;

namespace Console_TimeBombComment
{
    class Program
    {
        //Obsolete("should be deleted",TB_20210913)]
        static void Main(string[] args)
        {
            //TB: 2021-09-13 this is a comment transformed into an error
            //TB: and this is a warning
            //TB: 9999-12-30 and this will not appear
            Console.WriteLine("See the TB comment above ? ");
        }
    }
}

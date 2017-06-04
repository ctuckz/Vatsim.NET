using System;
using System.Reflection;
using NUnitLite;
using Vatsim.NET.Test;

namespace Vatsim.NET.NUnitLiteRunner
{
    class Program
    {
        static int Main(string[] args)
        {
            return new AutoRun(typeof(VatsimStatusTest).GetTypeInfo().Assembly)
                .Execute(args);
        }
    }
}
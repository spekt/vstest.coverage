
namespace Spekt.Vstest.Coverage.Report
{
    using System;
    using global::Coverage.Report.Interfaces;

    public class Trace :ITrace
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}

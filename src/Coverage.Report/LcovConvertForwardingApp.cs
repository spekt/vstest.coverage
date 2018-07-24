namespace Spekt.Vstest.Coverage.Report
{
    using System.Collections.Generic;
    using global::Coverage.Report.Interfaces;

    public class LcovConvertForwardingApp: ILcovConvertForwardingApp
    {
        private List<string> allArgs ;
        private ITrace trace;

        public LcovConvertForwardingApp() : this(new Trace())
        {
        }

        //TODO: make this .ctor internal
        public LcovConvertForwardingApp(ITrace trace)
        {
            this.trace = trace;
        }

        public void Initialize(ICollection<string> argsToForward)
        {
            this.allArgs = new List<string>(argsToForward);
        }

        public int Execute()
        {
            this.trace.Log($"Received arguments:... {string.Join(",", this.allArgs)}");
            this.trace.Log($"Convert to .coverage file to lcov");

            return 0;
        }

        public void Cancel()
        {
            this.trace.Log("Cancelling...");
        }
    }
}

namespace Spekt.Vstest.Coverage.Report
{
    using System.Collections.Generic;
    using global::Coverage.Report.Interfaces;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class ConvertToLcovTask : Task, ICancelableTask
    {
        private ILcovConvertForwardingApp lcovConvertForwardingApp;
        private ITrace trace;

        [Required]
        public string PathToCoverageFile
        {
            get;
            set;
        }

        public ConvertToLcovTask() : this(new Trace(), new LcovConvertForwardingApp())
        {
        }

        //TODO: make this .ctor internal
        public ConvertToLcovTask(ITrace trace, ILcovConvertForwardingApp lcovConvertForwardingApp)
        {
            this.trace = trace;
            this.lcovConvertForwardingApp = lcovConvertForwardingApp;
        }

        public override bool Execute()
        {
            this.trace.Log("Start executing ConvertToLcovTask...");
            this.lcovConvertForwardingApp.Initialize(CreateArgument());
            return lcovConvertForwardingApp.Execute() == 0;
        }

        public void Cancel()
        {
            this.trace.Log("Cancelling ConvertToLcovTask...");
            lcovConvertForwardingApp.Cancel();
        }

        internal ICollection<string> CreateArgument()
        {
            var allArgs = this.AddArgs();
            return allArgs;
        }

        private List<string> AddArgs()
        {
            var allArgs = new List<string>();

            if (string.IsNullOrEmpty(this.PathToCoverageFile))
            {
                this.trace.Log("PathToCoverageFile cannot be empty or null.");
            }
            else
            {
                allArgs.Add(this.PathToCoverageFile);
            }

            return allArgs;
        }
    }
}

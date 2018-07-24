namespace Coverage.Report.Interfaces
{
    using System.Collections.Generic;

    public interface ILcovConvertForwardingApp
    {
        void Initialize(ICollection<string> argsToForward);

        int Execute();

        void Cancel();
    }
}

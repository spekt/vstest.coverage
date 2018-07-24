using System.Collections.Generic;

namespace Spekt.Vstest.Coverage
{
    public class CodeCoverageDataModel
    {
        public CodeCoverageDataModel()
        {
            Sources = new List<string>();
            Ranges = new List<Range>();
        }

        // List of source files. Index will be the Id.
        public List<string> Sources { get; set; }

        // List of Ranges
        public List<Range> Ranges { get; set; }
    }
}

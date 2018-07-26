using System.Collections.Generic;
using System.Text;

namespace Spekt.Vstest.Coverage.LcovGenerator
{
    public class LcovModel
    {
        public Dictionary<string, Dictionary<int, int>> Data;

        public LcovModel()
        {

        }

        public LcovModel(Dictionary<string, Dictionary<int, int>> lcovModelData)
        {
            this.Data = lcovModelData;
        }

        public override string ToString()
        {
            StringBuilder lcovContents = new StringBuilder();

            foreach (var sourceFile in this.Data)
            {
                lcovContents.AppendLine($"SF:{sourceFile.Key}");

                foreach(var range in sourceFile.Value)
                {
                    lcovContents.AppendLine($"DA:{range.Key},{range.Value}");
                }

                lcovContents.AppendLine("end_of_record");
            }

            return lcovContents.ToString();
        }
    }

    public class Generator
    {
        public LcovModel GenerateLcovCoverageData(CodeCoverageDataModel codeCoverageDataModel)
        {
            Dictionary<string, Dictionary<int, int>> lcovModelData = new Dictionary<string, Dictionary<int, int>>();

            foreach (var range in codeCoverageDataModel.Ranges)
            {
                var source = codeCoverageDataModel.Sources[range.SourceFileId];


                if (!lcovModelData.ContainsKey(source))
                {
                    lcovModelData.Add(source, new Dictionary<int, int>());
                }

                for (int i = range.StartLine; i <= range.EndLine; i++)
                {
                    if (range.Covered)
                    {
                        lcovModelData[source][i] = lcovModelData[source].ContainsKey(i) ? lcovModelData[source][i] + 1 : 1;
                    }
                    else
                    {
                        lcovModelData[source][i] = lcovModelData[source].ContainsKey(i) ? lcovModelData[source][i] : 0;
                    }
                }
            }

            return new LcovModel(lcovModelData);
        }
    }
}
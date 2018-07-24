using System;
using System.Linq;
using System.Xml.Linq;

namespace Spekt.Vstest.Coverage
{
    public class CodeCoverageReader
    {
        public CodeCoverageDataModel ParseCoverageFile(XDocument coverageFileContents, XNamespace nameSpace)
        {
            if (coverageFileContents == null)
            {
                throw new ArgumentNullException(nameof(coverageFileContents));
            }

            if (coverageFileContents.Root == null)
            {
                throw new ArgumentException("Expected valid .coverage xml data.");
            }

            var xmlCodeCoverageData = new CodeCoverageDataModel();

            var modules = coverageFileContents.Elements(nameSpace + "results").FirstOrDefault().Elements(nameSpace + "modules").FirstOrDefault().Elements(nameSpace + "module");

            // This is used to uniquely assign sources ids and maintain mapping between the range objects and sources list
            int sourcesAddedUpUntillTheLastModule = 0;

            foreach (var module in modules)
            {
                var sourceFiles = module.Elements(nameSpace + "source_files").Elements(nameSpace + "source_file");

                foreach (var sourceFile in sourceFiles)
                {
                    xmlCodeCoverageData.Sources.Add(sourceFile.Attribute(nameSpace + "path").Value);
                }

                var functions = module.Elements(nameSpace + "functions").FirstOrDefault().Elements(nameSpace + "function");

                foreach (var function in functions)
                {
                    var ranges = function.Elements(nameSpace + "ranges").FirstOrDefault().Elements(nameSpace + "range");

                    foreach (var range in ranges)
                    {
                        var rangeData = new Range();
                        rangeData.SourceFileId = sourcesAddedUpUntillTheLastModule + Int32.Parse(range.Attribute(nameSpace + "source_id").Value);
                        rangeData.StartLine = Int32.Parse(range.Attribute(nameSpace + "start_line").Value);
                        rangeData.EndLine = Int32.Parse(range.Attribute(nameSpace + "end_line").Value);
                        rangeData.Covered = range.Attribute(nameSpace + "covered").Value == "yes" || range.Attribute(nameSpace + "covered").Value == "partial";
                        xmlCodeCoverageData.Ranges.Add(rangeData);
                    }
                }

                sourcesAddedUpUntillTheLastModule = xmlCodeCoverageData.Sources.Count();
            }

            return xmlCodeCoverageData;
        }
    }
}
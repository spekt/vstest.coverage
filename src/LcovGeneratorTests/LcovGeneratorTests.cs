using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spekt.Vstest.Coverage;
using Spekt.Vstest.Coverage.LcovGenerator;
using System;
using System.Collections.Generic;
using System.IO;

namespace LcovGeneratorTests
{
    [TestClass]
    public class LcovGeneratorTests
    {

        public LcovGeneratorTests()
        {
            LcovModel lcovModel = new LcovModel();
        }

        [TestMethod]
        public void GeneratorShouldReturnCorrectDictionary()
        {
            CodeCoverageDataModel codeCoverageDataModel = new CodeCoverageDataModel();
            Generator generator = new Generator();
            codeCoverageDataModel.Sources.Add("s1.cs");
            codeCoverageDataModel.Sources.Add("s2.cs");
            Range range1 = new Range(5, 6, true, 0);
            Range range2 = new Range(9, 12, true, 1);
            Range range3 = new Range(13, 13, false, 0);

            codeCoverageDataModel.Ranges.Add(range1);
            codeCoverageDataModel.Ranges.Add(range1);
            codeCoverageDataModel.Ranges.Add(range2);
            codeCoverageDataModel.Ranges.Add(range3);

            Dictionary<string, Dictionary<int, int>> expecedLcovModelData = new Dictionary<string, Dictionary<int, int>>();
            expecedLcovModelData.Add("s1.cs", new Dictionary<int, int> { { 5, 2 }, { 6, 2 }, { 13, 0 } });
            expecedLcovModelData.Add("s2.cs", new Dictionary<int, int> { { 9, 1 }, { 10, 1 }, { 11, 1 }, { 12, 1 } });

            var actualLcovModelData = generator.GenerateLcovCoverageData(codeCoverageDataModel);

            foreach (var modelData in expecedLcovModelData)
            {
                CollectionAssert.AreEqual(expecedLcovModelData[modelData.Key], actualLcovModelData.Data[modelData.Key]);
            }
        }

        [TestMethod]
        public void LcovModelToStringShouldWriteToFile()
        {
            CodeCoverageDataModel codeCoverageDataModel = new CodeCoverageDataModel();
            Generator generator = new Generator();

            codeCoverageDataModel.Sources.Add("s1.cs");
            codeCoverageDataModel.Sources.Add("s2.cs");

            Range range1 = new Range(5, 6, true, 0);
            Range range2 = new Range(9, 12, true, 1);
            Range range3 = new Range(13, 13, false, 0);

            codeCoverageDataModel.Ranges.Add(range1);
            codeCoverageDataModel.Ranges.Add(range1);
            codeCoverageDataModel.Ranges.Add(range2);
            codeCoverageDataModel.Ranges.Add(range3);

            var actualLcovModelData = generator.GenerateLcovCoverageData(codeCoverageDataModel);

            var actualText = actualLcovModelData.ToString();
            Console.WriteLine(actualText);

            string expectedText = "SF:s1.cs" + Environment.NewLine +
                "DA:5,2" + Environment.NewLine +
                "DA:6,2" + Environment.NewLine +
                "DA:13,0" + Environment.NewLine +
                "end_of_record" + Environment.NewLine +
                "SF:s2.cs" + Environment.NewLine +
                "DA:9,1" + Environment.NewLine +
                "DA:10,1" + Environment.NewLine +
                "DA:11,1" + Environment.NewLine +
                "DA:12,1" + Environment.NewLine +
                "end_of_record" + Environment.NewLine;

            Assert.AreEqual(actualText, expectedText);
            System.Console.WriteLine(expectedText);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spekt.Vstest.Coverage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Spekt.Vstest.Coverages
{
    [TestClass]
    public class CodeCoverageReaderTests
    {
        private readonly string RelatePathToParseCoverageFileWithOneSourceAndOneLineCovered = Path.Combine("TestAssets", "ParseCoverageFileWithOneSourceAndOneLineCovered.xml");
        private readonly string RelatePathToParseCoverageFileWithTwoSourcesAndOneMultipleLinesCovered = Path.Combine("TestAssets", "ParseCoverageFileWithTwoSourcesAndOneMultipleLinesCovered.xml");
        private readonly string RelatePathToParseCoverageFileWithTwoSourcesAndOneMultipleLinesCoveredSomePartiallyCovered = Path.Combine("TestAssets", "ParseCoverageFileWithTwoSourcesAndOneMultipleLinesCoveredSomePartiallyCovered.xml");

        private CodeCoverageReader codeCoverageReader;

        public CodeCoverageReaderTests()
        {
            this.codeCoverageReader = new CodeCoverageReader();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParseCoverageFileNullInputTest()
        {
            this.codeCoverageReader.ParseCoverageFile(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseCoverageFileEmptyInputTest()
        {
            this.codeCoverageReader.ParseCoverageFile(new XDocument(), null);
        }

        [TestMethod]
        public void ParseCoverageFileWithOneSourceAndOneLineCovered()
        {
            var coverageFileContents = XDocument.Parse(File.ReadAllText(RelatePathToParseCoverageFileWithOneSourceAndOneLineCovered));
            var nameSpace = coverageFileContents.Root.Name.Namespace;
            var coverageData = this.codeCoverageReader.ParseCoverageFile(coverageFileContents, nameSpace);
            var expectedSourceFileNames = new List<string> { @"C:\Users\shrer\Source\Repos\CodeCoverageToLcovConverter\CodeCoverageToLcovConverter\Class1.cs" };
            var expectedRanges = new List<Range> { new Range(8, 8 , true, 0), new Range(13, 13, false, 0), new Range(14, 14, false, 0) };
            AssertUtility(coverageData, expectedSourceFileNames, expectedRanges);
        }

        [TestMethod]
        public void ParseCoverageFileWithTwoSourcesAndOneMultipleLinesCovered()
        {
            var coverageFileContents = XDocument.Parse(File.ReadAllText(RelatePathToParseCoverageFileWithTwoSourcesAndOneMultipleLinesCovered));
            var nameSpace = coverageFileContents.Root.Name.Namespace;
            var coverageData = this.codeCoverageReader.ParseCoverageFile(coverageFileContents, nameSpace);
            var expectedSourceFileNames = new List<string> { @"C:\Users\shrer\Source\Repos\CodeCoverageToLcovConverter\CodeCoverageToLcovConverter\Class1.cs",
                @"C:\Users\shrer\Source\Repos\CodeCoverageToLcovConverter\CodeCoverageToLcovConverterTests\obj\Debug\netcoreapp2.0\CodeCoverageToLcovConverterTests.Program.cs",
                @"C:\Users\shrer\Source\Repos\CodeCoverageToLcovConverter\CodeCoverageToLcovConverterTests\UnitTest1.cs" };
            var expectedRanges = new List<Range> { new Range(8, 8, true, 0), new Range(13, 13, false, 0), new Range(4, 4, false, 1), new Range(4, 4, false, 1), new Range(11, 11, true, 2), new Range(12, 12, true, 2) };
            AssertUtility(coverageData, expectedSourceFileNames, expectedRanges);
        }

        [TestMethod]
        public void ParseCoverageFileWithTwoSourcesAndOneMultipleLinesCoveredSomePartiallyCovered()
        {
            var coverageFileContents = XDocument.Parse(File.ReadAllText(RelatePathToParseCoverageFileWithTwoSourcesAndOneMultipleLinesCoveredSomePartiallyCovered));
            var nameSpace = coverageFileContents.Root.Name.Namespace;
            var coverageData = this.codeCoverageReader.ParseCoverageFile(coverageFileContents, nameSpace);
            var expectedSourceFileNames = new List<string> { @"C:\Users\shrer\Source\Repos\CodeCoverageToLcovConverter\CodeCoverageToLcovConverter\Class1.cs",
                @"C:\Users\shrer\Source\Repos\CodeCoverageToLcovConverter\CodeCoverageToLcovConverterTests\obj\Debug\netcoreapp2.0\CodeCoverageToLcovConverterTests.Program.cs",
                @"C:\Users\shrer\Source\Repos\CodeCoverageToLcovConverter\CodeCoverageToLcovConverterTests\UnitTest1.cs" };
            var expectedRanges = new List<Range> { new Range(8, 8, true, 0), new Range(13, 13, false, 0), new Range(4, 4, false, 1), new Range(4, 4, false, 1), new Range(11, 11, true, 2), new Range(12, 12, true, 2) };
            AssertUtility(coverageData, expectedSourceFileNames, expectedRanges);
        }

        private void AssertUtility(CodeCoverageDataModel coverageData, List<string> expectedSourceFileNames, List<Range> expectedRanges)
        {
            CollectionAssert.AreEqual(coverageData.Sources, expectedSourceFileNames);
            CollectionAssert.AreEqual(coverageData.Ranges, expectedRanges, Comparer<Range>.Create((Range x, Range y) => { return x.StartLine == y.StartLine && x.EndLine == y.EndLine && x.SourceFileId == y.SourceFileId && x.Covered == y.Covered ? 0 : -1; }));
        }
    }
}
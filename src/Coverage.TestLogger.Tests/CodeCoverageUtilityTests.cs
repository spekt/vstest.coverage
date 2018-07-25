// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.CoverageLogger.Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CodeCoverageUtilityTests
    {
        private string codeCoverageExePath;
        private string sampleCodeCoverageFilePath;
        private string expectedXMLFilePath;
        private string sampleXMLFilePath;
        private CodeCoverageUtility codeCoverageUtility;

        [TestInitialize]
        public void Initialize()
        {
            this.codeCoverageUtility = new CodeCoverageUtility();
            var testContainerDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            this.sampleCodeCoverageFilePath = Path.Combine(testContainerDirectory, @"TestData\Sample.coverage");
            this.expectedXMLFilePath = Path.Combine(testContainerDirectory, @"TestData\Expected.xml");
            this.sampleXMLFilePath = Path.Combine(testContainerDirectory, @"TestData\Sample.xml");
            this.codeCoverageExePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\..\..\packages\microsoft.codecoverage\15.8.0\build\netstandard1.0\CodeCoverage");
        }

        [TestMethod]
        public void AnalyzeCoverageFileShouldGenerateXMLFile()
        {
            this.codeCoverageUtility.AnalyzeCoverageFile(this.sampleCodeCoverageFilePath, this.codeCoverageExePath);

            var expectedFileText = File.ReadAllText(this.expectedXMLFilePath);
            var generatedFileText = File.ReadAllText(this.sampleXMLFilePath);

            File.Delete(this.sampleCodeCoverageFilePath);

            Assert.AreEqual(expectedFileText, generatedFileText, "Generated file doesn't have correct data.");
        }

        [TestMethod]
        public void GetCoverageXMLShouldReturnSummary()
        {
            var expectedSummary = Environment.NewLine +
                                  "Code Coverage" + Environment.NewLine +
                                  "----------------" + Environment.NewLine +
                                  "Module Name\t\t\tNot Covered(Blocks)\t\tNot Covered(% Blocks)\t\tCovered(Blocks)\t\tCovered(% Blocks)" + Environment.NewLine +
                                  "unittestproject7.dll\t\t1\t\t\t\t\t100\t\t\t\t0\t\t\t\t0.00" + Environment.NewLine;

            var summary = this.codeCoverageUtility.GetCoverageSummary(this.expectedXMLFilePath);

            Assert.AreEqual(expectedSummary, summary, "Summary is not correct");
        }
    }
}

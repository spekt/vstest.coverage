namespace Spekt.Vstest.Coverage.Report
{
    using System.Collections.Generic;
    using global::Coverage.Report.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class LcovConvertForwardingAppTests
    {
        private const string pathToCoverageFile = @"C:\path\to\coverage_file.coverage";
        private LcovConvertForwardingApp lcovConvertForwardingApp;
        private Mock<ITrace> traceMock;

        public LcovConvertForwardingAppTests()
        {
            this.traceMock = new Mock<ITrace>();
            this.lcovConvertForwardingApp = new LcovConvertForwardingApp(this.traceMock.Object);
            this.lcovConvertForwardingApp.Initialize(new List<string>(){pathToCoverageFile});
        }

        [TestMethod]
        public void ExecuteShouldPrintGivenCoveragePath()
        {
            this.lcovConvertForwardingApp.Execute();
            this.traceMock.Verify(t => t.Log($"Received arguments:... {pathToCoverageFile}"));
        }

        [TestMethod]
        public void ExecuteShouldCovertGivenCoverageToLcovFormat()
        {
            this.lcovConvertForwardingApp.Execute();
            this.traceMock.Verify(t => t.Log("Convert to .coverage file to lcov"));
        }

        [TestMethod]
        public void CancelShouldPrintCancelMessage()
        {
            this.lcovConvertForwardingApp.Cancel();
            this.traceMock.Verify(t => t.Log("Cancelling..."));
        }
    }
}

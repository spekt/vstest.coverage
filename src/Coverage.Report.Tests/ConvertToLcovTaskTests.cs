namespace Spekt.Vstest.Coverage.Report
{
    using System.Collections.Generic;
    using global::Coverage.Report.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ConvertToLcovTaskTests
    {
        private ConvertToLcovTask convertToLcovTask;
        private Mock<ITrace> traceMock;
        private Mock<ILcovConvertForwardingApp> lcovConvertForwardingApp;
        private const string pathToCoverageFile = @"C:\path\to\coverage_file.coverage";

        public ConvertToLcovTaskTests()
        {
            this.traceMock = new Mock<ITrace>();
            this.lcovConvertForwardingApp = new Mock<ILcovConvertForwardingApp>();
            this.convertToLcovTask = new ConvertToLcovTask(this.traceMock.Object, this.lcovConvertForwardingApp.Object);
            this.convertToLcovTask.PathToCoverageFile = pathToCoverageFile;
        }

        [TestMethod]
        public void ExecuteShouldPrintStartingMessage()
        {
            this.convertToLcovTask.Execute();
            this.traceMock.Verify(t => t.Log("Start executing ConvertToLcovTask..."));
        }

        [TestMethod]
        public void ExecuteShouldInitializeLcovConvertForwardingApp()
        {
            ICollection<string> actualArguments = null;
            this.lcovConvertForwardingApp.Setup(l => l.Initialize(It.IsAny<ICollection<string>>()))
                .Callback<ICollection<string>>((args) => actualArguments = args);

            this.convertToLcovTask.Execute();

            Assert.IsNotNull(actualArguments);
            Assert.IsTrue(actualArguments.Contains(pathToCoverageFile));
        }

        [TestMethod]
        public void ExecuteShouldExecuteLcovConvertForwardingApp()
        {
            this.convertToLcovTask.Execute();

            this.lcovConvertForwardingApp.Verify(l=> l.Execute(), Times.Once);
        }

        [TestMethod]
        public void ExecuteShouldReturnTrueIfLcovConvertForwardingAppExecutesReturnsZero()
        {
            this.lcovConvertForwardingApp.Setup(l => l.Execute()).Returns(0);
            bool success = this.convertToLcovTask.Execute();

            Assert.IsTrue(success);
        }

        [TestMethod]
        public void ExecuteShouldReturnFalseIfLcovConvertForwardingAppExecutesReturnsNonZero()
        {
            this.lcovConvertForwardingApp.Setup(l => l.Execute()).Returns(1);
            bool success = this.convertToLcovTask.Execute();

            Assert.IsFalse(success);
        }

        [TestMethod]
        public void CancelShouldLogCancellingMessage()
        {
            this.convertToLcovTask.Cancel();

            this.traceMock.Verify( t => t.Log("Cancelling ConvertToLcovTask..."));
        }

        [TestMethod]
        public void CancelShouldLogCancelLcovConvertForwardingApp()
        {
            this.convertToLcovTask.Cancel();

            this.lcovConvertForwardingApp.Verify(l => l.Cancel());
        }
    }
}

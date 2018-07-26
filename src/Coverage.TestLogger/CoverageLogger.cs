// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Extensions.CoverageLogger
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Xml.Linq;
    using Microsoft.TestPlatform.Extensions.CoverageLogger;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
    using Spekt.Vstest.Coverage;
    using Spekt.Vstest.Coverage.LcovGenerator;

    /// <summary>
    /// Logger for Generating CodeCoverage Analysis
    /// </summary>
    [FriendlyName(FriendlyName)]
    [ExtensionUri(CoverageUri)]
    internal class CoverageLogger : ITestLogger
    {
        #region Constants

        private const string CoverageUri = "datacollector://microsoft/CodeCoverage/2.0";

        private const string FriendlyName = "CoverageLogger";

        private const string SetupInteropx86 = @"x86\Microsoft.VisualStudio.Setup.Configuration.Native.dll";

        private const string SetupInteropx64 = @"x64\Microsoft.VisualStudio.Setup.Configuration.Native.dll";

        private static readonly Uri CodeCoverageDataCollectorUri = new Uri(CoverageUri);

        private ManualResetEvent summaryCompletedEvent = new ManualResetEvent(false);

        private CodeCoverageUtility codeCoverageUtility;

        #endregion

        #region ITestLogger

        /// <inheritdoc/>
        public void Initialize(TestLoggerEvents events, string testResultsDirPath)
        {
            Console.WriteLine("CoverageLogger.Initialize: Initializing Spekt code coverage logger...");

            this.codeCoverageUtility = new CodeCoverageUtility();

            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            // Register for the events.
            events.TestRunComplete += this.TestRunCompleteHandler;

            Console.WriteLine("CoverageLogger.Initialize: Completed Spekt code coverage logger initialization...");
        }
        #endregion


        #region Event Handlers

        /// <summary>
        /// Called when a test run is completed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// Test run complete events arguments.
        /// </param>
        internal void TestRunCompleteHandler(object sender, TestRunCompleteEventArgs e)
        {
            if (e.AttachmentSets == null)
            {
                return;
            }

            var coverageAttachments = e.AttachmentSets
                .Where(dataCollectionAttachment => CodeCoverageDataCollectorUri.Equals(dataCollectionAttachment.Uri)).ToArray();

            Console.WriteLine($"CoverageLogger.TestRunCompleteHandler: Found {coverageAttachments.Length} attachments.");

            if (coverageAttachments.Any())
            {
                var codeCoverageFiles = coverageAttachments.Select(coverageAttachment => coverageAttachment.Attachments[0].Uri.LocalPath).ToArray();
                foreach (var codeCoverageFile in codeCoverageFiles)
                {
                    var resultFile = Path.Combine(Path.GetDirectoryName(codeCoverageFile), Path.GetFileNameWithoutExtension(codeCoverageFile) + ".xml");
                    try
                    {
                        this.codeCoverageUtility.AnalyzeCoverageFile(codeCoverageFile, this.GetCodeCoverageExePath());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    var summary = this.codeCoverageUtility.GetCoverageSummary(resultFile);
                    Console.WriteLine(summary);

                    var coverageFileContents = XDocument.Parse(File.ReadAllText(resultFile));

                    try
                    {
                        var codeCoverageInternalRepresentation = new CodeCoverageReader().ParseCoverageFile(coverageFileContents, coverageFileContents.Root.Name.Namespace);
                        var lcovFilePath = Path.Combine(new DirectoryInfo(Path.GetDirectoryName(codeCoverageFile)).Parent.FullName, "lcov.info");
                        File.WriteAllText(lcovFilePath, new Generator().GenerateLcovCoverageData(codeCoverageInternalRepresentation).ToString());
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err);
                    }
                }
            }
        }

        private string GetCodeCoverageExePath()
        {
            var traceDataCollectorBasePath = Environment.GetEnvironmentVariable("Spekt_TraceDataCollectorDirectoryPath");
            // TODO: add assert
            return Path.Combine(traceDataCollectorBasePath, "CodeCoverage");
        }

        #endregion
    }
}
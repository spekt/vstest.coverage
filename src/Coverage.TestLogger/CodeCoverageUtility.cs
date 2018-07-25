// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.TestPlatform.Extensions.CoverageLogger
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Xml;

    using Coverage.TestLogger.Resources;

    using Microsoft.VisualStudio.TestPlatform.ObjectModel;

    public class CodeCoverageUtility
    {
        private const string CodeCoverageExeRelativePath = @"CodeCoverage.exe";
        private Process vanguardProcess;
        private ManualResetEvent coverageXmlGenerateEvent;

        public static XmlReaderSettings ReaderSettings => new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreWhitespace = true,
            DtdProcessing = DtdProcessing.Prohibit
        };

        public CodeCoverageUtility()
        {
            this.coverageXmlGenerateEvent = new ManualResetEvent(false);
        }

        public string GetCoverageSummary(string coverageXml)
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (XmlReader reader = XmlReader.Create(coverageXml, ReaderSettings))
            {
                var settingsDocument = new XmlDocument();
                settingsDocument.Load(reader);

                var modules = settingsDocument.GetElementsByTagName("module");

                if (modules.Count > 0)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine(string.Format(CultureInfo.CurrentCulture, CoverageResource.CodeCoverageHeader));
                    stringBuilder.AppendLine("----------------");

                    var header = string.Concat(
                        string.Format(CultureInfo.CurrentCulture, CoverageResource.ModuleName),
                        "\t\t\t",
                        string.Format(CultureInfo.CurrentCulture, CoverageResource.NotBlocksCovered),
                        "\t\t",
                        string.Format(CultureInfo.CurrentCulture, CoverageResource.PercNotBlocksCovered),
                        "\t\t",
                        string.Format(CultureInfo.CurrentCulture, CoverageResource.BlocksCovered),
                        "\t\t",
                        string.Format(CultureInfo.CurrentCulture, CoverageResource.PercBlocksCovered));

                    stringBuilder.AppendLine(header);
                }

                foreach (XmlNode moudle in modules)
                {
                    stringBuilder.AppendLine(moudle.Attributes["name"].Value + "\t\t" +
                                                       moudle.Attributes["blocks_not_covered"].Value + "\t\t\t\t\t" +
                                                       (100 - float.Parse(moudle.Attributes["block_coverage"].Value)) + "\t\t\t\t" +
                                                       moudle.Attributes["blocks_covered"].Value + "\t\t\t\t" +
                                                       moudle.Attributes["block_coverage"].Value);
                }
            }

            return stringBuilder.ToString();
        }

        public void AnalyzeCoverageFile(string codeCoverageFilePath, string codeCoverageRootPath)
        {
            try
            {
                var resultFile = Path.Combine(Path.GetDirectoryName(codeCoverageFilePath), Path.GetFileNameWithoutExtension(codeCoverageFilePath) + ".xml");
                var arguments = "analyze /output:" + '"' + resultFile + '"' + " " + '"' + codeCoverageFilePath + '"';
                string codeCoverageExe = Path.Combine(codeCoverageRootPath, CodeCoverageExeRelativePath);

                if (!File.Exists(codeCoverageExe))
                {
                    throw new FileNotFoundException(codeCoverageExe);
                }

                Process vanguardProcess = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        FileName = codeCoverageExe,
                        Arguments = arguments,
                        RedirectStandardError = true
                    },
                    EnableRaisingEvents = true
                };

                vanguardProcess.Exited += this.CodeCoverageExited;
                vanguardProcess.Start();

                vanguardProcess.WaitForExit();
            }
            finally
            {
                this.vanguardProcess?.Dispose();
            }
        }

        public void KillCodeCoverageExe()
        {
            if (!this.vanguardProcess.HasExited)
            {
                this.vanguardProcess.Kill();
            }
        }

        private void CodeCoverageExited(object sender, EventArgs e)
        {
            if (this.vanguardProcess != null)
            {
                if (this.vanguardProcess.HasExited && this.vanguardProcess.ExitCode != 0)
                {
                    //TODO : Make it testable.
                    //EqtTrace.Error(this.vanguardProcess.StandardError.ReadToEnd());
                }

                this.vanguardProcess.Exited -= this.CodeCoverageExited;
            }

            this.coverageXmlGenerateEvent.Set();
        }
    }
}

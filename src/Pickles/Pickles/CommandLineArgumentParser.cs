﻿#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NDesk.Options;

namespace PicklesDoc.Pickles
{
    public class CommandLineArgumentParser
    {
        public const string HELP_FEATURE_DIR = "directory to start scanning recursively for features";
        public const string HELP_OUTPUT_DIR = "directory where output files will be placed";
        public const string HELP_RESULT_FILE = "a file containing the results of testing the features";
        public const string HELP_SUT_NAME = "the name of the system under test";
        public const string HELP_SUT_VERSION = "the version of the system under test";
        public const string HELP_LANGUAGE_FEATURE_FILES = "the language of the feature files";
        public const string HELP_DOCUMENTATION_FORMAT = "the format of the output documentation";
        public const string HELP_TEST_RESULTS_FORMAT = "the format of the linked test results (nunit|xunit)";
        public const string HELP_TEST_RESULTS_FILE = "the path to the linked test results file";

        private readonly OptionSet options;
        private string documentationFormat;
        private string featureDirectory;
        private bool helpRequested;
        private string language;
        private string outputDirectory;
        private string systemUnderTestName;
        private string systemUnderTestVersion;
        private string testResultsFile;
        private string testResultsFormat;
        private bool versionRequested;

        public CommandLineArgumentParser()
        {
            this.options = new OptionSet
                          {
                              {"f|feature-directory=", HELP_FEATURE_DIR, v => this.featureDirectory = v},
                              {"o|output-directory=", HELP_OUTPUT_DIR, v => this.outputDirectory = v},
                              {"trfmt|test-results-format=", HELP_TEST_RESULTS_FORMAT, v => this.testResultsFormat = v},
                              {"lr|link-results-file=", HELP_TEST_RESULTS_FILE, v => this.testResultsFile = v},
                              {"sn|system-under-test-name=", HELP_RESULT_FILE, v => this.systemUnderTestName = v},
                              {"sv|system-under-test-version=", HELP_SUT_NAME, v => this.systemUnderTestVersion = v},
                              {"l|language=", HELP_LANGUAGE_FEATURE_FILES, v => this.language = v},
                              {"df|documentation-format=", HELP_DOCUMENTATION_FORMAT, v => this.documentationFormat = v},
                              {"v|version", v => this.versionRequested = v != null},
                              {"h|?|help", v => this.helpRequested = v != null}
                          };
        }

        private void DisplayVersion(TextWriter stdout)
        {
            stdout.WriteLine("Pickles version {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        private void DisplayHelp(TextWriter stdout)
        {
            this.DisplayVersion(stdout);
            this.options.WriteOptionDescriptions(stdout);
        }

        public bool Parse(string[] args, Configuration configuration, TextWriter stdout)
        {
            configuration.FeatureFolder = new DirectoryInfo(Directory.GetCurrentDirectory());
            configuration.OutputFolder = new DirectoryInfo(Environment.GetEnvironmentVariable("TEMP"));

            List<string> extra = this.options.Parse(args);

            if (this.versionRequested)
            {
                this.DisplayVersion(stdout);
                return false;
            }
            else if (this.helpRequested)
            {
                this.DisplayHelp(stdout);
                return false;
            }

            if (!string.IsNullOrEmpty(this.featureDirectory))
                configuration.FeatureFolder = new DirectoryInfo(this.featureDirectory);
            if (!string.IsNullOrEmpty(this.outputDirectory)) configuration.OutputFolder = new DirectoryInfo(this.outputDirectory);
            if (!string.IsNullOrEmpty(this.testResultsFormat))
                configuration.TestResultsFormat =
                    (TestResultsFormat) Enum.Parse(typeof (TestResultsFormat), this.testResultsFormat, true);
            if (!string.IsNullOrEmpty(this.testResultsFile)) configuration.TestResultsFile = new FileInfo(this.testResultsFile);
            if (!string.IsNullOrEmpty(this.systemUnderTestName)) configuration.SystemUnderTestName = this.systemUnderTestName;
            if (!string.IsNullOrEmpty(this.systemUnderTestVersion))
                configuration.SystemUnderTestVersion = this.systemUnderTestVersion;
            if (!string.IsNullOrEmpty(this.language)) configuration.Language = this.language;
            if (!string.IsNullOrEmpty(this.documentationFormat))
                configuration.DocumentationFormat =
                    (DocumentationFormat) Enum.Parse(typeof (DocumentationFormat), this.documentationFormat, true);

            return true;
        }
    }
}
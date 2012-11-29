﻿using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using PicklesDoc.Pickles.Extensions;
using Should;

namespace PicklesDoc.Pickles.Test
{
    [TestFixture]
    public class WhenParsingCommandLineArguments
    {
        private const string expectedHelpString = @"  -f, --feature-directory=VALUE
                             directory to start scanning recursively for 
                               features
  -o, --output-directory=VALUE
                             directory where output files will be placed
      --trfmt, --test-results-format=VALUE
                             the format of the linked test results 
                               (nunit|xunit)
      --lr, --link-results-file=VALUE
                             the path to the linked test results file
      --sn, --system-under-test-name=VALUE
                             a file containing the results of testing the 
                               features
      --sv, --system-under-test-version=VALUE
                             the name of the system under test
  -l, --language=VALUE       the language of the feature files
      --df, --documentation-format=VALUE
                             the format of the output documentation
  -v, --version              
  -h, -?, --help";

        private static readonly string expectedVersionString =
            string.Format(@"Pickles version {0}", Assembly.GetExecutingAssembly().GetName().Version);

        [Test]
        public void ThenCanParseExcelDocumentationFormatWithLongFormSuccessfully()
        {
            var args = new[] {@"-documentation-format=excel"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            shouldContinue.ShouldBeTrue();
            configuration.DocumentationFormat.ShouldEqual(DocumentationFormat.Excel);
        }

        [Test]
        public void ThenCanParseExcelDocumentationFormatWithShortFormSuccessfully()
        {
            var args = new[] {@"-df=excel"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            shouldContinue.ShouldBeTrue();
            configuration.DocumentationFormat.ShouldEqual(DocumentationFormat.Excel);
        }

        [Test]
        public void ThenCanParseHelpRequestWithLongFormSuccessfully()
        {
            var args = new[] {@"--help"};

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, writer);


            StringAssert.Contains(expectedHelpString.ComparisonNormalize(),
                                  writer.GetStringBuilder().ToString().ComparisonNormalize());
            Assert.AreEqual(false, shouldContinue);
            Assert.AreEqual(Path.GetFullPath(Directory.GetCurrentDirectory()), configuration.FeatureFolder.FullName);
            Assert.AreEqual(Path.GetFullPath(Environment.GetEnvironmentVariable("TEMP")),
                            configuration.OutputFolder.FullName);
            Assert.AreEqual(false, configuration.HasTestResults);
            Assert.AreEqual(null, configuration.TestResultsFile);
        }

        [Test]
        public void ThenCanParseHelpRequestWithQuestionMarkSuccessfully()
        {
            var args = new[] {@"-?"};

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, writer);

            StringAssert.Contains(expectedHelpString.ComparisonNormalize(),
                                  writer.GetStringBuilder().ToString().ComparisonNormalize());
            Assert.AreEqual(false, shouldContinue);
            Assert.AreEqual(Path.GetFullPath(Directory.GetCurrentDirectory()), configuration.FeatureFolder.FullName);
            Assert.AreEqual(Path.GetFullPath(Environment.GetEnvironmentVariable("TEMP")),
                            configuration.OutputFolder.FullName);
            Assert.AreEqual(false, configuration.HasTestResults);
            Assert.AreEqual(null, configuration.TestResultsFile);
        }

        [Test]
        public void ThenCanParseHelpRequestWithShortFormSuccessfully()
        {
            var args = new[] {@"-h"};

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, writer);

            StringAssert.Contains(expectedHelpString.ComparisonNormalize(),
                                  writer.GetStringBuilder().ToString().ComparisonNormalize());
            Assert.AreEqual(false, shouldContinue);
            Assert.AreEqual(Path.GetFullPath(Directory.GetCurrentDirectory()), configuration.FeatureFolder.FullName);
            Assert.AreEqual(Path.GetFullPath(Environment.GetEnvironmentVariable("TEMP")),
                            configuration.OutputFolder.FullName);
            Assert.AreEqual(false, configuration.HasTestResults);
            Assert.AreEqual(null, configuration.TestResultsFile);
        }

        [Test]
        public void ThenCanParseLongFormArgumentsSuccessfully()
        {
            var args = new[] {@"--feature-directory=c:\features", @"--output-directory=c:\features-output"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Assert.AreEqual(true, shouldContinue);
            Assert.AreEqual(@"c:\features", configuration.FeatureFolder.FullName);
            Assert.AreEqual(@"c:\features-output", configuration.OutputFolder.FullName);
            Assert.AreEqual(false, configuration.HasTestResults);
            Assert.AreEqual(null, configuration.TestResultsFile);
        }

        [Test]
        public void ThenCanParseResultsFileWithLongFormSuccessfully()
        {
            var args = new[] {@"-link-results-file=c:\results.xml"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Assert.AreEqual(true, shouldContinue);
            Assert.AreEqual(Path.GetFullPath(Directory.GetCurrentDirectory()), configuration.FeatureFolder.FullName);
            Assert.AreEqual(Path.GetFullPath(Environment.GetEnvironmentVariable("TEMP")),
                            configuration.OutputFolder.FullName);
            Assert.AreEqual(true, configuration.HasTestResults);
            Assert.AreEqual(@"c:\results.xml", configuration.TestResultsFile.FullName);
        }

        [Test]
        public void ThenCanParseResultsFileWithShortFormSuccessfully()
        {
            var args = new[] {@"-lr=c:\results.xml"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Assert.AreEqual(true, shouldContinue);
            Assert.AreEqual(Path.GetFullPath(Directory.GetCurrentDirectory()), configuration.FeatureFolder.FullName);
            Assert.AreEqual(Path.GetFullPath(Environment.GetEnvironmentVariable("TEMP")),
                            configuration.OutputFolder.FullName);
            Assert.AreEqual(true, configuration.HasTestResults);
            Assert.AreEqual(@"c:\results.xml", configuration.TestResultsFile.FullName);
        }

        [Test]
        public void ThenCanParseResultsFormatMstestWithLongFormSuccessfully()
        {
            var args = new[] {@"-test-results-format=mstest"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Assert.AreEqual(true, shouldContinue);
            Assert.AreEqual(TestResultsFormat.MsTest, configuration.TestResultsFormat);
        }

        [Test]
        public void ThenCanParseResultsFormatMstestWithShortFormSuccessfully()
        {
            var args = new[] {@"-trfmt=mstest"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Assert.AreEqual(true, shouldContinue);
            Assert.AreEqual(TestResultsFormat.MsTest, configuration.TestResultsFormat);
        }

        [Test]
        public void ThenCanParseResultsFormatNunitWithLongFormSuccessfully()
        {
            var args = new[] {@"-test-results-format=nunit"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Assert.AreEqual(true, shouldContinue);
            Assert.AreEqual(TestResultsFormat.NUnit, configuration.TestResultsFormat);
        }

        [Test]
        public void ThenCanParseResultsFormatNunitWithShortFormSuccessfully()
        {
            var args = new[] {@"-trfmt=nunit"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Assert.AreEqual(true, shouldContinue);
            Assert.AreEqual(TestResultsFormat.NUnit, configuration.TestResultsFormat);
        }

        [Test]
        public void ThenCanParseResultsFormatXunitWithLongFormSuccessfully()
        {
            var args = new[] {@"-test-results-format=xunit"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Assert.AreEqual(true, shouldContinue);
            Assert.AreEqual(TestResultsFormat.xUnit, configuration.TestResultsFormat);
        }

        [Test]
        public void ThenCanParseResultsFormatXunitWithShortFormSuccessfully()
        {
            var args = new[] {@"-trfmt=xunit"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Assert.AreEqual(true, shouldContinue);
            Assert.AreEqual(TestResultsFormat.xUnit, configuration.TestResultsFormat);
        }

        [Test]
        public void ThenCanParseShortFormArgumentsSuccessfully()
        {
            var args = new[] {@"-f=c:\features", @"-o=c:\features-output"};

            var configuration = new Configuration();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, TextWriter.Null);

            Assert.AreEqual(true, shouldContinue);
            Assert.AreEqual(@"c:\features", configuration.FeatureFolder.FullName);
            Assert.AreEqual(@"c:\features-output", configuration.OutputFolder.FullName);
            Assert.AreEqual(false, configuration.HasTestResults);
            Assert.AreEqual(null, configuration.TestResultsFile);
        }

        [Test]
        public void ThenCanParseVersionRequestLongFormSuccessfully()
        {
            var args = new[] {@"--version"};

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, writer);

            StringAssert.IsMatch(expectedVersionString.ComparisonNormalize(),
                                 writer.GetStringBuilder().ToString().ComparisonNormalize());
            Assert.AreEqual(false, shouldContinue);
            Assert.AreEqual(Path.GetFullPath(Directory.GetCurrentDirectory()), configuration.FeatureFolder.FullName);
            Assert.AreEqual(Path.GetFullPath(Environment.GetEnvironmentVariable("TEMP")),
                            configuration.OutputFolder.FullName);
            Assert.AreEqual(false, configuration.HasTestResults);
            Assert.AreEqual(null, configuration.TestResultsFile);
        }

        [Test]
        public void ThenCanParseVersionRequestShortFormSuccessfully()
        {
            var args = new[] {@"-v"};

            var configuration = new Configuration();
            var writer = new StringWriter();
            var commandLineArgumentParser = new CommandLineArgumentParser();
            bool shouldContinue = commandLineArgumentParser.Parse(args, configuration, writer);

            StringAssert.IsMatch(expectedVersionString.ComparisonNormalize(),
                                 writer.GetStringBuilder().ToString().ComparisonNormalize());
            Assert.AreEqual(false, shouldContinue);
            Assert.AreEqual(Path.GetFullPath(Directory.GetCurrentDirectory()), configuration.FeatureFolder.FullName);
            Assert.AreEqual(Path.GetFullPath(Environment.GetEnvironmentVariable("TEMP")),
                            configuration.OutputFolder.FullName);
            Assert.AreEqual(false, configuration.HasTestResults);
            Assert.AreEqual(null, configuration.TestResultsFile);
        }
    }
}
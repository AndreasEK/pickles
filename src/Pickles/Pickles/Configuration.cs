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
using System.IO;

namespace PicklesDoc.Pickles
{
    public class Configuration
    {
        public DirectoryInfo FeatureFolder { get; set; }

        public DirectoryInfo OutputFolder { get; set; }

        public DocumentationFormat DocumentationFormat { get; set; }

        public string Language { get; set; }

        public TestResultsFormat TestResultsFormat { get; set; }

        public bool HasTestResults
        {
            get { return this.TestResultsFile != null; }
        }

        public FileInfo TestResultsFile { get; set; }

        public string SystemUnderTestName { get; set; }

        public string SystemUnderTestVersion { get; set; }
    }
}
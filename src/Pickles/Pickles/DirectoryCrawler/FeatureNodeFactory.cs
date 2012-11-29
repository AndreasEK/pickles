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
using System.Xml.Linq;
using PicklesDoc.Pickles.DocumentationBuilders.HTML;
using PicklesDoc.Pickles.Extensions;
using PicklesDoc.Pickles.Parser;

namespace PicklesDoc.Pickles.DirectoryCrawler
{
    public class FeatureNodeFactory
    {
        private readonly FeatureParser featureParser;
        private readonly HtmlMarkdownFormatter htmlMarkdownFormatter;
        private readonly RelevantFileDetector relevantFileDetector;

        public FeatureNodeFactory(RelevantFileDetector relevantFileDetector, FeatureParser featureParser,
                                  HtmlMarkdownFormatter htmlMarkdownFormatter)
        {
            this.relevantFileDetector = relevantFileDetector;
            this.featureParser = featureParser;
            this.htmlMarkdownFormatter = htmlMarkdownFormatter;
        }

        public IDirectoryTreeNode Create(FileSystemInfo root, FileSystemInfo location)
        {
            string relativePathFromRoot = root == null ? @".\" : PathExtensions.MakeRelativePath(root, location);

            var directory = location as DirectoryInfo;
            if (directory != null)
            {
                return new FolderDirectoryTreeNode(directory, relativePathFromRoot);
            }

            var file = location as FileInfo;
            if (this.relevantFileDetector.IsFeatureFile(file))
            {
                Feature feature = this.featureParser.Parse(file.FullName);
                if (feature != null)
                {
                    return new FeatureDirectoryTreeNode(file, relativePathFromRoot, feature);
                }

                throw new InvalidOperationException("This feature file could not be read and will be excluded");
            }
            else if (this.relevantFileDetector.IsMarkdownFile(file))
            {
                XElement markdownContent = this.htmlMarkdownFormatter.Format(File.ReadAllText(file.FullName));
                return new MarkdownTreeNode(file, relativePathFromRoot, markdownContent);
            }

            throw new InvalidOperationException("Cannot create an IItemNode-derived object for " + file.FullName);
        }
    }
}
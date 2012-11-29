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
using System.Linq;
using System.Xml.Linq;
using PicklesDoc.Pickles.Parser;

namespace PicklesDoc.Pickles.DocumentationBuilders.HTML
{
  public class HtmlScenarioOutlineFormatter
  {
    private readonly HtmlDescriptionFormatter htmlDescriptionFormatter;

    private readonly HtmlImageResultFormatter htmlImageResultFormatter;

    private readonly HtmlStepFormatter htmlStepFormatter;

    private readonly HtmlTableFormatter htmlTableFormatter;

    private readonly XNamespace xmlns;

    public HtmlScenarioOutlineFormatter(
      HtmlStepFormatter htmlStepFormatter,
      HtmlDescriptionFormatter htmlDescriptionFormatter,
      HtmlTableFormatter htmlTableFormatter,
      HtmlImageResultFormatter htmlImageResultFormatter)
    {
      this.htmlStepFormatter = htmlStepFormatter;
      this.htmlDescriptionFormatter = htmlDescriptionFormatter;
      this.htmlTableFormatter = htmlTableFormatter;
      this.htmlImageResultFormatter = htmlImageResultFormatter;
      this.xmlns = HtmlNamespace.Xhtml;
    }

    private XElement FormatHeading(ScenarioOutline scenarioOutline)
    {
      if (string.IsNullOrEmpty(scenarioOutline.Name)) return null;

      return new XElement(xmlns + "div",
                          new XAttribute("class", "scenario-heading"),
                          new XElement(xmlns + "h2", scenarioOutline.Name),
                          htmlDescriptionFormatter.Format(scenarioOutline.Description)
        );
    }

    private XElement FormatSteps(ScenarioOutline scenarioOutline)
    {
      if (scenarioOutline.Steps == null) return null;

      return new XElement(xmlns + "div",
                          new XAttribute("class", "steps"),
                          new XElement(xmlns + "ul",
                                       scenarioOutline.Steps.Select(
                                         step => htmlStepFormatter.Format(step)))
        );
    }

    private XElement FormatExamples(ScenarioOutline scenarioOutline)
    {
      var exampleDiv = new XElement(xmlns + "div");

      foreach (var example in scenarioOutline.Examples)
      {
        exampleDiv.Add(new XElement(xmlns + "div",
                                    new XAttribute("class", "examples"),
                                    new XElement(xmlns + "h3", "Examples: " + example.Name),
                                    htmlDescriptionFormatter.Format(example.Description),
                                    (example.TableArgument == null) ? null : htmlTableFormatter.Format(example.TableArgument)
                         ));
      }

      return exampleDiv;
    }

    public XElement Format(ScenarioOutline scenarioOutline, int id)
    {
      var tags = RetrieveTags(scenarioOutline);

      if (tags.Contains("@future"))
      {
        return new XElement(xmlns + "li");
      }

      return new XElement(xmlns + "li",
                          new XAttribute("class", "scenario"),
                          htmlImageResultFormatter.Format(scenarioOutline),
                          FormatHeading(scenarioOutline),
                          FormatSteps(scenarioOutline),
                          (scenarioOutline.Examples == null || !scenarioOutline.Examples.Any())
                            ? null
                            : FormatExamples(scenarioOutline)
        );
    }

    internal static string[] RetrieveTags(ScenarioOutline scenarioOutline)
    {
      if (scenarioOutline == null) return new string[0];

      if (scenarioOutline.Feature == null) return scenarioOutline.Tags.ToArray();

      return scenarioOutline.Feature.Tags.Concat(scenarioOutline.Tags).ToArray();
    }
  }
}
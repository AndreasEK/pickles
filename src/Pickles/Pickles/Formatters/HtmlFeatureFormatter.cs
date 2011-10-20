﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NGenerics.DataStructures.Trees;
using Pickles.Parser;

namespace Pickles.Formatters
{
    public class HtmlFeatureFormatter
    {
        private readonly HtmlScenarioFormatter htmlScenarioFormatter;

        public HtmlFeatureFormatter(HtmlScenarioFormatter htmlScenarioFormatter)
        {
            this.htmlScenarioFormatter = htmlScenarioFormatter;
        }

        public XElement Format(Feature feature)
        {
            var xmlns = XNamespace.Get("http://www.w3.org/1999/xhtml");

            var div = new XElement(xmlns + "div",
                        new XAttribute("id", "feature"),
                        new XElement(xmlns + "h1", feature.Title), 
                        !string.IsNullOrWhiteSpace(feature.Description) ? new XElement(xmlns + "p", feature.Description) : null
                    );

            var scenarios = new XElement(xmlns + "ul", new XAttribute("id", "scenarios"));
            int id = 0;
            foreach (var scenario in feature.Scenarios)
            {
                scenarios.Add(this.htmlScenarioFormatter.Format(scenario, id++));
            }

            div.Add(scenarios);

            return div;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;
using Autofac;
using PicklesDoc.Pickles.DocumentationBuilders.HTML;
using PicklesDoc.Pickles.Parser;

namespace PicklesDoc.Pickles.Test.DocumentationBuilders.HTML
{
    [TestFixture]
    public class WhenFormattingStep : BaseFixture
    {
        private const string EXPECTED_GIVEN_HTML = "Given ";

        private static void AssertDeepEqualNodes(XElement expected, XElement actual)
        {
            const string format = "Expected:\r\n{0}\r\nActual:\r\n{1}\r\n";
            Assert.IsTrue(XNode.DeepEquals(expected, actual), string.Format(format, expected, actual));
        }

        [Test]
        public void Multiline_strings_are_formatted_as_list_items_with_pre_elements_formatted_as_code_internal()
        {
            var step = new Step
                           {
                               Keyword = Keyword.Given,
                               NativeKeyword = "Given ",
                               Name = "a simple step",
                               TableArgument = null,
                               DocStringArgument = "this is a\nmultiline table\nargument",
                           };

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            XNamespace xmlns = XNamespace.Get("http://www.w3.org/1999/xhtml");
            var expected = new XElement(xmlns + "li",
                                        new XAttribute("class", "step"),
                                        new XElement(xmlns + "span", new XAttribute("class", "keyword"),
                                                     EXPECTED_GIVEN_HTML),
                                        new XText("a simple step"),
                                        new XElement(xmlns + "div",
                                                     new XAttribute("class", "pre"),
                                                     new XElement(xmlns + "pre",
                                                                  new XElement(xmlns + "code",
                                                                               new XAttribute("class", "no-highlight"),
                                                                               new XText(
                                                                                   "this is a\nmultiline table\nargument")
                                                                      )
                                                         )
                                            )
                );

            expected.ShouldDeepEquals(actual);
        }


        [Test]
        public void Simple_steps_are_formatted_as_list_items()
        {
            var step = new Step
                           {
                               Keyword = Keyword.Given,
                               Name = "a simple step",
                               NativeKeyword = "Given ",
                               TableArgument = null,
                               DocStringArgument = null,
                           };

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            XNamespace xmlns = XNamespace.Get("http://www.w3.org/1999/xhtml");
            var expected = new XElement(xmlns + "li",
                                        new XAttribute("class", "step"),
                                        new XElement(xmlns + "span", new XAttribute("class", "keyword"),
                                                     EXPECTED_GIVEN_HTML),
                                        "a simple step"
                );

            expected.ShouldDeepEquals(actual);
        }

        [Test]
        public void Steps_get_selected_Language()
        {
            var step = new Step
                           {
                               Keyword = Keyword.Given,
                               Name = "ett enkelt steg",
                               NativeKeyword = "Givet ",
                               TableArgument = null,
                               DocStringArgument = null,
                           };

            var configuration = Container.Resolve<Configuration>();
            configuration.Language = "sv";

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            XNamespace xmlns = XNamespace.Get("http://www.w3.org/1999/xhtml");
            var expected = new XElement(
                xmlns + "li",
                new XAttribute("class", "step"),
                new XElement(xmlns + "span", new XAttribute("class", "keyword"), "Givet "),
                "ett enkelt steg");

            expected.ShouldDeepEquals(actual);
        }

        [Test]
        public void Tables_are_formatted_as_list_items_with_tables_internal()
        {
            var table = new Table
                            {
                                HeaderRow = new TableRow("Column 1", "Column 2"),
                                DataRows = new List<TableRow> {new TableRow("Value 1", "Value 2")}
                            };

            var step = new Step
                           {
                               Keyword = Keyword.Given,
                               NativeKeyword = "Given ",
                               Name = "a simple step",
                               TableArgument = table,
                               DocStringArgument = null,
                           };

            var formatter = Container.Resolve<HtmlStepFormatter>();
            XElement actual = formatter.Format(step);

            XNamespace xmlns = XNamespace.Get("http://www.w3.org/1999/xhtml");
            var expected = new XElement(xmlns + "li",
                                        new XAttribute("class", "step"),
                                        new XElement(xmlns + "span", new XAttribute("class", "keyword"),
                                                     EXPECTED_GIVEN_HTML),
                                        new XText("a simple step"),
                                        new XElement(xmlns + "div",
                                                     new XAttribute("class", "table_container"),
                                                     new XElement(xmlns + "table",
                                                                  new XAttribute("class", "datatable"),
                                                                  new XElement(xmlns + "thead",
                                                                               new XElement(xmlns + "tr",
                                                                                            new XElement(xmlns + "th",
                                                                                                         "Column 1"),
                                                                                            new XElement(xmlns + "th",
                                                                                                         "Column 2")
                                                                                   )
                                                                      ),
                                                                  new XElement(xmlns + "tbody",
                                                                               new XElement(xmlns + "tr",
                                                                                            new XElement(xmlns + "td",
                                                                                                         "Value 1"),
                                                                                            new XElement(xmlns + "td",
                                                                                                         "Value 2")
                                                                                   )
                                                                      )
                                                         )
                                            )
                );

            expected.ShouldDeepEquals(actual);
        }
    }
}
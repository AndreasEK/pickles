﻿using System;
using ClosedXML.Excel;
using NUnit.Framework;
using Autofac;
using PicklesDoc.Pickles.DocumentationBuilders.Excel;
using PicklesDoc.Pickles.Parser;
using Should;

namespace PicklesDoc.Pickles.Test.DocumentationBuilders.Excel
{
    [TestFixture]
    public class WhenCreatingSheetNamesFromFeatures : BaseFixture
    {
        [Test]
        public void ThenCanCreateSimpleNameSuccessfully()
        {
            var excelSheetNameGenerator = Container.Resolve<ExcelSheetNameGenerator>();
            var feature = new Feature {Name = "A short feature name"};

            string name;
            using (var wb = new XLWorkbook())
            {
                name = excelSheetNameGenerator.GenerateSheetName(wb, feature);
            }

            name.ShouldEqual("ASHORTFEATURENAME");
        }

        [Test]
        public void ThenCanShortenLongDuplicatedNameSuccessfully()
        {
            var excelSheetNameGenerator = Container.Resolve<ExcelSheetNameGenerator>();
            var feature1 = new Feature {Name = "This is a really really long feature name that needs to be shortened A"};
            var feature2 = new Feature {Name = "This is a really really long feature name that needs to be shortened B"};

            string name1;
            string name2;
            using (var wb = new XLWorkbook())
            {
                name1 = excelSheetNameGenerator.GenerateSheetName(wb, feature1);
                wb.AddWorksheet(name1);
                name2 = excelSheetNameGenerator.GenerateSheetName(wb, feature2);
            }

            name2.ShouldEqual("THISISAREALLYREALLYLONGFEATU(1)");
        }

        [Test]
        public void ThenCanShortenLongMultipleDuplicatedNameSuccessfully()
        {
            var excelSheetNameGenerator = Container.Resolve<ExcelSheetNameGenerator>();
            var feature1 = new Feature {Name = "This is a really really long feature name that needs to be shortened A"};
            var feature2 = new Feature {Name = "This is a really really long feature name that needs to be shortened B"};
            var feature3 = new Feature {Name = "This is a really really long feature name that needs to be shortened C"};

            string name1;
            string name2;
            string name3;
            using (var wb = new XLWorkbook())
            {
                name1 = excelSheetNameGenerator.GenerateSheetName(wb, feature1);
                wb.AddWorksheet(name1);
                name2 = excelSheetNameGenerator.GenerateSheetName(wb, feature2);
                wb.AddWorksheet(name2);
                name3 = excelSheetNameGenerator.GenerateSheetName(wb, feature3);
            }

            name3.ShouldEqual("THISISAREALLYREALLYLONGFEATU(2)");
        }

        [Test]
        public void ThenCanShortenLongNameSuccessfully()
        {
            var excelSheetNameGenerator = Container.Resolve<ExcelSheetNameGenerator>();
            var feature = new Feature {Name = "This is a really really long feature name that needs to be shortened"};

            string name;
            using (var wb = new XLWorkbook())
            {
                name = excelSheetNameGenerator.GenerateSheetName(wb, feature);
            }

            name.ShouldEqual("THISISAREALLYREALLYLONGFEATUREN");
        }

        [Test]
        public void ThenItWillRemoveUnnecessaryAndInvalidCharacters()
        {
            var excelSheetNameGenerator = Container.Resolve<ExcelSheetNameGenerator>();
            var feature = new Feature { Name = @"This Had invalid characters: :\/?*[]" };
            
            string name;
            using (var wb = new XLWorkbook())
            {
                name = excelSheetNameGenerator.GenerateSheetName(wb, feature);
            }

            name.ShouldEqual("THISHADINVALIDCHARACTERS");
        }
    }
}
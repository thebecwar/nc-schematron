using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;

namespace NCSchematronTest
{
    [TestClass]
    public class CCDTests
    {

        [TestMethod]
        public void SchemaLoadTest()
        {
            var filename = FileHelper.FindSchema("HITSP_C32.sch");

            var resolver = new Resolver();

            var schema = NCSchematron.Types.Schema.FromFile(filename, resolver);
            Assert.IsNotNull(schema);
            Assert.IsTrue(schema.Patterns.Length == 65);
        }

        
        public class DataTestRowAttribute : Attribute, ITestDataSource
        {
            public IEnumerable<object[]> GetData(MethodInfo methodInfo)
            {
                // { filename, { phase, failures, passes }
                yield return new object[] {
                    "JohnHalamkaCCDDocument_C32.xml",
                    new[] 
                    {
                        new Tuple<string, int, int>("errors", 0, 59),
                        new Tuple<string, int, int>("warning", 137, 98),
                        new Tuple<string, int, int>("note", 70, 34),
                        new Tuple<string, int, int>("violation", 0, 314),
                    }
                };
                yield return new object[]
                {
                    "CCD_HITSP_C32_Valid_WithViolations.xml",
                    new []
                    {
                        new Tuple<string, int, int>("errors", 2, 218),
                        new Tuple<string, int, int>("warning", 82, 147),
                        new Tuple<string, int, int>("note", 101, 75),
                        new Tuple<string, int, int>("violation", 19, 218),
                    }
                };
            }

            public string GetDisplayName(MethodInfo methodInfo, object[] data)
            {
                return (string)data[0];
            }
        }

        [DataTestMethod]
        //[DataRow("CCD_HITSP_C32_ALL_TemplateIdsAtRoot.xml", 4, 6)]
        //[DataRow("CCD_HITSP_C32_HistoryAndMedications_WithAllR2Elements.xml", 27, 8)]
        //[DataRow("CCD_HITSP_C32_Medications_Template_Robust.xml", 29, 6)]
        //[DataRow("CCD_HITSP_C32_Medications_Template_Small.xml", 0, 0)]
        //[DataRow("CCD_HITSP_C32_Minimal_NoSections_Valid.xml", 0, 0)]
        //[DataRow("CCD_HITSP_C32_Minimal_WithEntries_Valid.xml", 0, 0)]
        //[DataRow("CCD_HITSP_C32_Minimal_WithSections_Valid.xml", 0, 0)]
        //[DataRow("CCD_HITSP_C32_v2.1_Examples.xml", 0, 0)]
        //[DataRow("CCD_HITSP_C32_Valid_WithViolations.xml", 0, 0)]
        //[DataRow("CCD_Minimal_No_C32_templateIds.xml", 0, 0)]
        //[DataRow("JohnHalamkaCCDDocument_C32.xml", 0, 0)]
        //[DataRow("JohnHalamkaCCDDocument_CCDonly.xml", 0, 0)]
        [DataTestRow]
        public void DataTest(string file, Tuple<string, int, int>[] expectedResults)
        {
            var schemaFilename = FileHelper.FindSchema("HITSP_C32.sch");
            var resolver = new Resolver();
            var schema = NCSchematron.Types.Schema.FromFile(schemaFilename, resolver);

            var filename = FileHelper.FindCCD(file);
            var document = new XmlDocument();
            document.Load(filename);

            var resultsDict = schema.EvaluatePhase(document.CreateNavigator());
            foreach (var res in expectedResults)
            {
                var results = resultsDict[res.Item1];
                var passes = results.PatternResults.Sum(x =>
                    x.PatternFired ? x.RuleResults.Sum(y =>
                        y.RuleFired ? y.ExecutedAssertions.Count(z =>
                            z.IsError == false) : 0) : 0);
                var failures = results.PatternResults.Sum(x =>
                    x.PatternFired ? x.RuleResults.Sum(y =>
                        y.RuleFired ? y.ExecutedAssertions.Count(z =>
                            z.IsError == true) : 0) : 0);
                Assert.AreEqual(res.Item2, failures, 0, "Failure Count");
                Assert.AreEqual(res.Item3, passes, 0, "Pass Count");
            }
        }
    }
}

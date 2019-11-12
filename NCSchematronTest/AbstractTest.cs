using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NCSchematronTest
{
    [TestClass]
    public class AbstractTest
    {
        [TestMethod]
        public void SimpleAbstractDocument()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NCSchematron.Types.Schema), "http://purl.oclc.org/dsdl/schematron");
            NCSchematron.Types.Schema schema = NCSchematron.Types.Schema.FromFile(FileHelper.FindSchema("abstract-test.sch"));

            XmlDocument doc = new XmlDocument();
            doc.Load(FileHelper.FindDocument("abstract-test.xml"));

            var result = schema.Evaluate(doc.CreateNavigator());
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            // HTML test pattern
            var res = result[0];
            Assert.IsTrue(res.PatternFired);
            Assert.AreEqual(2, res.RuleResults.Length);
            Assert.IsTrue(res.RuleResults[0].RuleFired);
            Assert.AreEqual(2, res.RuleResults[0].ExecutedAssertions.Length);
            Assert.IsFalse(res.RuleResults[0].ExecutedAssertions[0].IsError);
            Assert.AreEqual("A table has at least one row", res.RuleResults[0].ExecutedAssertions[0].Message);
            Assert.IsTrue(res.RuleResults[0].ExecutedAssertions[1].IsError);
            Assert.AreEqual("A table has at least one row", res.RuleResults[0].ExecutedAssertions[1].Message);
            Assert.IsTrue(res.RuleResults[1].RuleFired);
            Assert.AreEqual(2, res.RuleResults[1].ExecutedAssertions.Length);
            Assert.IsFalse(res.RuleResults[1].ExecutedAssertions[0].IsError);
            Assert.AreEqual("A table row has at least one cell", res.RuleResults[1].ExecutedAssertions[0].Message);
            Assert.IsTrue(res.RuleResults[1].ExecutedAssertions[1].IsError);
            Assert.AreEqual("A table row has at least one cell", res.RuleResults[1].ExecutedAssertions[1].Message);

            // cals test pattern
            res = result[1];
            Assert.IsTrue(res.PatternFired);
            Assert.AreEqual(2, res.RuleResults.Length);
            Assert.IsTrue(res.RuleResults[0].RuleFired);
            Assert.AreEqual(2, res.RuleResults[0].ExecutedAssertions.Length);
            Assert.IsFalse(res.RuleResults[0].ExecutedAssertions[0].IsError);
            Assert.AreEqual("A table has at least one row", res.RuleResults[0].ExecutedAssertions[0].Message);
            Assert.IsTrue(res.RuleResults[0].ExecutedAssertions[1].IsError);
            Assert.AreEqual("A table has at least one row", res.RuleResults[0].ExecutedAssertions[1].Message);
            Assert.IsTrue(res.RuleResults[1].RuleFired);
            Assert.AreEqual(2, res.RuleResults[1].ExecutedAssertions.Length);
            Assert.IsFalse(res.RuleResults[1].ExecutedAssertions[0].IsError);
            Assert.AreEqual("A table row has at least one cell", res.RuleResults[1].ExecutedAssertions[0].Message);
            Assert.IsTrue(res.RuleResults[1].ExecutedAssertions[1].IsError);
            Assert.AreEqual("A table row has at least one cell", res.RuleResults[1].ExecutedAssertions[1].Message);
        }
    }
}

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
    [TestCategory("Variable Substitution")]
    public class VariableTests
    {
        [TestMethod]
        public void SimpleVariableTest()
        {
            NCSchematron.Types.Schema schema = NCSchematron.Types.Schema.FromFile(FileHelper.FindSchema("VariableTests.sch"));

            XmlDocument doc = new XmlDocument();
            doc.Load(FileHelper.FindDocument("VariableTests.xml"));

            var result = schema.Evaluate(doc.CreateNavigator());

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result[0].PatternFired);
            Assert.AreEqual(1, result[0].RuleResults.Length);
            Assert.IsTrue(result[0].RuleResults[0].RuleFired);
            Assert.AreEqual(1, result[0].RuleResults[0].ExecutedAssertions.Length);
            Assert.IsTrue(result[0].RuleResults[0].ExecutedAssertions[0].IsError);
            var expected = "Var1: '1234'\nVar2: 1234\nVar3: 'ABCD'\nVar4: ABCD\nVar5: 'XYZ'\nVar6: XYZ";
            Assert.AreEqual(expected, result[0].RuleResults[0].ExecutedAssertions[0].Message.Trim());
        }
    }
}

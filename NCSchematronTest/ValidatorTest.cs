using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using NCSchematron;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.XPath;

namespace NCSchematronTest
{
    [TestClass]
    public class ValidatorTest
    {
        [TestMethod]
        public void StringTest()
        {
            string schemaFilename = FileHelper.FindSchema("abstract-test.sch");
            string xmlFilename = FileHelper.FindDocument("abstract-test.xml");

            Validator validator = new Validator(schemaFilename);

            var results = validator.ValidateFile(xmlFilename);

            var output = results.GetResultString(2);
            System.Diagnostics.Debug.Print(output);
        }

        [TestMethod]
        public void ComplexStringTest()
        {
            string schemaFilename = FileHelper.FindSchema("HITSP_C32.sch");
            string xmlFilename = FileHelper.FindCCD("CCD_HITSP_C32_HistoryAndMedications_WithAllR2Elements.xml");

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.DtdProcessing = DtdProcessing.Parse;
            readerSettings.XmlResolver = FileHelper.Resolver;
            XmlReader reader = XmlReader.Create(schemaFilename, readerSettings);
            Validator validator = new Validator(reader);
            var results = validator.ValidateFile(xmlFilename);
            var output = results.GetResultString(2);
            System.Diagnostics.Debug.Print(output);
        }
    }
}

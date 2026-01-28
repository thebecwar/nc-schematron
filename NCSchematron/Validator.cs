using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace NCSchematron
{
    public class Validator
    {
        public Types.Schema Schema { get; private set; }
        public Validator(string schemaFilename)
        {
            using (var s = File.OpenRead(schemaFilename))
            {
                var reader = XmlReader.Create(s);
                LoadSchema(reader);
            }
        }
        public Validator(XmlReader reader)
        {
            LoadSchema(reader);
        }
        private void LoadSchema(XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Types.Schema));
            this.Schema = (Types.Schema)serializer.Deserialize(reader);
        }

        public ValidationResult ValidateFile(string filename)
        {
            using (Stream stream = File.OpenRead(filename))
            {
                return ValidateStream(stream);
            }
        }
        public ValidationResult ValidateXml(string xml)
        {
            using (Stream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)))
            {
                return ValidateStream(stream);
            }
        }
        public ValidationResult ValidateStream(Stream stream)
        {
            XPathDocument doc = new XPathDocument(stream);
            return ValidateStream(doc);
        }
        public ValidationResult ValidateStream(XPathDocument doc)
        {
            if (this.Schema.Phases != null)
            {
                var result = this.Schema.EvaluatePhase(doc.CreateNavigator());
                return new ValidationResult(result);
            }
            else
            {
                var result = this.Schema.Evaluate(doc.CreateNavigator());
                return new ValidationResult(result);
            }
        }
    }
}

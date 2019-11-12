using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NCSchematron.Types
{
    [Serializable]
    public class Active
    {
        [XmlAttribute(AttributeName = "pattern")]
        public string Pattern { get; set; }
    }
}

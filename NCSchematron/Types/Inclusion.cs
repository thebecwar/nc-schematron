using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NCSchematron.Types
{
    /*
     * inclusion = element include {
     *   attribute href { uriValue },
     *   foreign-empty
     * }
     */
    [Serializable]
    public class Inclusion
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }
}

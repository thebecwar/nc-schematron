using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NCSchematron.Types
{
    /*
     * ns = element ns {
     *   attribute uri { uriValue },
     *   attribute prefix { nameValue },
     *   foreign-empty
     * }
     */
    [Serializable]
    public class Ns
    {
        [XmlAttribute(AttributeName = "uri")]
        public string Uri { get; set; }

        [XmlAttribute(AttributeName = "prefix")]
        public string Prefix { get; set; }
    }
}

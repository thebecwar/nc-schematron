using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NCSchematron.Types
{
    /* value-of = element value-of {
     *   attribute select { pathValue },
     *   foreign-empty
     * }
     */
    [Serializable]
    public class ValueOf
    {
        [XmlAttribute(AttributeName = "select")]
        public string Select { get; set; }
    }
}

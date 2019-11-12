using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace NCSchematron.Types
{
    [Serializable]
    public class Report : Assert
    {
        public override EvaluationResult Evaluate(Schema schema, XPathNavigator navigator, XsltContext context)
        {
            var res = base.Evaluate(schema, navigator, context);
            res.IsError = !res.IsError;
            return res;
        }
    }
}

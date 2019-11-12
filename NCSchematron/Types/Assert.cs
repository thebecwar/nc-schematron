using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace NCSchematron.Types
{
    /*
     * assert = element assert {
     *   attribute test { exprValue },
     *   attribute flag { flagValue }?,
     *   attribute id { xsd:ID }?,
     *   attribute diagnostics { xsd:IDREFS }?,
     *   attribute properties { xsd:IDREFS }?,
     *   rich,
     *   linkable,
     *   (foreign & (text | name | value-of | emph | dir | span)*)
     *   }
     */
    [Serializable]
    public class Assert : IXmlSerializable
    {
        public string Id { get; set; }
        public string Test { get; set; }
        public string Flag { get; set; }

        public string Diagnostics { get; set; }
        public string Properties { get; set; }

        #region Linkable

        public string Role { get; set; }

        public string Subject { get; set; }

        #endregion

        #region Rich Attributes

        public string Icon { get; set; }

        public string See { get; set; }

        public string Fpi { get; set; }

        #endregion

        public string DescriptionFragment { get; set; }


        public string EvaluateDescriptionFragment(XsltContext context)
        {
            var fragment = new XmlDocument();
            var nav = fragment.CreateNavigator();
            fragment.LoadXml($"<root xmlns:sch=\"http://purl.oclc.org/dsdl/schematron\">{this.DescriptionFragment}</root>");
            var nsMgr = new XmlNamespaceManager(fragment.NameTable);
            nsMgr.AddNamespace("sch", "http://purl.oclc.org/dsdl/schematron");
            var nodes = fragment.SelectNodes("//sch:value-of", nsMgr);
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["select"].Value != null)
                {
                    var evalExpr = XPathExpression.Compile(node.Attributes["select"].Value);
                    evalExpr.SetContext(context);
                    var text = nav.Evaluate(evalExpr);
                    var replNode = fragment.CreateTextNode((string)text);
                    node.ParentNode.ReplaceChild(replNode, node);

                }
            }
            var result = fragment.DocumentElement.InnerXml.Trim().Split('\n');
            return String.Join("\n", result.Select(x => x.Trim()));
        }

        public virtual EvaluationResult Evaluate(Schema schema, XPathNavigator navigator, XsltContext context)
        {
            if (compiledExpression == null)
            {
                compiledExpression = XPathExpression.Compile(this.Test);
            }
            compiledExpression.SetContext(context);

            var executionExpression = compiledExpression;

            if (compiledExpression.ReturnType == XPathResultType.Navigator)
            {
                var value = (string)navigator.Evaluate(compiledExpression);
                executionExpression = XPathExpression.Compile(value);
                executionExpression.SetContext(context);
            }

            var result = navigator.Evaluate(executionExpression);

            bool ok = false;
            switch (executionExpression.ReturnType)
            {
                case XPathResultType.Boolean:
                    ok = (bool)result;
                    break;
                case XPathResultType.Error:
                    ok = false;
                    break;
                case XPathResultType.NodeSet:
                    ok = ((XPathNodeIterator)result).Count != 0;
                    break;
                case XPathResultType.Number:
                    ok = (int)result != 0;
                    break;
                case XPathResultType.String: // Result Type "Navigator" is the same as string?
                    ok = !String.IsNullOrEmpty((string)result);
                    break;
                default:
                    ok = false;
                    break;
            }

            return new EvaluationResult()
            {
                Assertion = this,
                IsError = !ok,
                Message = this.EvaluateDescriptionFragment(context)
            };
        }

        private XPathExpression compiledExpression;

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            this.Id = reader.GetAttribute("id");
            this.Test = reader.GetAttribute("test");
            this.Flag = reader.GetAttribute("flag");
            this.Role = reader.GetAttribute("role");
            this.Subject = reader.GetAttribute("subject");
            this.Icon = reader.GetAttribute("icon");
            this.See = reader.GetAttribute("see");
            this.Fpi = reader.GetAttribute("fpi");
            this.DescriptionFragment = reader.ReadInnerXml();
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}

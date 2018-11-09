using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ReconciliationFileViewer
{
    public class XmlHelper
    {
        private XmlHelper()
        {
        }

        private static bool IsAnElement(XmlNode node)
        {
            if (node == null)
                return false;
            return node.NodeType == XmlNodeType.Element;
        }
        private static bool HasElementChildren(XmlNode node)
        {
            if (node == null)
                return false;
            return IsAnElement(node.FirstChild);
        }

        public static string[] GetRowNames(string fileName)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);

            return GetRowNames(xml);
        }
        public static string[] GetRowNames(XmlReader reader)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);

            return GetRowNames(xml);
        }

        public static string[] GetRowNames(XmlDocument xml)
        {
            List<string> rowNames = new List<string>();
            XmlNode node = xml.DocumentElement;
            while (IsAnElement(node))
            {
                // find first element that has no elements as children
                if (HasElementChildren(node))
                {
                    node = node.FirstChild;
                }

                // that element's parent is a row
                else
                {
                    node = node.ParentNode;
                    if (!IsAnElement(node))
                        break;

                    if (!rowNames.Contains(node.Name))
                        rowNames.Add(node.Name);

                    while (node.NextSibling == null)
                    {
                        node = node.ParentNode;
                        if (!IsAnElement(node))
                            break;
                    }
                    if (IsAnElement(node))
                        node = node.NextSibling;
                }
            }
            return rowNames.ToArray();
        }

    }
}

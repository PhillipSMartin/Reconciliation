using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using LumenWorks.Framework.IO.Csv;

namespace ReconciliationLib
{
    public class CSVtoXMLConverter
    {
        public static int Convert(string csvPath, out string xml)
        {
            return Convert(csvPath, "root", out xml);
        }
        public static int Convert(string csvPath, string root, out string xml)
        {
            return Convert(csvPath, root, ',', out xml);
        }
        public static int Convert(string csvPath, string root, char delimiter, out string xml)
        {
            int recordCount = 0;
            xml = String.Empty;

            // Create XML document
            XmlDocument xmlDoc = new XmlDocument();

            // open the csv file
            using (StreamReader streamReader = new StreamReader(csvPath))
            {
                CsvReader input = new CsvReader(streamReader, false, delimiter);

                // Add XML declaration tag: <?xml>
                XmlDeclaration xmlNode = xmlDoc.CreateXmlDeclaration("1.0", null, null);
                xmlDoc.AppendChild(xmlNode);

                // Add root node: <root>...</root>
                XmlElement rootNode = xmlDoc.CreateElement(root);
                xmlDoc.AppendChild(rootNode);

                // Generate column names for use in the XML document
                string[] columnNames = new string[input.FieldCount];

                if (input.ReadNextRecord())
                {
                    for (int colIndex = 0; colIndex < input.FieldCount; colIndex++)
                    {
                        // If available, use the given column title
                        columnNames[colIndex] = input[colIndex];
                        // If no column title is given, generate a new one
                        if (columnNames[colIndex] == "")
                            columnNames[colIndex] = "Field" + colIndex.ToString();
                        // Make sure that the column name is a valid XML name
                        columnNames[colIndex] = XmlConvert.EncodeName(columnNames[colIndex].Replace(' ', '_').Replace("/", "").Replace("'", "").Replace("&", "n").Replace("-", "").Replace("%", "Pct"));
                    }

                    // Add node for every row in the CSV document
                    while (input.ReadNextRecord())
                    {
                        recordCount++;

                        // <Row>...</Row>
                        XmlElement rowNode = xmlDoc.CreateElement("Row");
                        rootNode.AppendChild(rowNode);

                        // Add sub node for each available cell in the row
                        for (int colIndex = 0; colIndex < input.FieldCount; colIndex++)
                        {
                            // <ColumnName>...</ColumnName>
                            XmlElement cellNode = xmlDoc.CreateElement(columnNames[colIndex]);
                            string str = input[colIndex];
                            if (!String.IsNullOrEmpty(str))
                            {
                                cellNode.AppendChild(xmlDoc.CreateTextNode(str.Replace("'", "")));
                                rowNode.AppendChild(cellNode);
                            }
                        }
                    }
                }
            }

            // Write XML content to StringWriter
            using (StringWriter stringWriter = new StringWriter())
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                //               xmlTextWriter.Formatting = Formatting.Indented;  // Output formatted XML code
                xmlDoc.WriteTo(xmlTextWriter);

                // Output XML code
                xml = stringWriter.ToString();
            }

            return recordCount;
        }
    }
}

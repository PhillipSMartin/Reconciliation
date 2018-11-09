using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace ReconciliationLib
{
    public class XLStoXMLConverter
    {
        public static int Convert(string xlsPath, out string xml)
        {
            return Convert(xlsPath, "root", out xml);
        }
        public static int Convert(string xlsPath, string root, out string xml)
        {
            int recordCount = 0;
            xml = String.Empty;

            // open the xml file
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            object obj;
            string str;
            int rCnt = 0;
            int cCnt = 0;

            xlApp = new Excel.Application();
            xlApp.DisplayAlerts = false;
            xlWorkBook = xlApp.Workbooks.Open(xlsPath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;

            // Create XML document
            XmlDocument xmlDoc = new XmlDocument();

            // Add XML declaration tag: <?xml>
            XmlDeclaration xmlNode = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDoc.AppendChild(xmlNode);

            // Add root node: <root>...</root>
            XmlElement rootNode = xmlDoc.CreateElement(root);
            xmlDoc.AppendChild(rootNode);

            // Generate column names for use in the XML document
            string[] columnNames = new string[range.Columns.Count];
            for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
            {
                    // If available, use the given column title
                columnNames[cCnt-1] = (string)(range.Cells[1, cCnt] as Excel.Range).Value2;
                // If no column title is given, generate a new one
                if (columnNames[cCnt - 1] == "")
                    columnNames[cCnt - 1] = "Field" + cCnt.ToString();
                // Make sure that the column name is a valid XML name
                columnNames[cCnt - 1] = XmlConvert.EncodeName(columnNames[cCnt - 1].Replace(' ', '_').Replace("/", "").Replace("'", ""));
            }



            // Add node for every row in the CSV document
            for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)
            {
                recordCount++;

                // <Row>...</Row>
                XmlElement rowNode = xmlDoc.CreateElement("Row");
                rootNode.AppendChild(rowNode);

                // Add sub node for each available cell in the row
                for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                {
                    // <ColumnName>...</ColumnName>
                    XmlElement cellNode = xmlDoc.CreateElement(columnNames[cCnt-1]);
                    obj = (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                    if (obj != null)
                    {
                        str = obj.ToString();
                        if (!String.IsNullOrEmpty(str))
                        {
                            cellNode.AppendChild(xmlDoc.CreateTextNode(str.Replace("'", "")));
                            rowNode.AppendChild(cellNode);
                        }
                    }
                }
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            ReleaseObject(xlWorkSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlApp);

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
        private static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonExecutor
{
    public class Executor
    {
        static void Main()
        {
            XMLParser.XMLParser.ParseDataFromXML("../../../collection.xml");
            PDFExporter.PDFExporter.ExportDataToPDF("1999", "2006");
        }
    }
}

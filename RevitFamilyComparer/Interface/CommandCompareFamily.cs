using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitFamilyComparer.Interface
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandCompareFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string rfa1 = FilesWorker.GetXmlFileByUser();
            if (string.IsNullOrEmpty(rfa1)) return Result.Cancelled;
            string rfa2 = FilesWorker.GetXmlFileByUser();
            if (string.IsNullOrEmpty(rfa2)) return Result.Cancelled;

            string xml1 = System.IO.File.ReadAllText(rfa1);
            string xml2 = System.IO.File.ReadAllText(rfa2);

            string result = XmlComparer.Comparer.CompareXmls(xml1, xml2);

            using (FormResult form = new FormResult(result))
            {
                if (form.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;
            }


            return Result.Succeeded;
        }

    }
}

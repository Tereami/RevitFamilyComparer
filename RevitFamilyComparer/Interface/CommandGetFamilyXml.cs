using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitFamilyComparer.Interface
{
    /// <summary>
    /// Create family description and save it at XML file
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandGetFamilyXml : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Revit family (*.rfa)|*.rfa|All files (*.*)|*.*";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

            string filePath = dialog.FileName;
            string folder = System.IO.Path.GetDirectoryName(filePath);
            string filenameWithoutEx = System.IO.Path.GetFileNameWithoutExtension(filePath);
            string xmlFile = System.IO.Path.Combine(folder, filenameWithoutEx + ".xml");

            Document familyDoc = commandData.Application.Application.OpenDocumentFile(filePath);
            FamilyInfo fi = new FamilyInfo(familyDoc);

            System.Xml.Serialization.XmlSerializer xmls =
                new System.Xml.Serialization.XmlSerializer(typeof(FamilyInfo));

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(xmlFile))
            {
                xmls.Serialize(writer, fi);
            }

            TaskDialog.Show("CommandGetFamilyXml", "Успешно обработано! \n" + xmlFile);

            familyDoc.Close(false);
            return Result.Succeeded;
        }
    }
}

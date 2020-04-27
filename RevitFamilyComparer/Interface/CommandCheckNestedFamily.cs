using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace RevitFamilyComparer.Interface
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandCheckNestedFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app =
                commandData.Application.Application;

            string familyPath = "";
            string libraryPath = "";
            
            using(FormCheckNestedFamily form = new FormCheckNestedFamily())
            {
                if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return Result.Cancelled;
                familyPath = form.FamilyPath;
                libraryPath = form.LibraryPath;
            }

            Document myFamDoc = app.OpenDocumentFile(familyPath);
            //int myFamDocOwnerFamilyId = myFamDoc.OwnerFamily.Id.IntegerValue; ;
            FamilyInfo fi1 = new FamilyInfo(myFamDoc);
            string xml1 = fi1.SerializeToXml();
            myFamDoc.Close(false);

            string familyTitle = System.IO.Path.GetFileNameWithoutExtension(familyPath);

            Dictionary<string,List<string>> library = FilesWorker.GetFamiliesLibrary(app, libraryPath);

            //ищу, в какие семейства загружено моё семейство
            List<string> parentFams = new List<string>();
            foreach(KeyValuePair<string,List<string>> kvp in library)
            {
                foreach(string fam in kvp.Value)
                {
                    if (fam.Equals(familyTitle))
                        parentFams.Add(kvp.Key);
                }
            }

            //открываю эти семейства и проверяю, отличается ли вложенное семейство
            Dictionary<string, string> log = new Dictionary<string, string>();
            foreach(string parentFamFile in parentFams)
            {
                Document parentfamDoc = app.OpenDocumentFile(parentFamFile);
                List<Family> nestedFams = new FilteredElementCollector(parentfamDoc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .Where(i => i.Name == familyTitle)
                    .ToList();
                if (nestedFams.Count == 0) 
                    throw new Exception("Не удалось найти " + familyTitle + " в семействе " + parentFamFile);

                Document nestedFamForChecking = parentfamDoc.EditFamily(nestedFams.First());
                

                FamilyInfo fi2 = new FamilyInfo(nestedFamForChecking);
                //int idOffset = nestedFamForChecking.OwnerFamily.Id.IntegerValue - myFamDocOwnerFamilyId;
                fi2.ApplyIdOffset(fi1);

                string xml2 = fi2.SerializeToXml();
                nestedFamForChecking.Close(false);
                parentfamDoc.Close(false);

                string compareResult = XmlComparer.Comparer.CompareXmls(xml1, xml2);
                if (string.IsNullOrEmpty(compareResult)) compareResult = "IDENTITY!";
                log.Add(parentFamFile, compareResult);
            }

            using (FormResultNestedFamily formResult = new FormResultNestedFamily(familyTitle, log))
            {
                formResult.ShowDialog();
            }



           return Result.Succeeded;
        }
    }
}

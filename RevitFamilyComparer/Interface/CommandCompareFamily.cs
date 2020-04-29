using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace RevitFamilyComparer.Interface
{
    /// <summary>
    /// Compare two families and show information about differences
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandCompareFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Application app = commandData.Application.Application;

            string famFilePath1 = FilesWorker.GetFamilyFileByUser("Главное семейство");
            if (string.IsNullOrEmpty(famFilePath1)) return Result.Cancelled;
            Document famdoc1 = app.OpenDocumentFile(famFilePath1);

            string famFilePath2 = FilesWorker.GetFamilyFileByUser("Семейство для сравнения");
            if (string.IsNullOrEmpty(famFilePath2)) return Result.Cancelled;
            Document famdoc2 = app.OpenDocumentFile(famFilePath2);

            //пытаюсь нормализовать смещение id загрузкой семейств в пустой проект
            //FamilyInfo fi1 = GetFamInfoByBlankProjectDocument(famFilePath1, app);
            //FamilyInfo fi2 = GetFamInfoByBlankProjectDocument(famFilePath2, app);

            FamilyInfo fi1 = new FamilyInfo(famdoc1);
            FamilyInfo fi2 = new FamilyInfo(famdoc2);


            // = new FamilyInfo(famdoc1);
            //= new FamilyInfo(famdoc2);

            //int idOffset = famdoc2.OwnerFamily.Id.IntegerValue - famdoc1.OwnerFamily.Id.IntegerValue;
            //int idOffset = fi2.List_RefPlanes.First().Id - fi1.List_RefPlanes.First().Id;
            fi2.ApplyIdOffset(fi1);



            //famdoc1.Close(false);
            //famdoc2.Close(false);

            string xml1 = fi1.SerializeToXml();
            string xml2 = fi2.SerializeToXml();

            string result = XmlComparer.Comparer.CompareXmls(xml1, xml2);

            FormResult form = new FormResult(result);
            form.ShowDialog();

            return Result.Succeeded;
        }

        /// <summary>
        /// Unlucky atteampt to eliminate shift of Ids throught loading family to blank project
        /// </summary>
        /// <param name="famFilePath"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        private FamilyInfo GetFamInfoByBlankProjectDocument(string famFilePath, Application app)
        {
            Document blankDoc = app.NewProjectDocument(UnitSystem.Metric);
            FamilyInfo fi = null;
            using (Transaction t = new Transaction(blankDoc))
            {
                t.Start("load first family");
                Family fam = null;
                blankDoc.LoadFamily(famFilePath, out fam);
                t.Commit();
                Document famdoc1 = blankDoc.EditFamily(fam);
                fi = new FamilyInfo(famdoc1);
                famdoc1.Close(false);
            }
            string folder = System.IO.Path.GetDirectoryName(famFilePath);
            string projectPath = System.IO.Path.Combine(folder, "proj " + fi.Name + DateTime.Now.ToString("HHmmss") + ".rvt");
            blankDoc.SaveAs(projectPath);
            blankDoc.Close(false);
            return fi;
        }
    }
}

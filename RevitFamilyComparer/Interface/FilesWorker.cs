using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using System.IO;

namespace RevitFamilyComparer.Interface
{
    public static class FilesWorker
    {
        public static string GetFamilyFileByUser(string windowTitle)
        {
            System.Windows.Forms.OpenFileDialog dialog1 = new System.Windows.Forms.OpenFileDialog();
            dialog1.Filter = "Revit family (*.rfa)|*.rfa|All files (*.*)|*.*";
            dialog1.Multiselect = false;
            dialog1.Title = windowTitle;
            if (dialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) return null;
            string filePath = dialog1.FileName;
            return filePath;
        }

        public static string GetXmlFileByUser()
        {
            System.Windows.Forms.OpenFileDialog dialog1 = new System.Windows.Forms.OpenFileDialog();
            dialog1.Filter = "XML (*.xml)|*.xml|All files (*.*)|*.*";
            dialog1.Multiselect = false;
            if (dialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) return string.Empty;
            string filePath = dialog1.FileName;
            return filePath;
        }

        /// <summary>
        /// Получает из папки список всех семейств и вложенных общих семейств
        /// </summary>
        /// <param name="app"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetFamiliesLibrary(
            Autodesk.Revit.ApplicationServices.Application app, string folder)
        {
            string[] files = Directory.GetFiles(folder, "*.rfa", System.IO.SearchOption.AllDirectories);
            Dictionary<string, List<string>> library = new Dictionary<string, List<string>>();

            foreach(string file in files)
            {
                Document famdoc = app.OpenDocumentFile(file);
                //string title = System.IO.Path.GetFileNameWithoutExtension(file);

                //if (library.ContainsKey(title)) throw new Exception("ДУБЛИРОВАНИЕ СЕМЕЙСТВА " + title);
                

                List<Family> nestedFams = new FilteredElementCollector(famdoc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .ToList();
                List<string> nestedFamsTitles = new List<string>();
                foreach(Family fam in nestedFams)
                {
                    Parameter sharedFamParam = fam.get_Parameter(BuiltInParameter.FAMILY_SHARED);
                    if (sharedFamParam == null) continue;
                    if (!sharedFamParam.HasValue) continue;
                    int isShared = sharedFamParam.AsInteger();
                    if(isShared == 1)
                    {
                        nestedFamsTitles.Add(fam.Name);
                    }
                }
                library.Add(file, nestedFamsTitles);
                famdoc.Close(false);
            }
            return library;
        }


        private static string RemoveExtensionIfExists(string title)
        {
            if(title.EndsWith(".rfa"))
            {
                return title.Substring(0, title.Length - 5);
            }
            return title;
        }
    }
}

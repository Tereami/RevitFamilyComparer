using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamilyComparer.Interface
{
    public static class FilesWorker
    {
        public static string GetFamilyFileByUser()
        {
            System.Windows.Forms.OpenFileDialog dialog1 = new System.Windows.Forms.OpenFileDialog();
            dialog1.Filter = "Revit family (*.rfa)|*.rfa|All files (*.*)|*.*";
            dialog1.Multiselect = false;
            if (dialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK) return string.Empty;
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
    }
}

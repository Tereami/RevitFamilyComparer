using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitFamilyComparer.Interface
{
    public partial class FormCheckNestedFamily : Form
    {

        public string FamilyPath;
        public string LibraryPath;

        public FormCheckNestedFamily()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            FamilyPath = textBoxFamilyPath.Text;
            LibraryPath = textBoxLibraryPath.Text;

            this.Close();
        }

        private void buttonSelectFamily_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog1 = new System.Windows.Forms.OpenFileDialog();
            dialog1.Filter = "Revit family (*.rfa)|*.rfa|All files (*.*)|*.*";
            dialog1.Multiselect = false;
            if (dialog1.ShowDialog() != DialogResult.OK) return;
            textBoxFamilyPath.Text = dialog1.FileName;
        }

        private void buttonSelectLibrary_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() != DialogResult.OK) return;
            textBoxLibraryPath.Text = dialog.SelectedPath;
        }
    }
}

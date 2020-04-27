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
    public partial class FormResultNestedFamily : Form
    {
        public FormResultNestedFamily(string title, Dictionary<string,string> data)
        {
            InitializeComponent();

            labelTitle.Text = title;

            ColumnHeader columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "Path";
            columnHeader1.Width = 150;

            ColumnHeader columnHeader2 = new ColumnHeader();
            columnHeader2.Text = "Result";
            columnHeader2.Width = 150;

            this.listView1.Columns.Add(columnHeader1);
            this.listView1.Columns.Add(columnHeader2);

            foreach (var kvp in data)
            {
                ListViewItem listItem = new ListViewItem(kvp.Key);
                listItem.SubItems.Add(kvp.Value);
                listView1.Items.Add(listItem);
            }
        }
    }
}

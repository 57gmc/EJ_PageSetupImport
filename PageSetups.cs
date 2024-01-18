using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EJ_.Forms
{
    public partial class PageSetups : Form
    {
        public PageSetups()
        {
            InitializeComponent();
        }

        public string Selected { get; set; }

        public string[] SetListItems 
        { 
            //get; 
            set {
                lbxPageSetups.Items.Clear();
                lbxPageSetups.Items.AddRange(value);
                lbxPageSetups.SelectedIndex = 0;
            }
            
        }

        public string SetPrompt
        {
            //get; 
            set { lblPrompt.Text = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {            
            this.Selected = lbxPageSetups.SelectedItem.ToString();
            // setting the DialogResult here forces the button to execute
            // when called from another sub.
            this.DialogResult = DialogResult.OK;
        }

        private void lbxPageSetups_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // allow the user to close the form on selection.
            btnOK_Click(sender, e);
        }
    }
}

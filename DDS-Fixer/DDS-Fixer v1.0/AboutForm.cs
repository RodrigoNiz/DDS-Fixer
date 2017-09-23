using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDS_Fixer_v1._0
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

 
        public void VisitGitHub()
        {
            if (DialogResult.Yes == MessageBox.Show("Visit DDS-Fixer GitHub page?", "Visit website?", MessageBoxButtons.YesNo))
            {
                try
                {
                    System.Diagnostics.Process.Start(@"https://github.com/RodrigoNiz/DDS-Fixer");
                }
                catch
                    (
                     System.ComponentModel.Win32Exception noBrowser)
                {
                    if (noBrowser.ErrorCode == -2147467259)
                        MessageBox.Show(noBrowser.Message);
                }
                catch (System.Exception other)
                {
                    MessageBox.Show(other.Message);
                }
                Close();
            }

        }

        public void VisitIconSource()
        {
            if (DialogResult.Yes == MessageBox.Show("Visit iconarchive.com?", "Visit website?", MessageBoxButtons.YesNo))
            {
                try
                {
                    System.Diagnostics.Process.Start(@"http://www.iconarchive.com/show/oxygen-icons-by-oxygen-icons.org/Apps-okteta-icon.html");
                }
                catch
                    (
                     System.ComponentModel.Win32Exception noBrowser)
                {
                    if (noBrowser.ErrorCode == -2147467259)
                        MessageBox.Show(noBrowser.Message);
                }
                catch (System.Exception other)
                {
                    MessageBox.Show(other.Message);
                }
                Close();

            }

        }
        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            VisitGitHub();
           
        }

        private void linkLabelIconSource_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            VisitIconSource();
     
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            toolTip.SetToolTip(linkLabelGitHub, "Visit github.com");
            toolTip.SetToolTip(linkLabelIconSource, "Visit iconarchive.com");
        }
    }
}

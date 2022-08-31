using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogConvertTool
{
    public partial class AddConfigDlg : Form
    {
        private string configName;
        public string ConfigName
        {
            get
            {
                return configName;
            }
            set
            {
                configName = value;
            }
        }

        public bool bConfirm { get; set; }

        public AddConfigDlg()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            configName = textBoxName.Text;
            bConfirm = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            bConfirm = false;
            Close();
        }
    }
}

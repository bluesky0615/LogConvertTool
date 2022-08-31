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
    public enum AddDlgType
    {
        AddKeyword = 0,
        AddKeyword_withInsertPos
    };

    public enum InsertPosType
    {
        SelectRow_Before = 0,
        SelectRow_After
    };

    public partial class AddKeywordDlg : Form
    {
        private string keyName;
        public string KeyName
        {
            get
            {
                return keyName;
            }
            set
            {
                keyName = value;
            }
        }

        public InsertPosType InsertPos { get; set; }

        private AddDlgType type;

        public bool bConfirm { get; set; }

        public AddKeywordDlg(AddDlgType _type)
        {
            InitializeComponent();

            type = _type;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            keyName = textBoxName.Text;
            InsertPos = (InsertPosType)comboBox1.SelectedIndex;
            bConfirm = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            bConfirm = false;
            Close();
        }

        private void AddKeywordDlg_Load(object sender, EventArgs e)
        {
            if(type == AddDlgType.AddKeyword)
            {
                label2.Visible = false;
                comboBox1.Visible = false;
            }

            comboBox1.SelectedIndex = 0;
        }
    }
}

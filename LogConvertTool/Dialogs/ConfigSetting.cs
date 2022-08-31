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

    public partial class ConfigSetting : Form
    {
        private string configName;

        private string xmlPath;

        //private List<KeywordInfo> lastKeyList;

        private BindingList<KeywordInfo> bdList;

        private SectionRule secRule;

        public ConfigSetting(string cfgName)
        {
            InitializeComponent();

            configName = cfgName;

            xmlPath = XmlHelper.XmlLocationPath + configName + ".xml";
        }

        private void ConfigSetting_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;

            label_curCfg.Text = "配置名称: " + configName;

            List<KeywordInfo> keyList;
            XmlHelper.GetAllKeywords(xmlPath, out keyList);
            
            bdList  = new BindingList<KeywordInfo>(keyList);

            XmlHelper.GetSectionRule(xmlPath, out secRule);

            InitTab1GridView();
            InitTab2Controls();
        }

        private void InitTab1GridView()
        {



            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.Name = "CsvField";
            column1.HeaderText = "提取字段(csv)";
            column1.DataPropertyName = "CsvField";
            column1.Width = 160;
            dataGridView1.Columns.Add(column1);

            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.Name = "LogField";
            column2.HeaderText = "关键字(log)";
            column2.DataPropertyName = "LogField";
            column2.Width = 160;
            dataGridView1.Columns.Add(column2);

            DataGridViewComboBoxColumn column3 = new DataGridViewComboBoxColumn();
            column3.Name = "Format";
            column3.HeaderText = "格式";
            column3.DataPropertyName = "Format";
            column3.Width = 100;
            column3.Items.Add("数字");
            column3.Items.Add("多个数字");
            column3.Items.Add("字符串");
            column3.Items.Add("日期时间");
            dataGridView1.Columns.Add(column3);

            DataGridViewComboBoxColumn column4 = new DataGridViewComboBoxColumn();
            column4.Name = "DigitNum";
            column4.HeaderText = "数字个数";
            column4.DataPropertyName = "DigitNum";
            column4.Width = 80;
            column4.Items.Add("2");
            column4.Items.Add("3");
            column4.Items.Add("4");
            column4.Items.Add("x");
            dataGridView1.Columns.Add(column4);


            dataGridView1.DataSource = bdList;

            //dataGridView1.Columns["CsvField"].HeaderText = "提取字段(csv)";
            //dataGridView1.Columns["LogField"].HeaderText = "关键字(log)";
            ////dataGridView1.Columns["Format"].HeaderText = "格式";

            //dataGridView1.Columns["CsvField"].Width = 180;
            //dataGridView1.Columns["LogField"].Width = 180;

            //dataGridView1.Columns["CsvField"].ReadOnly = true;


        }

        private void InitTab2Controls()
        {
            textBox_startFlag.Text = secRule.StartFlag;
            textBox_endFlag.Text = secRule.EndFlag;

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                string itemString = checkedListBox1.Items[i].ToString();
                if ( secRule.RowSplitters.Contains(itemString) )
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddKeywordDlg addDlg = new AddKeywordDlg(AddDlgType.AddKeyword);
            addDlg.ShowDialog();

            if(addDlg.bConfirm)
            {
                if(CheckAddItemInBdList(addDlg.KeyName))
                {
                    MessageBox.Show("不能添加已经存在项目！");
                    return;
                }

                KeywordInfo info = new KeywordInfo();
                info.CsvField = addDlg.KeyName;
                bdList.Add(info);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            
            if(dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult res = MessageBox.Show("确定要删除此行数据？", "删除", MessageBoxButtons.OKCancel);
                if(res == DialogResult.OK)
                {
                    string strDelete = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                    var whereRemove = bdList.FirstOrDefault(t => t.CsvField == strDelete);
                    bool ret = bdList.Remove(whereRemove);
                    if(!ret)
                    {
                        MessageBox.Show("删除失败!");
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择一行数据!");
            }
            
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsSaveIgnoreDigitNum;
                XmlHelper.UpdateAllKeywords(xmlPath, bdList.ToList(), out IsSaveIgnoreDigitNum);

                GetTab2Content();
                XmlHelper.UpdateSecRules(xmlPath, secRule);

                if(IsSaveIgnoreDigitNum)
                {
                    MessageBox.Show("保存成功.\n('数字个数'只有格式为多个数字时将会保存，其他格式不保存)");
                }
                else
                {
                    MessageBox.Show("保存成功.");
                }
                

                this.Close();
            }
            catch(Exception )
            {
                MessageBox.Show("保存失败！");
            }
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                AddKeywordDlg addDlg = new AddKeywordDlg(AddDlgType.AddKeyword_withInsertPos);
                addDlg.ShowDialog();

                if (addDlg.bConfirm)
                {
                    if (CheckAddItemInBdList(addDlg.KeyName))
                    {
                        MessageBox.Show("不能添加已经存在项目！");
                        return;
                    }

                    KeywordInfo info = new KeywordInfo();
                    info.CsvField = addDlg.KeyName;

                    InsertPosType pos = addDlg.InsertPos;

                    string strInsert = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    var whereRemove = bdList.FirstOrDefault(t => t.CsvField == strInsert);
                    int index = bdList.IndexOf(whereRemove);
                    index = (pos == InsertPosType.SelectRow_Before) ? index : (index + 1);
                    bdList.Insert(index, info);
                }
            }
            else
            {
                MessageBox.Show("请先选择一行插入数据的位置!");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 0)
            {
                buttonAdd.Visible = true;
                buttonDelete.Visible = true;
                buttonInsert.Visible = true;
            }
            else
            {
                buttonAdd.Visible = false;
                buttonDelete.Visible = false;
                buttonInsert.Visible = false;
            }
        }

        private bool CheckAddItemInBdList(string addName)
        {
            foreach(KeywordInfo item in bdList)
            {
                if(item.CsvField == addName)
                {
                    return true;
                }
            }

            return false;
        }

        private void GetTab2Content()
        {
            secRule.StartFlag = textBox_startFlag.Text;
            secRule.EndFlag = textBox_endFlag.Text;

            secRule.RowSplitters.Clear();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    secRule.RowSplitters.Add(checkedListBox1.Items[i].ToString());
                }
            }
        }
    }
}

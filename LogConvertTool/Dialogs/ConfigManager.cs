using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogConvertTool
{
    public partial class ConfigManager : Form
    {
        public ConfigManager(string curSel)
        {
            InitializeComponent();

            _curSelItem = curSel;
        }

        private string _curSelItem;

        public delegate void ConfigDeleteEventHandler(string name);
        public delegate void ConfigAddEventHandler(string name);
        public event ConfigDeleteEventHandler deleteConfigHandler;
        public event ConfigAddEventHandler addConfigHandler;

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddConfigDlg addDlg = new AddConfigDlg();
            addDlg.ShowDialog();
            if ( addDlg.bConfirm )
            {
                if(IsExistsItem(addDlg.ConfigName))
                {
                    MessageBox.Show("不能加入重复配置项！");
                    return;
                }

                if (String.IsNullOrEmpty(addDlg.ConfigName))
                {
                    MessageBox.Show("不能加入名字为空的配置项！");
                    return;
                }

                string xmlPath = XmlHelper.XmlLocationPath + addDlg.ConfigName + ".xml";

                XmlHelper.createXmlFile(xmlPath);

                listView1.Items.Add(addDlg.ConfigName);

                addConfigHandler?.Invoke(addDlg.ConfigName);
            }

        }

        private void ConfigManager_Load(object sender, EventArgs e)
        {
            //listView1.Columns.Add("配置项", 120, HorizontalAlignment.Left); //一步添加

            updateListView1();

            foreach (ColumnHeader ch in listView1.Columns) { ch.Width = -2; }
        }

        private void updateListView1()
        {
            listView1.Items.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(XmlHelper.XmlLocationPath);
            FileInfo[] files = dirInfo.GetFiles("*.xml");

            foreach (FileInfo file in files)
            {
                if (CheckIsValidXml(file.FullName))
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FullName);
                    if (!string.IsNullOrEmpty(_curSelItem) && fileNameWithoutExt == _curSelItem)
                    {
                        ListViewItem item = new ListViewItem(fileNameWithoutExt);
                        item.Selected = true;
                        item.Focused = true;
                        listView1.Items.Add(item);
                    }
                    else
                    {
                        listView1.Items.Add(fileNameWithoutExt);
                    }
                    
                }
            }
        }

        private bool IsExistsItem(string text)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text == text)
                    return true;
            }
            return false;
        }

        static public bool CheckIsValidXml(string path)
        {
            string txt = File.ReadAllText(path);

            Regex reg1 = new Regex("<Root>"); // 定义一个Regex对象实例
            Match m1 = reg1.Match(txt);     // 在字符串中匹配
            Regex reg2 = new Regex("<SectionRules"); // 定义一个Regex对象实例
            Match m2 = reg2.Match(txt);     // 在字符串中匹配
            Regex reg3 = new Regex("<Keywords"); // 定义一个Regex对象实例
            Match m3 = reg3.Match(txt);     // 在字符串中匹配
            if (m1.Success && m2.Success && m3.Success)
            {
                return true;
            }

            return false;
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            string selectItemName = "";
            if (listView1.SelectedItems.Count > 0)
            {
                selectItemName = listView1.SelectedItems[0].Text;
            }
            else
            {
                MessageBox.Show("请先在列表中选择一项！");
                return;
            }

            ConfigSetting setDlg = new ConfigSetting(selectItemName);
            setDlg.ShowDialog();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string selectItemName = "";
            if(listView1.SelectedItems.Count > 0)
            {
                selectItemName = listView1.SelectedItems[0].Text;
            }
            else
            {
                MessageBox.Show("请现在列表中选择一项！");
                return;
            }

            DialogResult res = MessageBox.Show("确定要删除此配置项？\n(删除后无法恢复)", "删除", MessageBoxButtons.OKCancel);
            if(res == DialogResult.OK)
            {
                try
                {
                    string xmlPath = XmlHelper.XmlLocationPath + selectItemName + ".xml";

                    if (File.Exists(xmlPath))
                    {
                        File.Delete(xmlPath);
                    }

                    //listView1.Items.RemoveByKey(selectItemName);
                    updateListView1();

                    deleteConfigHandler?.Invoke(selectItemName);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

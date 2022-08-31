using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogConvertTool
{
    public partial class Form1 : Form
    {
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Form1()
        {
            InitializeComponent();
        }



        private void button_SelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Log files (*.log)|*.log|Text files (*.txt)|*.txt";
            openDlg.RestoreDirectory = true;


            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                //获取文件的路径

                this.textBox_sourceFile.Text = openDlg.FileName;
            }
        }

        private void button_Convert_Click(object sender, EventArgs e)
        {
            //log.Info("Convert start:" + DateTime.Now.ToString("mm:ss:fff"));

            richTextBox_convertCsv.Clear();

            if(comboBox_logType.SelectedItem == null || String.IsNullOrEmpty(comboBox_logType.Text))
            {
                MessageBox.Show("没有选择配置！请先选择配置");
                return;
            }

            string filePath = textBox_sourceFile.Text;
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("没有选择文件！请先选择文件");
                return ;
            }

            if(!File.Exists(filePath))
            {
                MessageBox.Show("文件不存在");
                return;
            }

            string xmlPath = XmlHelper.XmlLocationPath + comboBox_logType.Text + ".xml";
            ConfigInfo config;
            XmlHelper.GetConfigInfoFromXml(xmlPath, out config);

            //设置鼠标繁忙
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            string outputFilePath = String.Empty;
            try
            {
                string lineStr;//记录文件的行内容

                ParseLogHelper logHelper = new ParseLogHelper(config);

                Encoding encoding = EncodingType.GetType(filePath);
                //1.读取文件每一行
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fs, encoding/*Encoding.UTF8*/);

                    while (sr.Peek() != -1)
                    {
                        lineStr = sr.ReadLine();

                        logHelper.ParsePerLine(lineStr);
                    }

                    sr.Close();
                    fs.Close();
                }

                string dirPath = Path.GetDirectoryName(filePath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                //2.将内容转换成CsvDataFormat
                List<CsvDataFormat> csvDataList = logHelper.Convert2CsvDataList();
                if(csvDataList.Count == 0)
                {
                    richTextBox_convertCsv.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + ": 转换失败！\n" + "原因：log文件格式与所选配置格式不一致";
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    return;
                }

                //3.将CsvDataFormat写入csv文件
                outputFilePath = dirPath + "\\" + fileNameWithoutExtension + ".csv";
                ConvertCsvHelper.WriteCsvData2File(outputFilePath, csvDataList);

                richTextBox_convertCsv.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + ": 转换成功！\n" + "输出路径为：" + outputFilePath;
                this.Cursor = System.Windows.Forms.Cursors.Default;

                Properties.Settings.Default.LastSelectConfig = comboBox_logType.Text;
                Properties.Settings.Default.Save();

                //log.Info("Convert end:" + DateTime.Now.ToString("mm:ss:fff"));
            }
            catch (IOException er)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                if (er.Message.Contains("另一进程使用"))
                {
                    richTextBox_convertCsv.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + ": 转换失败！\n" + "原因：该csv文件正被打开，请关闭该文件后再尝试";
                }
                else
                {
                    MessageBox.Show(string.Format("文件读取出错!\n消息={0}", er.Message));
                }
            }
        }

        private string ConvertFileName2DateFormat(string fileName)
        {
            string[] format = { "yyyyMMdd" };
            DateTime result;
            if(DateTime.TryParseExact(fileName, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,out result))
            {
                return result.ToString("yyyy/MM/dd");
            }
            else
            {
                return String.Empty;
            }
        }

        private void button_Config_Click(object sender, EventArgs e)
        {

            string curSelItem = String.Empty;
            if(comboBox_logType.Items.Count > 0)
            {
                if(comboBox_logType.SelectedItem != null)
                    curSelItem = comboBox_logType.SelectedItem.ToString();
            }
            
            ConfigManager cfgMgr = new ConfigManager(curSelItem);
            cfgMgr.addConfigHandler += addComboxItem;
            cfgMgr.deleteConfigHandler += deleteComboxItem;

            cfgMgr.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;

            DirectoryInfo dirInfo = new DirectoryInfo(XmlHelper.XmlLocationPath);
            FileInfo[] files = dirInfo.GetFiles("*.xml");

            foreach (FileInfo file in files)
            {
                if (ConfigManager.CheckIsValidXml(file.FullName))
                {
                    comboBox_logType.Items.Add(Path.GetFileNameWithoutExtension(file.FullName));
                }
            }

            if(comboBox_logType.Items.Count > 0)
            {
                string lastSelectItem = Properties.Settings.Default.LastSelectConfig;
                if (!string.IsNullOrEmpty(lastSelectItem))
                {
                    comboBox_logType.SelectedItem = lastSelectItem;
                }
                else 
                { 
                    comboBox_logType.SelectedIndex = 0; 
                }
            }
        }

        private void deleteComboxItem(string name)
        {
            comboBox_logType.Items.Remove(name);
        }

        private void addComboxItem(string name)
        {
            comboBox_logType.Items.Add(name);
        }

        private void button_SelectDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openDlg = new FolderBrowserDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                this.textBox_sourceDir.Text = openDlg.SelectedPath;
            }
        }

        private void button_MultiConvert_Click(object sender, EventArgs e)
        {
            richTextBox_multiConvert.Clear();

            if (comboBox_logType.SelectedItem == null || String.IsNullOrEmpty(comboBox_logType.Text))
            {
                MessageBox.Show("没有选择配置！请先选择配置");
                return;
            }

            string dirPath = textBox_sourceDir.Text;
            if (string.IsNullOrEmpty(dirPath))
            {
                MessageBox.Show("没有选择目录！请先选择目录");
                return;
            }

            if (!Directory.Exists(dirPath))
            {
                MessageBox.Show("目录不存在!");
                return;
            }

            string xmlPath = XmlHelper.XmlLocationPath + comboBox_logType.Text + ".xml";
            ConfigInfo config;
            XmlHelper.GetConfigInfoFromXml(xmlPath, out config);


            string outputFilePath = String.Empty;
            try
            {
                string lineStr;//记录文件的行内容

                ParseLogHelper logHelper = new ParseLogHelper(config);

                List<FileInfo> logFileList  = GetLogFilesFromDir(dirPath);

                if(logFileList.Count <= 0)
                {
                    MessageBox.Show("该目录下没有日志文件！");
                    return;
                }

                //设置鼠标繁忙
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                foreach (FileInfo file  in logFileList)
                {
                    string filePath = file.FullName;

                    Encoding encoding = EncodingType.GetType(filePath);
                    //1.读取文件每一行
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        StreamReader sr = new StreamReader(fs, encoding/*Encoding.UTF8*/);

                        while (sr.Peek() != -1)
                        {
                            lineStr = sr.ReadLine();

                            logHelper.ParsePerLine(lineStr);
                        }

                        sr.Close();
                        fs.Close();
                    }
                }

                //2.将内容转换成CsvDataFormat
                List<CsvDataFormat> csvDataList = logHelper.Convert2CsvDataList();
                if (csvDataList.Count == 0)
                {
                    richTextBox_multiConvert.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") +  ": 转换失败！\n" + "原因：log文件格式与所选配置格式不一致";
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    return;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                //3.将CsvDataFormat写入csv文件
                outputFilePath = dirPath + "\\" + dirInfo.Name + ".csv";
                ConvertCsvHelper.WriteCsvData2File(outputFilePath, csvDataList);

                richTextBox_multiConvert.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + ": 转换成功！\n" + "输出路径为：" + outputFilePath;
                this.Cursor = System.Windows.Forms.Cursors.Default;

                Properties.Settings.Default.LastSelectConfig = comboBox_logType.Text;
                Properties.Settings.Default.Save();
            }
            catch (IOException er)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                if (er.Message.Contains("另一进程使用"))
                {
                    richTextBox_multiConvert.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + ": 转换失败！\n" + "原因：该csv文件正被打开，请关闭该文件后再尝试";
                }
                else
                {
                    MessageBox.Show(string.Format("文件读取出错!\n消息={0}", er.Message));
                }
            }
        }

        private List<FileInfo> GetLogFilesFromDir(string path)
        {
            List<FileInfo> flist = new List<FileInfo>();

            DirectoryInfo fdir = new DirectoryInfo(path);
            FileInfo[] file = fdir.GetFiles();

            foreach (FileInfo f in file) //显示当前目录所有文件   
            {
                if (f.Extension == ".log" || f.Extension == ".txt")
                {
                    flist.Add(f);
                }
            }
            
            return flist;
        }
    }
}

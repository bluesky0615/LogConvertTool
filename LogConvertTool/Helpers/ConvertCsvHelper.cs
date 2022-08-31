using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogConvertTool
{
    class ConvertCsvHelper
    {

        //private List<CsvDataFormat> csvDataList;

        static public void WriteCsvData2File(string destFilePath, List<CsvDataFormat> csvDataList)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(destFilePath, false, Encoding.UTF8))
                {
                    if(csvDataList.Count > 0)
                    {
                        WriteHeader(sw, csvDataList[0]);
                    }
                    
                    foreach(CsvDataFormat data in csvDataList)
                    {
                        WriteRecord(sw,data);
                    }

                    sw.Close();
                }
            }
            catch (IOException er)
            {
                MessageBox.Show(string.Format("文件写入出错！\n消息={0}", er.Message));
                throw er;
            }
        }

        static private void WriteHeader(StreamWriter stream, CsvDataFormat csvData)
        {
            string strHeader = String.Empty;
            foreach (string key in csvData.headValueDict.Keys )
            {
                strHeader += key + ",";
            }
            strHeader = strHeader.TrimEnd(',');
            stream.WriteLine(strHeader);
        }

        static private void WriteRecord(StreamWriter stream, CsvDataFormat data)
        {
            StringBuilder strRecord = new StringBuilder("",256);

            foreach(string value in data.headValueDict.Values)
            {
                string addStr = String.Empty;
                if(value.Contains(','))
                {
                    addStr = "\"" + value + "\"";
                }
                else
                {
                    addStr = value;
                }
                strRecord.Append(addStr + ",");
            }

            stream.WriteLine(strRecord.ToString());
        }
    }
}

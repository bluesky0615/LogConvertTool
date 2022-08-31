using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogConvertTool
{
    enum LineTag
    {
        Line_None = 0,
        Line_Start = 1,
        Line_End,
        Line_Middle
    }

    class ParseLogHelper
    {
        //#region 单例模式
        //private volatile static ParseLogHelper _instance = null;
        //private static readonly object lockHelper = new object();//线程锁
        //public static ParseLogHelper Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (lockHelper)
        //            {
        //                if (_instance == null)
        //                {
        //                    _instance = new ParseLogHelper();
        //                }
        //            }
        //        }
        //        return _instance;
        //    }
        //}
        //#endregion 单例模式

        public const string TIME_REGEX =        @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)";

        public const string DATE_REGEX =        @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))";

        public const string DATETIME_REGEX =    @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)";

        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ParseLogHelper(ConfigInfo cfg)
        {
            config = cfg;

            sectionList = new List<OneSection>();

            curTag = LineTag.Line_None;
        }

        ConfigInfo config;

        private List<OneSection> sectionList;

        private List<string> curSection = new List<string>();
        //private string curSection_startTime;

        //private LineTag preTag;
        private LineTag curTag;

        public void ParsePerLine(string strLine)
        {
            try
            {
                if (strLine.Contains(config.m_sectionRule.StartFlag))
                {
                    if (curTag == LineTag.Line_Middle)
                    {
                        OneSection section = new OneSection();
                        section.lineList = curSection;
                        sectionList.Add(section);
                    }

                    curTag = LineTag.Line_Start;
                }
                else if (strLine.Contains(config.m_sectionRule.EndFlag))
                {
                    if (curTag == LineTag.Line_Middle)
                    {
                        OneSection section = new OneSection();
                        section.lineList = curSection;
                        sectionList.Add(section);
                    }

                    curTag = LineTag.Line_End;
                }
                else
                {
                    //string time, content;
                    //SpliteTimeContent(strLine, out time, out content);

                    if (curTag == LineTag.Line_Start)
                    {
                        curSection = new List<string>();
                        curTag = LineTag.Line_Middle;
                    }

                    if(curTag == LineTag.Line_Middle)
                    {
                        curSection.Add(strLine);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                throw ex;
            }
        }

        public List<CsvDataFormat> Convert2CsvDataList()
        {
            List<CsvDataFormat> csvDataList = new List<CsvDataFormat>();

            foreach(OneSection sec in sectionList)
            {
                CsvDataFormat csvData = ParsePerSection(sec);

                csvDataList.Add(csvData);
            }

            return csvDataList;
        }

        private CsvDataFormat ParsePerSection(OneSection section)
        {
            CsvDataFormat csvData = new CsvDataFormat();

            //init headValueDict
            foreach(KeywordInfo info in config.m_keywordList)
            {
                csvData.headValueDict.Add(info.CsvField, "");
            }

            foreach(string row in section.lineList)
            {
                foreach(KeywordInfo info in config.m_keywordList)
                {
                    if (String.IsNullOrEmpty(info.LogField))
                        continue;

                    if (row.Contains(info.LogField))
                    {
                        //ExtractFormat exFmt = GetFormatTypeByContent(info.Format);

                        string rowDateTime = string.Empty;
                        string rowTrimTime = TrimDateTimeString(row, ref rowDateTime);

                        //string[] splitArr = row.Split(config.m_sectionRule.RowSplitters.ToArray(), StringSplitOptions.RemoveEmptyEntries);
                        
                        //if(splitArr.Length > 1)
                        //{
                        //    foreach(string str in splitArr)
                        //    {
                                MatchCongifKeywords(rowTrimTime, rowDateTime, ref csvData);
                        //    }
                        //}
                        //else
                        //{
                        //    MatchCongifKeyword(row, info, rowDateTime, ref csvData);
                        //}

                        break;
                    }
                }
            }

            return csvData;
        }

        private string TrimDateTimeString(string strRow, ref string dateTime)
        {
            string strTrim = string.Empty;

            Regex reg1 = new Regex(DATETIME_REGEX);
            Match match1 = reg1.Match(strRow);
            if (match1.Success)
            {
                dateTime = match1.Groups[0].Value;
            }
            else
            {
                Regex reg2 = new Regex(TIME_REGEX);
                Match match2 = reg2.Match(strRow);
                if (match2.Success)
                {
                    dateTime = match2.Groups[0].Value;
                }
            }

            if(!string.IsNullOrEmpty(dateTime))
            {
                strTrim = strRow.Replace(dateTime,"");
            }
            else
            {
                strTrim = strRow;
            }

            return strTrim;
        }

        private void MatchCongifKeyword(string row, KeywordInfo keyInfo, string dateTime, ref CsvDataFormat csvData)
        {
            try
            {
                string strExtact = string.Empty;

                //int index = row.IndexOf(keyInfo.LogField);

                string findKwStr = row.Substring(row.IndexOf(keyInfo.LogField) + keyInfo.LogField.Length);

                ExtractFormat fmt = GetFormatTypeByContent(keyInfo.Format);
                if (fmt == ExtractFormat.Format_number)     //数字
                {
                    strExtact = ExtractNumberFromString(findKwStr);
                }
                else if (fmt == ExtractFormat.Format_num_array) //多个数字
                {
                    int num = 0;
                    if(!int.TryParse(keyInfo.DigitNum, out num))
                    {
                        if(keyInfo.DigitNum == "x" || string.IsNullOrEmpty(keyInfo.DigitNum))
                            num = 100;
                    }
                    strExtact = ExtractNumArrayFromString(findKwStr, num);
                }
                else if (fmt == ExtractFormat.Format_string) //字符串
                {
                    strExtact = findKwStr.Replace(keyInfo.LogField, "");
                }
                else if (fmt == ExtractFormat.Format_dateTime) //日期时间
                {
                    strExtact = dateTime;
                }

                //var firstKey = csvData.headValueDict.FirstOrDefault(q => q.Value == keyword).Key;  //get first key
                csvData.headValueDict[keyInfo.CsvField] = strExtact;
            }
            catch(Exception ex)
            {
                log.Info(ex.Message);
            }
        }

        private void MatchCongifKeywords(string row, string dateTime, ref CsvDataFormat csvData)
        {
            foreach (KeywordInfo info in config.m_keywordList)
            {
                if (String.IsNullOrEmpty(info.LogField))
                    continue;
                if(row.Contains(info.LogField))
                {
                    MatchCongifKeyword(row, info, dateTime, ref csvData);
                }
            }
        }

        private string ExtractNumberFromString(string text)
        {
            Regex r = new Regex("[+-]?\\d+\\.?\\d*");
            bool ismatch = r.IsMatch(text);
            MatchCollection mc = r.Matches(text);

            string result = string.Empty;
            if(mc.Count > 0)
            {
                result = mc[0].Value;
            }

            return result;
        }

        private string ExtractNumArrayFromString(string text,int digitNum)
        {
            Regex r = new Regex("\\d+\\.?\\d*");
            bool ismatch = r.IsMatch(text);
            MatchCollection mc = r.Matches(text);

            string result = string.Empty;
            if (mc.Count > 0)
            {
                int count = 0;
                foreach(Match m in mc)
                {
                    result = result + m.Value + ",";
                    count += 1;

                    if (count >= digitNum)
                        break;
                }

                result = result.TrimEnd(',');
            }

            return result;
        }

        private ExtractFormat GetFormatTypeByContent(string content)
        {
            ExtractFormat type;

            switch (content)
            {
                case "数字":
                    type = ExtractFormat.Format_number;
                    break;
                case "多个数字":
                    type = ExtractFormat.Format_num_array;
                    break;
                case "字符串":
                    type = ExtractFormat.Format_string;
                    break;
                case "日期时间":
                    type = ExtractFormat.Format_dateTime;
                    break;
                default:
                    type = ExtractFormat.Format_number;
                    break;
            }

            return type;
        }

        public bool WordsIScn(string words)
        {
            string TmmP;
            for (int i = 0; i < words.Length; i++)
            {
                TmmP = words.Substring(i, 1);
                byte[] sarr = System.Text.Encoding.GetEncoding("gb2312").GetBytes(TmmP);
                if (sarr.Length == 2)
                {
                    return true;
                }
            }
            return false;
        }

        public string GB2312toUTF8(string str)
        {
            Encoding utf8;
            Encoding gb2312;
            utf8 = Encoding.GetEncoding("UTF-8");
            gb2312 = Encoding.GetEncoding("GB2312");
            byte[] gb = gb2312.GetBytes(str);
            gb = Encoding.Convert(gb2312, utf8, gb);
            return utf8.GetString(gb);
        }
    }
}

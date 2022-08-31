using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogConvertTool
{
    class CsvDataFormat
    {
        public Dictionary<string,string> headValueDict = new Dictionary<string, string>();
        //public string ProductID;
        //public string ProgramName;
        //public string PgNGCode;

        //public string BestFlicker;
        //public string BestVCom;

        //public string WhiteLv;
        //public string WhiteChromaX;
        //public string WhiteChromaY;

        //public string RedLv;
        //public string RedChromaX;
        //public string RedChromaY;

        //public string GreenLv;
        //public string GreenChromaX;
        //public string GreenChromaY;

        //public string BlueLv;
        //public string BlueChromaX;
        //public string BlueChromaY;

        //public string Ntsc;
        //public string Contrast;

        //public string CCT_ColorTemp;
        //public string WorkTime;
    }

    class OneSection
    {
        public List<string> lineList;
        //public string startTime;
    }


    public enum ExtractFormat
    {
        Format_number = 0,
        Format_num_array,
        Format_string,
        Format_dateTime
    };

    public class KeywordInfo
    {
        public string CsvField { get; set; }
        public string LogField { get; set; }

        public string Format { get; set; }

        public string DigitNum { get; set; }

        //public static bool operator == (KeywordInfo lhs, KeywordInfo rhs)
        //{
        //    bool status = false;
        //    if (lhs.CsvField == rhs.CsvField)
        //    {
        //        status = true;
        //    }
        //    return status;
        //}
        //public static bool operator != (KeywordInfo lhs, KeywordInfo rhs)
        //{
        //    bool status = false;
        //    if (lhs.CsvField != rhs.CsvField)
        //    {
        //        status = true;
        //    }
        //    return status;
        //}
    }

    public class SectionRule
    {
        public string StartFlag { get; set; }
        public string EndFlag { get; set; }

        public List<string> RowSplitters = new List<string>();
    }

    public class ConfigInfo
    {
        public List<KeywordInfo>    m_keywordList = new List<KeywordInfo>();
        public SectionRule          m_sectionRule;
    }
}

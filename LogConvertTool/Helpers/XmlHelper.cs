using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogConvertTool
{
    public class XmlHelper
    {
        public static string XmlLocationPath => Environment.CurrentDirectory + "\\Config\\";

        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public XmlHelper()
        {
        }

        public static void GetConfigInfoFromXml(string path, out ConfigInfo info)
        {
            info = new ConfigInfo();

            List<KeywordInfo> kwords;
            GetAllKeywords(path,out kwords);
            SectionRule secRule;
            GetSectionRule(path, out secRule);

            info.m_keywordList = kwords;
            info.m_sectionRule = secRule;
        }

        public static void createXmlFile(string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //创建一个根节点（一级）
            XmlElement root = doc.CreateElement("Root");
            doc.AppendChild(root);
            //创建节点（二级）
            XmlNode node1 = doc.CreateElement("SectionRules");
            XmlNode node2 = doc.CreateElement("Keywords");
            root.AppendChild(node1);
            root.AppendChild(node2);
            doc.Save(path);
        }

        public static void GetAllKeywords(string path, out List<KeywordInfo> kwList)
        {
            kwList = new List<KeywordInfo>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNodeList xnList = doc.SelectNodes("/Root/Keywords/Keyword");
                if(xnList.Count>0)
                {
                    foreach(XmlNode n in xnList)
                    {
                        KeywordInfo info = new KeywordInfo();
                        info.CsvField = n.SelectSingleNode("CsvField").InnerText;
                        info.LogField = n.SelectSingleNode("LogField").InnerText;
                        string formatText = n.SelectSingleNode("Format").InnerText;
                        if(string.IsNullOrEmpty(formatText))
                        {
                            formatText = "数字";
                        }
                        info.Format = formatText;

                        string digNumText = String.Empty;
                        XmlNode digNumNode = n.SelectSingleNode("DigitNum");
                        if(digNumNode != null)
                        {
                            digNumText = digNumNode.InnerText;
                        }
                        info.DigitNum = digNumText;

                        kwList.Add(info);
                    }
                }
                //value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch(XmlException ex) 
            {
                log.Info(ex.Message);
            }
        }

        public static void GetSectionRule(string path, out SectionRule rule)
        {
            rule = new SectionRule();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNode node1 = doc.SelectSingleNode("/Root/SectionRules/StartFlag");
                if (node1 != null) rule.StartFlag = node1.InnerText;

                XmlNode node2 = doc.SelectSingleNode("/Root/SectionRules/EndFlag");
                if (node2 != null) rule.EndFlag = node2.InnerText;

                XmlNodeList xnList = doc.SelectNodes("/Root/SectionRules/RowSplitters/Splitter");
                if (xnList.Count > 0)
                {
                    foreach (XmlNode n in xnList)
                    {
                        rule.RowSplitters.Add(n.InnerText);
                    }
                }
                //value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch (XmlException ex)
            {
                log.Info(ex.Message);
            }
        }

        public static void UpdateAllKeywords(string path, List<KeywordInfo> kwList, out bool isSaveIgnore)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode parentNode = doc.SelectSingleNode("/Root/Keywords");
                if(parentNode != null)
                {
                    parentNode.RemoveAll();
                }

                isSaveIgnore = false;
                foreach (KeywordInfo info in kwList)
                {
                    XmlElement xe = doc.CreateElement("Keyword");
                    XmlNode child1 = doc.CreateElement("CsvField");
                    child1.InnerText = info.CsvField;
                    XmlNode child2 = doc.CreateElement("LogField");
                    child2.InnerText = info.LogField;
                    XmlNode child3 = doc.CreateElement("Format");
                    child3.InnerText = info.Format;
                    xe.AppendChild(child1);
                    xe.AppendChild(child2);
                    xe.AppendChild(child3);

                    XmlNode child4 = doc.CreateElement("DigitNum");
                    if(info.Format == "多个数字")
                    {
                        child4.InnerText = info.DigitNum;
                    }
                    else
                    {
                        child4.InnerText = String.Empty;
                        if(!string.IsNullOrEmpty(info.DigitNum))
                        {
                            isSaveIgnore = true;
                        }
                    }
                        
                    xe.AppendChild(child4);

                    if (parentNode != null)
                    {
                        parentNode.AppendChild(xe);
                    }
                }

                doc.Save(path);
            }
            catch ( XmlException ex )
            {
                throw ex;
            }
        }

        public static void UpdateSecRules(string path, SectionRule rule)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode parentNode = doc.SelectSingleNode("/Root/SectionRules");
                if (parentNode != null)
                {
                    parentNode.RemoveAll();
                }
                else
                {
                    return;
                }

                XmlNode child1 = doc.CreateElement("StartFlag");
                child1.InnerText = rule.StartFlag;
                XmlNode child2 = doc.CreateElement("EndFlag");
                child2.InnerText = rule.EndFlag;

                XmlNode child3 = doc.CreateElement("RowSplitters");
                foreach(string item in  rule.RowSplitters)
                {
                    XmlNode nd = doc.CreateElement("Splitter");
                    nd.InnerText = item;
                    child3.AppendChild(nd);
                }

                parentNode.AppendChild(child1);
                parentNode.AppendChild(child2);
                parentNode.AppendChild(child3);

                doc.Save(path);
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        /**************************************************
            * 使用示列:
            * XmlHelper.Read(path, "/Node", "")
            * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
            ************************************************/
        public static string Read(string path, string node, string attribute)
        {
            string value = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch { }
            return value;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
            * 使用示列:
            * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
            * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
            * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
            ************************************************/
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(path);
            }
            catch ( XmlException ex )
            {
                log.Error(ex.Message);
            }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
            * 使用示列:
            * XmlHelper.Insert(path, "/Node", "", "Value")
            * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
            ************************************************/
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                doc.Save(path);
            }
            catch { }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
            * 使用示列:
            * XmlHelper.Delete(path, "/Node", "")
            * XmlHelper.Delete(path, "/Node", "Attribute")
            ************************************************/
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                doc.Save(path);
            }
            catch { }
        }
    }
}

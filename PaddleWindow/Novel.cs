using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PaddleWindow
{
    public class Novel
    {
        public class Chapter
        {
            public string Title;
            public string Content;
            public override string ToString()
            {
                return $"{Title}";
            }
        }

        public string Title = "未命名";

        public int ChapterCount { get { return chapters.Count; } }

        public int charCount { get; private set; }

        public List<Chapter> chapters { get; private set; }


        public Novel()
        {

        }


        public static Novel LoadByFile(string filePath)
        {
            Novel novel = new Novel();
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open);
                using (StreamReader sr = new StreamReader(fs,Encoding.UTF8))
                {
                    string content = sr.ReadToEnd();
                    sr.Close();
                    fs.Close();
                    novel.chapters = LoadChapter(content);
                }
            }
            catch (Exception)
            {
                fs?.Close();
                throw;
            }
            return novel;
        }

        private static List<Chapter> LoadChapter(string novelContent)
        {
            List<Chapter> rs = new List<Chapter>();
            novelContent = novelContent.Replace("\r","");
            string[] cs = novelContent.Split('\n');
            int i = 0;
            string strLine = cs[i]; //读取文件中的一行
            StringBuilder stringBuilder = new StringBuilder();
            Chapter preChap = new Chapter();
            while (i < cs.Length)
            {
                string[] patArr = { "第(.*?)章", "第(.*?)回", "第(.*?)集", "第(.*?)卷", "第(.*?)部", "第(.*?)篇", "第(.*?)节", "第(.*?)季" };
                bool flag = false;
                foreach (var pattern in patArr)
                {
                    Regex reg = new Regex(pattern);
                    if (reg.IsMatch(strLine))
                    {
                        if (strLine.Length < 20)//超过20个字的标题，可能是小说里面出现的“书中书、文中文”...
                        {
                            flag = true;
                            Chapter chapter = new Chapter()
                            {
                                Title = TrimBlank(strLine),
                            };
                            preChap.Content = stringBuilder.ToString();
                            preChap = chapter;
                            rs.Add(chapter);
                            stringBuilder.Clear();
                        }
                        break;
                    }
                }
                if (!flag)
                {
                    stringBuilder.AppendLine(strLine);
                }
                strLine = cs[i++];
            }
            return rs;
        }

        private static string TrimBlank(string str)
        {
            return str.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        }

        public override string ToString()
        {
            return $"名称：{Title},章节数量{ChapterCount},字数：{charCount}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using PaddleWindow.Utils;

namespace PaddleWindow
{
    /// <summary>
    /// NavelManagerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NavelManagerWindow : Window
    {
        public NavelManagerWindow()
        {
            string path = @"C:\Users\Administrator\Desktop\测试环境\Test.txt";
            InitializeComponent();
            this.chapterListBox.ItemsSource = ReadChapterList(path);
        }


        private List<string> ReadChapterList(string path)
        {
            return NovelExtractUtil.LoadChapter(path);
        }
    }
}

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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using PaddleWindow.Utils;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace PaddleWindow
{
    /// <summary>
    /// PaddleControl.xaml 的交互逻辑
    /// </summary>
    public partial class PaddleControl : UserControl
    {
        private readonly string path = @"C:\Users\Administrator\Desktop\测试环境\Test.txt";

        private Novel novel;

        private int currentIndex = 0;

        private enum LeftPanelState
        {
            Hide,
            Show,
            Moveing
        }

        private LeftPanelState leftPanelState = LeftPanelState.Hide;

        public PaddleControl()
        {
            InitializeComponent();
            ReadNovelChapter(path);
            RenderChapter(novel.chapters[currentIndex]);
            this.novelLabel.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            this.chapterListBox.AddHandler(UIElement.MouseDownEvent,
    new MouseButtonEventHandler((x, s) =>
    {
        Console.WriteLine((this.chapterListBox.SelectedItem as Novel.Chapter).Content);
    }), true);
            this.novelLabel.MouseWheel += NovelLabel_MouseWheel;
        }

        private void NovelLabel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && currentIndex > 0)
            {
                RenderChapter(novel.chapters[--currentIndex]);
                this.novelLabel.ScrollToEnd();
                this.novelLabel.ScrollToEnd();
                Console.WriteLine("上一章");
            }
            else if(e.Delta < 0 && currentIndex < novel.chapters.Count - 1)
            {
                RenderChapter(novel.chapters[++currentIndex]);
                this.novelLabel.ScrollToHome();
                Console.WriteLine("下一章");
            }
        }

        private void RenderChapter(Novel.Chapter chapter)
        {
            this.novelLabel.Text = "\r\n\r\n\r\n" + chapter.Title + "\r\n\r\n\r\n" + chapter.Content;
        }

        private void ReadNovelChapter(string path)
        {
            novel = Novel.LoadByFile(path);
            this.chapterListBox.ItemsSource = novel.chapters;
        }


        private void HideLeftPanle()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation
            {
                Duration = new Duration
(TimeSpan.FromSeconds(.5))
            };

            TranslateTransform transform = new TranslateTransform();
            leftPanel.RenderTransform = transform;
            doubleAnimation.To = -250;
            doubleAnimation.Completed += (a, b) =>
            {
                leftControlBtn.Content = ">";
                leftPanelState = LeftPanelState.Hide;
            };
            transform.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
        }

        private void ShowLeftPanle()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation
            {
                Duration = new Duration
(TimeSpan.FromSeconds(.5))
            };

            TranslateTransform transform = new TranslateTransform();
            leftPanel.RenderTransform = transform;
            doubleAnimation.To = 0;
            doubleAnimation.Completed += (a, b) =>
            {
                leftControlBtn.Content = "<";
                leftPanelState = LeftPanelState.Show;
            };
            transform.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
        }


        private void leftControlBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (leftPanelState)
            {
                case LeftPanelState.Hide:
                    ShowLeftPanle();
                    break;
                case LeftPanelState.Show:
                    HideLeftPanle();
                    break;
                case LeftPanelState.Moveing:
                    break;
                default:
                    break;
            }
        }
    }
}

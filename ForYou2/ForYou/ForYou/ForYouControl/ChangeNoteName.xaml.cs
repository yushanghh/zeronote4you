using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ForYou.ForYouControl
{
    /// <summary>
    /// ChangeNoteName.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeNoteName : Window
    {
        public ChangeNoteName()
        {
            InitializeComponent();
            FocusManager.SetFocusedElement(this, tbContent);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tbContent.Text = tbContent.Text.Trim();
            if (tbContent.Text.Length > 0)
            {
                NoteView.note_name = tbContent.Text.ToString();
                Close();//关闭窗口
            }
            else
            {
                MessageBox.Show("输入的字符串长度不能为空！");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
                Close();//关闭窗口
        }
    }
}

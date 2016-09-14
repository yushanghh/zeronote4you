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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ForYou.ForYouControl;
using WPF_WYSIWYG_HTML_Editor;
using System.IO;

namespace ForYou
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void tabcontrolNoteTag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i=tabcontrolNoteTag.SelectedIndex;

            TabItem tci = tabcontrolNoteTag.SelectedItem as TabItem;

            

            //点击+选项框的操作
            if (i == tabcontrolNoteTag.Items.Count - 1 && tabcontrolNoteTag.Items.Count != 1)
            {
                User.rtbContent.IsEnabled = false;
                addNewTab(i);
            }
            else if (i != -1 && tabcontrolNoteTag.Items.Count != 1)
            {
                if (((tci.Header as TextBox).Name).Substring(5).Equals((User.SelectGroup)))
                {

                }
                else
                {
                    User.SelectGroup = ((tci.Header as TextBox).Name).Substring(5);

                    //在此从数据库处获得该组下一个note的编号

                   // User.NextNoteNum = 1;
                    //获得结束
                    User.rtbContent.RibbonButtonNew_Click(sender, e);
                    //(((tci.Content as Grid).Children[0]) as NoteView).Grid_Loaded(sender, e);
                   (((tci.Content as Grid).Children[0]) as NoteView).lvNoteTitle_SelectionChanged(sender, e);
                }
            }
        }

        private void tabMenuItem_Click(object sender, RoutedEventArgs e)
        {

            TabItem tci = ((e.Source as MenuItem).Parent as ContextMenu).PlacementTarget as TabItem;

            if ((e.Source as MenuItem).Header.ToString() == "删除")
            {
                    tabcontrolNoteTag.Items.IndexOf(tci);
                    int i = tabcontrolNoteTag.Items.IndexOf(tci);
                    if (i == tabcontrolNoteTag.Items.Count - 2)
                    {
                        tabcontrolNoteTag.SelectedIndex = i - 1;
                    };
                    int group_id= int.Parse(((tci.Header as TextBox).Name).Substring(5));
                    DirectoryInfo di = new DirectoryInfo(System.IO.Path.Combine(User.UserPath, ((tci.Header as TextBox).Name).Substring(5)));
                    di.Delete(true);
                    function.delete_group(group_id);
                    tabcontrolNoteTag.Items.Remove(tci);
                    if(tabcontrolNoteTag.Items.Count==1)
                    {
                        //tabcontrolNoteTag.Items.Clear();
                        addNewTab(0);
                    }
            }
            else if((e.Source as MenuItem).Header.ToString() == "重命名")
            {
                TextBox tbHeader = tci.Header as TextBox;
                tbHeader.IsReadOnly = false;
                tbHeader.Focusable = true;
                tbHeader.Focus();
            }
        }



        private void tbHeader_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbHeader = sender as TextBox;

            if(tbHeader.Text=="")
            {
                MessageBox.Show("组名不能为空");
                tbHeader.Focus();
            }
            else 
            {
                tbHeader.IsReadOnly = true;
                tbHeader.Focusable = false;
                //操作数据库  group i 改编号为 i 的组名 
                int num;

                num = int.Parse((tbHeader.Name).Substring(5));
                function.modify_groupname(num, tbHeader.Text);
            }
        }

        private void windowMain_Loaded(object sender, RoutedEventArgs e)
        {

            //html编辑器的载入 
            User.rtbContent = new WebEditor();
            Grid.SetRow(User.rtbContent, 3);
            Grid.SetColumn(User.rtbContent, 3);
            Grid.SetColumnSpan(User.rtbContent, 2);
            User.rtbContent.IsEnabled = false;
            gridHtmlEditor.Children.Add(User.rtbContent);
            //html编辑器载入完成
            

            //数据读入阶段 获取组名 用户名 第一组的笔记列表
            User.UserId = "zhanyuqi";
            //User.UserGroup=new List<object>();
            //Num file = new Num();
            //file.Name = "物理";
            //file.NumOfName = 1;
            //User.UserGroup.Add(file);
            //User.NextGroupNum = 2;
            ////User.NextNoteNum = 1;

            //获取分组
            List<Num> all_group = new List<Num>();
            all_group = function.get_group<Num>();
            User.UserGroup = new List<object>();
            foreach (var num in all_group)
            {
                Num file = num;
                User.UserGroup.Add(file);
            }
            //User.NextGroupNum ++;
            //User.NextNoteNum = 1;
            //数据读入完成


            //文件操作阶段
            string path;
            string str = User.UserId;                           
            path = this.GetType().Assembly.Location;            //获得exe所在路径
            path = System.IO.Path.GetDirectoryName(path);       //改为目录
            path = System.IO.Path.Combine(path, str);           //与str合并为单独文件夹
            Directory.CreateDirectory(path);                    //创建名为Data的文件夹 有就不会创
            User.UserPath = path;
            //path_file = Directory.GetFiles(path);               //获得Data下的所有文件
            //文件操作完成


            TabItem tci;
            if (User.UserGroup.Count!=0)
            { 
                for(int i=0;i<User.UserGroup.Count;i++)
                { 
                    tci = new TabItem();
                    //标题
                    TextBox tbHeader = new TextBox();
                    tbHeader.Name = "Group"+(User.UserGroup[i] as Num).group_id;
                    tbHeader.Text = (User.UserGroup[i] as Num).group_name;                               //标题名称
                    
                    //创建对应的文件夹
                    path = System.IO.Path.Combine(User.UserPath, (User.UserGroup[i] as Num).group_id.ToString());
                    Directory.CreateDirectory(path);                  


                    tbHeader.IsReadOnly = true;
                    tbHeader.Focusable = false;
                    tbHeader.BorderThickness = new Thickness(0);
                    tbHeader.Background = Brushes.Transparent;
                    tbHeader.LostKeyboardFocus += tbHeader_LostKeyboardFocus;

                    tci.Header = tbHeader;

                    Grid temGrid = new Grid();

                    NoteView noteContent = new NoteView((User.UserGroup[i] as Num).group_id);

                    temGrid.Children.Add(noteContent);

                    tci.Content = temGrid;

                    ContextMenu tabMenu = new ContextMenu();
                    MenuItem tabMenuItem = new MenuItem();
                    tabMenuItem.Header = "删除";
                    tabMenuItem.Click += tabMenuItem_Click;
                    tabMenu.Items.Add(tabMenuItem);

                    tabMenuItem = new MenuItem();
                    tabMenuItem.Header = "重命名";
                    tabMenuItem.Click += tabMenuItem_Click;
                    tabMenu.Items.Add(tabMenuItem);


                    tci.ContextMenu = tabMenu;

                    tabcontrolNoteTag.Items.Add(tci);
                }
            }
            else 
            {

            }
            tci = new TabItem();
            tci.Header = "+";
            tabcontrolNoteTag.Items.Add(tci);
        }

        public void addNewTab(int i)
        {
            TabItem tci = new TabItem();
            //标题
            TextBox tbHeader = new TextBox();
            tbHeader.Text = "新建组";                               //标题名称

            //增加分组
            function.insert_group("新建组");

            //数据库操作 增加一个组
            tbHeader.Name = "Group" + function.get_maxid(0);

            

            string path;
            path = System.IO.Path.Combine(User.UserPath, function.get_maxid(0).ToString());           //与str合并为单独文件夹
            Directory.CreateDirectory(path);                    //创建名为Data的文件夹 有就不会创



            //User.NextGroupNum++;
            //数据库操作



            tbHeader.IsReadOnly = true;
            tbHeader.Focusable = false;
            tbHeader.BorderThickness = new Thickness(0);
            tbHeader.Background = Brushes.Transparent;
            tbHeader.LostKeyboardFocus += tbHeader_LostKeyboardFocus;
            tci.Header = tbHeader;
            Grid temGrid = new Grid();
            NoteView noteContent = new NoteView();
            temGrid.Children.Add(noteContent);
            tci.Content = temGrid;

            ContextMenu tabMenu = new ContextMenu();
            MenuItem tabMenuItem = new MenuItem();
            tabMenuItem.Header = "删除";
            tabMenuItem.Click += tabMenuItem_Click;
            tabMenu.Items.Add(tabMenuItem);

            tabMenuItem = new MenuItem();
            tabMenuItem.Header = "重命名";
            tabMenuItem.Click += tabMenuItem_Click;
            tabMenu.Items.Add(tabMenuItem);

            tci.ContextMenu = tabMenu;

            tabcontrolNoteTag.Items.Add(tci);

            tci = new TabItem();
            tci.Header = "+";
            tabcontrolNoteTag.Items.Add(tci);
            tabcontrolNoteTag.Items.RemoveAt(i);
        }
    }
}

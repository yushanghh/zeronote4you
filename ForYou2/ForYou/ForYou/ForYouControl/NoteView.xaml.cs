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
using WPF_WYSIWYG_HTML_Editor;
using ForYou.ForYouControl;
using System.IO;

namespace ForYou
{
    /// <summary>
    /// NoteContent.xaml 的交互逻辑
    /// </summary>
    public partial class NoteView : UserControl
    {


        private long _Load_Group_Id;
        static public string note_name;
        //WebEditor rtbContent;
        public List<object> listNote;
        public NoteView()
        {
            InitializeComponent();
            listNote = new List<object>();
        }



        public NoteView(long p)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            listNote = new List<object>();
            _Load_Group_Id = p;
        }


        public void Grid_Loaded(object sender, RoutedEventArgs e)      //初始化listview
        {


            listNote = new List<object>();
            List<Note> all_group = new List<Note>();
            //all_group = function.get_note<Note>((int)_Load_Group_Id);
            all_group = function.get_note<Note>(int.Parse(User.SelectGroup));
            //MessageBox.Show(User.SelectGroup);
            User.UserGroup = new List<object>();
            if (all_group != null)
            {
                foreach (var num in all_group)
                {
                    //Num file = num;
                    //User.UserGroup.Add(file);
                    Note note = num;
                    List<string> pic =function.get_pic<string>(int.Parse(User.SelectGroup), (int)note.note_id);
                    note.cout_of_img = pic.Count;
                    //function.
                    //note.note_name = "chenmengmeicun";
                    listNote.Add(note);

                }
                lvNoteTitle.DataContext = listNote;
                lvNoteTitle.Items.Refresh();
            }

            /*Note test = new Note();
            test.Title = "笔记标题";
            test.Content = "文章内容";
            listNote.Add(test);      
            lvNoteTitle.DataContext = listNote;
            lvNoteTitle.Items.Refresh();*/

            //rtbContent = new WebEditor();
            //Grid.SetRow(rtbContent, 3);
            //Grid.SetColumn(rtbContent, 3);
            //Grid.SetColumnSpan(rtbContent, 2);
            //grid.Children.Add(rtbContent);
        }

        public void lvNoteTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Note note = lvNoteTitle.SelectedItem as Note;
            if (note != null && note is Note)
            {
                User.rtbContent.IsEnabled = true;
                string path;
                path = System.IO.Path.Combine(User.UserPath, User.SelectGroup);
                path = System.IO.Path.Combine(path, note.note_id.ToString());
                Directory.CreateDirectory(path);

                User.rtbContent.directory = path;
                //数据库获得下一张图片编号
                User.rtbContent.img_num = note.cout_of_img+1;
                //结束
                User.rtbContent.path = System.IO.Path.Combine(path, User.SelectGroup+"-"+ note.note_id.ToString() + ".html");

                User.rtbContent.RibbonButtonOpen_Click(sender,e);
                //MessageBox.Show("员工姓名：" + note.Title +
                //    "\n\n" + "员工年龄：" + note.Content + "\n\n");
                ////tbTitle.Text = note.Title;
                ////tbTitle.Focus();
                //rtbContent.Document.Blocks.Clear();
                //Paragraph paragraph = new Paragraph(new Run(note.Content));
                //rtbContent.Document.Blocks.Add(paragraph);
            }  
        }

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            //文件操作
            string path;
            int group_id= int.Parse(User.SelectGroup);
            path = System.IO.Path.Combine(User.UserPath, User.SelectGroup);
            function.insert_note(group_id);
            int note_id = function.get_maxid(1);
            path = System.IO.Path.Combine(path, note_id.ToString());
            Directory.CreateDirectory(path);
            List<string> hello = new List<string>();
            
            
            //User.NextNoteNum++;

            User.rtbContent.path = System.IO.Path.Combine(path, User.SelectGroup + "-" + note_id.ToString() + ".html");
            User.rtbContent.RibbonButtonNew_Click(sender, e);
            User.rtbContent.RibbonButtonSave_Click(sender,e);


            function.modify_note(group_id, note_id, User.rtbContent.path, hello);
            function.modify_notename(group_id, note_id, "笔记标题");


            Note test = new Note();
            test.note_name = "笔记标题";
            test.Content = "文章内容";
            test.note_id = note_id;
            test.modify_time = DateTime.Now.ToString();
            listNote.Add(test);
            lvNoteTitle.DataContext = listNote;
            lvNoteTitle.Items.Refresh();
            lvNoteTitle.SelectedIndex = listNote.Count - 1;

            
        }

        private void lvNoteTitle_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //ListView
            ListView aItem = (ListView)sender;
           // MessageBox.Show(aItem.SelectedIndex.ToString());

            Note note = lvNoteTitle.SelectedItem as Note;
            if (note != null && note is Note)
            { 
            ContextMenu tabMenu = new ContextMenu();
            MenuItem tabMenuItem;

            tabMenuItem = new MenuItem();
            tabMenuItem.Header = "设置笔记标题";
            tabMenuItem.Click += lvRename_Click;
            tabMenu.Items.Add(tabMenuItem);

            tabMenuItem = new MenuItem();
            tabMenuItem.Header = "删除";
            tabMenuItem.Click += lvDelete_Click;
            tabMenu.Items.Add(tabMenuItem);

            aItem.ContextMenu = tabMenu;
            }

        }


        private void lvDelete_Click(object sender, RoutedEventArgs e)
        {
            ListView lvNote = ((e.Source as MenuItem).Parent as ContextMenu).PlacementTarget as ListView;
            listNote = (List<object>)lvNote.DataContext;
            if (lvNote.SelectedIndex != -1)
            {
                Note note = (Note)listNote[lvNote.SelectedIndex];
                function.delete_note(int.Parse(User.SelectGroup), (int)note.note_id);
                listNote.RemoveAt(lvNote.SelectedIndex);
                lvNote.DataContext = listNote;
                lvNote.Items.Refresh();
                lvNoteTitle.SelectedIndex = -1;
                User.rtbContent.IsEnabled = false;
            }
        }

        private void lvRename_Click(object sender, RoutedEventArgs e)
        {
            ListView lvNote = ((e.Source as MenuItem).Parent as ContextMenu).PlacementTarget as ListView;
            listNote = (List<object>)lvNote.DataContext;
            if (lvNote.SelectedIndex != -1)
            {
                ChangeNoteName isw = new ChangeNoteName();
                isw.Title = "命名页面";
                isw.ShowDialog();//模式，弹出！
                Note note = (Note)listNote[lvNote.SelectedIndex];
                if (note_name != null)
                {
                    note.note_name = note_name;
                    function.modify_notename(int.Parse(User.SelectGroup), (int)note.note_id, note_name);

                    listNote.RemoveAt(lvNote.SelectedIndex);
                    listNote.Insert(lvNote.SelectedIndex, note);

                    lvNoteTitle.DataContext = listNote;
                    lvNoteTitle.Items.Refresh();
                }
            }
        }
    }
}

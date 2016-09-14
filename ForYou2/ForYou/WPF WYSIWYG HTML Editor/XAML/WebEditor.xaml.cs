using mshtml;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPF_WYSIWYG_HTML_Editor
{
    public partial class WebEditor : UserControl
    {
        public string path { get; set; }
        public string name { get; set; }
        public string directory { get; set; }
        public int img_num { get; set; }
        public WebEditor()
        {
            InitializeComponent();            
        }

        private void SettingsBold_Click(object sender, RoutedEventArgs e)
        {
            Format.bold();            
        }

        private void SettingsItalic_Click(object sender, RoutedEventArgs e)
        {
            Format.Italic();
        }

        private void SettingsUnderLine_Click(object sender, RoutedEventArgs e)
        {
            Format.Underline();
        }

        private void SettingsRightAlign_Click(object sender, RoutedEventArgs e)
        {
            Format.Underline();
        }

        private void SettingsLeftAlign_Click(object sender, RoutedEventArgs e)
        {
            Format.JustifyLeft();
        }

        private void SettingsCenter2_Click(object sender, RoutedEventArgs e)
        {
            Format.JustifyCenter();
        }

        private void SettingsJustifyRight_Click(object sender, RoutedEventArgs e)
        {
            Format.JustifyRight();
        }

        private void SettingsJustifyFull_Click(object sender, RoutedEventArgs e)
        {
            Format.JustifyFull();
        }

        private void SettingsInsertOrderedList_Click(object sender, RoutedEventArgs e)
        {
            Format.InsertOrderedList();
        }

        private void SettingsBullets_Click(object sender, RoutedEventArgs e)
        {
            Format.InsertUnorderedList();
        }

        private void SettingsOutIdent_Click(object sender, RoutedEventArgs e)
        {
            Format.Outdent();            
        }

        private void SettingsIdent_Click(object sender, RoutedEventArgs e)
        {
            Format.Indent();  
        }

        public void RibbonButtonNew_Click(object sender, RoutedEventArgs e)
        {
            Gui.newdocument();
        }

        public void RibbonButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            Gui.newdocumentFile(path);
        }

        private void RibbonButtonOpenweb_Click(object sender, RoutedEventArgs e)
        {
            webBrowserEditor.newWb(@"http://www.codeproject.com/");
        }

        private void SettingsFontColor_Click(object sender, RoutedEventArgs e)
        {
            Gui.SettingsFontColor();
        }

        private void SettingsBackColor_Click(object sender, RoutedEventArgs e)
        {
            Gui.SettingsBackColor();
        }

        private void SettingsAddLink_Click(object sender, RoutedEventArgs e)
        {
            Gui.SettingsAddLink();
        }

        private void SettingsAddImage_Click(object sender, RoutedEventArgs e)
        {
            string temp;
            temp=img_num.ToString()+".jpg";
            Gui.SettingsAddImage(System.IO.Path.Combine(directory, temp));//***************************
            img_num++;
        }

        public void RibbonButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Gui.RibbonButtonSave(path);//**********************************************************************************
        }

        private void RibbonComboboxFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Gui.RibbonComboboxFonts(RibbonComboboxFonts);            
        }

        private void RibbonComboboxFontHeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gui.RibbonComboboxFontHeight(RibbonComboboxFontHeight);
        }

        private void RibbonComboboxFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // Gui.RibbonComboboxFormat(RibbonComboboxFormat);
        }

        private void EditWeb_Click(object sender, RoutedEventArgs e)
        {
            Gui.EditWeb();
        }

        private void ViewHTML_Click(object sender, RoutedEventArgs e)
        {
            Gui.ViewHTML();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Gui.webBrowser = webBrowserEditor;
            Gui.htmlEditor = HtmlEditor1;
            Initialisation.webeditor = this;
            Gui.newdocument();
            
            Initialisation.RibbonComboboxFontsInitialisation();
            Initialisation.RibbonComboboxFontSizeInitialisation();
            Initialisation.RibbonComboboxFormatInitionalisation();
        }
    }
}

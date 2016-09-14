﻿using mshtml;
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


namespace WPF_WYSIWYG_HTML_Editor
{
    /// <summary>
    /// Interaction logic for Image.xaml
    /// </summary>
    public partial class Image : Window, IDisposable
    {
        public HTMLDocument doc;

        public string path{get;set;}
        public Image(HTMLDocument Doc)
        {
            InitializeComponent();
            doc = Doc;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            description.Focus();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (doc != null)
            {
                dynamic r = doc.selection.createRange();
                string destPath = path;
                System.IO.File.Delete(destPath);
                System.IO.File.Copy(link.Text, destPath);
                r.pasteHTML(string.Format(@"<img alt=""{1}"" src=""{0}"">", destPath, description.Text));
                //r.pasteHTML(string.Format(@"<img src=""{0}"">", link.Text, description.Text));
                this.Hide();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.png.*)|*.png|All files (*.gif)|*.gif|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    link.Text = openFileDialog.FileName;
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

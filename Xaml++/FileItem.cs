using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Windows.Documents;
using System.Windows;

namespace Xaml__
{
    public class FileItem : INotifyPropertyChanged
    {
        private string path;
        private string text;
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }
        public string Name { get { return System.IO.Path.GetFileName(Path); } }
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }
        public bool Changed { get; set; }
        public bool Created { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FileItem(string path, bool created, string text = null)
        {
            Path = path;
            Changed = false;
            Created = created;
            this.text = text;

            if (text == null)
                this.text = File.ReadAllText(path);
        }

        public void Save(string text)
        {
            if (Changed)
            {
                File.WriteAllText(Path, text);
                Changed = false;
            }
        }
        public void SaveAs(string path, string text)
        {
            Path = path;
            Created = false;
            Save(text);
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

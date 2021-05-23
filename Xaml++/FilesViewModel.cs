using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Xaml__
{
    class FilesViewModel : INotifyPropertyChanged
    {
        private int createdFilesCount;
        private FileItem selectedFile;
        public List<FileItem> Files { get; set; }
        private FileItem SelectedFile
        {
            get { return selectedFile; }
            set
            {
                selectedFile = value;
                OnPropertyChanged("SelectedFile");
            }
        }
        public RelayCommand AddCommand { get; }
        public RelayCommand CreateCommand { get; }

        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        public event PropertyChangedEventHandler PropertyChanged;

        public FilesViewModel()
        {
            createdFilesCount = 0;
            Files = new List<FileItem>();

            openFileDialog = new OpenFileDialog()
            {
                Filter = "XAML files(*.xaml)|*.xaml",
                CheckFileExists = true,
                Multiselect = false
            };
            saveFileDialog = new SaveFileDialog()
            {
                Title = "Save As",
                Filter = "XAML files(*.xaml)|*.xaml"
            };

            AddCommand = new RelayCommand(OpenFile);
            CreateCommand = new RelayCommand(CreateFile);
        }
        private void OpenFile(object parameter)
        {
            if(openFileDialog.ShowDialog() == true)
            {
                FileItem file = new FileItem(openFileDialog.FileName, false);
                Files.Add(file);
                SelectedFile = file;
            }
        }
        private void CreateFile(object parameter)
        {
            createdFilesCount++;
            FileItem file = new FileItem("./new " + createdFilesCount.ToString(), true);
            Files.Add(file);
            SelectedFile = file;
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

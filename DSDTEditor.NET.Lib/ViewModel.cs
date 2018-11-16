using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DSDTEditor.NET.Shared;
using Microsoft.Win32;

namespace DSDTEditor.NET.Lib
{
    public class ViewModel : DSDTProperty
    {
        private string fileName;
        private ObservableCollection<IACPIObject> tables;
        private List<string> fileContent;
        private int currentPosition;
        private RelayCommand openFileCommand;

        public ViewModel()
        {
            this.tables = new ObservableCollection<IACPIObject>();
        }

        public string FileName
        {
            get => this.fileName;
            private set => this.evaluateSelectedFileAndOpenIt(value);
        }

        public ObservableCollection<IACPIObject> Tables
        {
            get => this.tables;
            set => this.SetField(ref this.tables, value);
        }

        public ICommand OpenFileCommand
        {
            get
            {
                if (this.openFileCommand == null)
                    this.openFileCommand = new RelayCommand(() => this.openDslFile());
                return this.openFileCommand;
            }
        }

        private void openDslFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".dsl";
            openFileDialog.Filter = "Decompiled Source Language (.dsl)|*.dsl";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Open Decompiled Source Language File";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
                this.FileName = openFileDialog.FileName;
        }

        private void evaluateSelectedFileAndOpenIt(string fileName)
        {
            if (this.SetField(ref this.fileName, fileName, "FileName"))
            {
                if (Path.GetExtension(this.FileName).ToLowerInvariant().Equals(".dsl"))
                    this.loadFile();
            }
        }

        private void loadFile()
        {
            if (this.fileContent == null)
                this.fileContent = new List<string>();
            else
                this.fileContent.Clear();
            this.currentPosition = 0;
            this.fileContent.AddRange(File.ReadAllLines(this.FileName, Encoding.UTF8));
            this.Tables.Clear();
            this.createObjects();
        }

        private void createObjects()
        {
            List<string> comments = new List<string>();
            bool comment = false;
            while(currentPosition < this.fileContent.Count)
            {
                if (this.fileContent[currentPosition].Trim().StartsWith("/*"))
                {
                    comments.Add(this.fileContent[currentPosition]);
                    comment = true;
                }
                else if (this.fileContent[currentPosition].Trim().StartsWith("*/"))
                {
                    comments.Add(this.fileContent[currentPosition]);
                    comment = false;
                }
                else if (this.fileContent[currentPosition].Trim().StartsWith("DefinitionBlock"))
                {
                    Table table = new Table();
                    if (table.CreateTable(ref currentPosition, this.fileContent, comments))
                        this.Tables.Add(table);
                }

                if (comment)
                    comments.Add(this.fileContent[currentPosition]);

                currentPosition++;
            }
        }
    }
}

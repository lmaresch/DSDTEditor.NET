using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSDTEditor.NET.Shared;

namespace DSDTEditor.NET.Lib
{
    public class ViewModel : DSDTProperty
    {
        private string fileName;
        private ObservableCollection<IACPIObject> tables;
        private List<string> fileContent;
        private int currentPosition;
		public ViewModel()
        {
            this.tables = new ObservableCollection<IACPIObject>();
            this.fileName = @"C:\Users\d053898\Downloads\iasl-win-20181031\amltables.dsl";
            this.currentPosition = 0;
            this.loadFile();
            this.createObjects();
        }

		public string FileName
        {
            get => this.fileName;
            set => this.SetField(ref this.fileName, value);
        }

		public ObservableCollection<IACPIObject> Tables
        {
            get => this.tables;
            set => this.SetField(ref this.tables, value);
        }

		private void loadFile()
        {
            this.fileContent = new List<string>(File.ReadAllLines(this.FileName, Encoding.UTF8));
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

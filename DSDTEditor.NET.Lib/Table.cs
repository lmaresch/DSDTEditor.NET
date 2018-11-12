using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSDTEditor.NET.Shared;

namespace DSDTEditor.NET.Lib
{
    public class Table : DSDTProperty, IACPIObject
    {
        private string name;
        private int revision;
        private string oemId;
        private string oemTableId;
        private int oemRevision;
        private ObservableCollection<IACPIObject> childs;
        private ObservableCollection<string> comment;

        public Table()
        {
            this.childs = new ObservableCollection<IACPIObject>();
            this.comment = new ObservableCollection<string>();
        }

        public string Name { get => this.name; set => this.SetField(ref this.name, value); }
        public ObservableCollection<IACPIObject> Childs { get => this.childs; set => this.SetField(ref this.childs, value); }
        public ACPIObjectTypes ACPIObjectType { get => ACPIObjectTypes.ACPI_TYPE_TABLE; }

        public string FullName { get => this.ToString(); }


        public bool CreateTable(ref int currentPosition, List<string> fileContent, List<string> comments)
        {
            List<string> definition = new List<string>
                (fileContent[currentPosition].Replace("DefinitionBlock (", string.Empty).Replace(")", string.Empty).Split(new char[] { ',' }));
            this.name = definition[1].Replace("\"", "");
            this.revision = int.Parse(definition[2]);
            this.oemId = definition[3];
            this.oemTableId = definition[4];
            //this.oemRevision = int.Parse(definition[5].Substring(2), System.Globalization.NumberStyles.HexNumber);
            currentPosition++;
            bool table = false;
            int level = 0;
            while (currentPosition < fileContent.Count)
            {
                string trimmedString = fileContent[currentPosition].Trim();
                if (trimmedString.StartsWith("{"))
                {
                    if (!trimmedString.Contains("}"))
                    {
                        if (table)
                        {
                            level++;
                            Debug.WriteLine("[Table] Line: {0} - Level: {1} - Inhalt: {2}", currentPosition + 1, level, fileContent[currentPosition]);
                        }
                        else
                        {
                            table = true;
                        }
                    }
                }
                else if (trimmedString.StartsWith("}"))
                {
                    if (level > 0)
                    {
                        level--;
                        Debug.WriteLine("[Table] Line: {0} - Level: {1} - Inhalt: {2}", currentPosition + 1, level, fileContent[currentPosition]);
                    }
                    else
                    {
                        table = false;
                        break;
                    }
                }
                else if (trimmedString.StartsWith("Scope"))
                {
                    Scope scope = new Scope();
                    if (scope.FillObject(ref currentPosition, fileContent))
                        this.Childs.Add(scope);
                }
                else if (trimmedString.StartsWith("Method"))
                {
                    Method scope = new Method();
                    if (scope.FillObject(ref currentPosition, fileContent))
                        this.Childs.Add(scope);
                }

                currentPosition++;
            }
            return true;
        }

        public override string ToString() => "Table " + this.Name;
    }
}

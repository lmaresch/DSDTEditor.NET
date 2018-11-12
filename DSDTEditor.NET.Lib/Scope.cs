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
    public class Scope : DSDTProperty, IACPIObject
    {
        private string name;
        private ObservableCollection<IACPIObject> childs;

        public Scope() => this.childs = new ObservableCollection<IACPIObject>();

        public string Name { get => this.name; set => this.SetField(ref this.name, value); }
        public ObservableCollection<IACPIObject> Childs { get => this.childs; set => this.SetField(ref this.childs, value); }
        public ACPIObjectTypes ACPIObjectType { get => ACPIObjectTypes.ACPI_TYPE_ANY;}
        public string FullName { get => this.ToString(); }


        public bool FillObject(ref int currentPosition, List<string> fileContent)
        {
            this.Name = fileContent[currentPosition].Trim().Replace("Scope (", string.Empty).Replace(")", string.Empty);
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
                            Debug.WriteLine("[Scope] Line: {0} - Level: {1} - Inhalt: {2}", currentPosition + 1, level, fileContent[currentPosition]);
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
                        Debug.WriteLine("[Scope] Line: {0} - Level: {1} - Inhalt: {2}", currentPosition + 1, level, fileContent[currentPosition]);
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

        public override string ToString() => "Scope " + this.Name;
    }
}

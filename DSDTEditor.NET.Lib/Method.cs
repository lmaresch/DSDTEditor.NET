using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSDTEditor.NET.Shared;

namespace DSDTEditor.NET.Lib
{
    public class Method : DSDTProperty, IACPIObject
    {
        private string name;
        private ObservableCollection<IACPIObject> childs;

        public string Name { get => this.name; set => this.SetField(ref this.name, value); }
        public ObservableCollection<IACPIObject> Childs { get => this.childs; set => this.SetField(ref this.childs, value); }
        public ACPIObjectTypes ACPIObjectType { get => ACPIObjectTypes.ACPI_TYPE_METHOD; }
    }
}

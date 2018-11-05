using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSDTEditor.NET.Shared;

namespace DSDTEditor.NET.Lib
{
    public interface IACPIObject
    {
        string Name
        {
            get;
            set;
        }

        ObservableCollection<IACPIObject> Childs
        {
            get;
            set;
        }

        ACPIObjectTypes ACPIObjectType
        {
            get;
        }
    }
}

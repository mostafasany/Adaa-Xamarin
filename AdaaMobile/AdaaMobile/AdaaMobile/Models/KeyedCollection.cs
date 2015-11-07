using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.Models
{
    public class KeyedCollection<T> : ObservableCollection<T>
    {
        public string Key { get; set; }
    }
}

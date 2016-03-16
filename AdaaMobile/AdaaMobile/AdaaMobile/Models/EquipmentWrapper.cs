using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models.Response;

namespace AdaaMobile.Models
{
    public class EquipmentWrapper : BindableBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        private Equipment _equipment;

        public Equipment Equipment
        {
            get { return _equipment; }
            private set { SetProperty(ref _equipment, value); }
        }

        public EquipmentWrapper( Equipment equipment)
        {
            _equipment = equipment;
        }


    }
}

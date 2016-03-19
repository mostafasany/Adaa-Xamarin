using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models;
using AdaaMobile.Models.Response;

namespace AdaaMobile.ViewModels
{
    public class EquipmentsSelectionViewModel : BindableBase
    {

        #region Fields
        #endregion

        #region Properties

        private EquipmentWrapper[] _equipmentWrappers;
        public EquipmentWrapper[] EquipmentWrappers
        {
            get { return _equipmentWrappers; }
            set { SetProperty(ref _equipmentWrappers, value); }
        }

        #endregion

        #region Initialization
        public EquipmentsSelectionViewModel()
        {

        }
        #endregion

        #region Commands
        #endregion

        #region Methods

        public void InitializeWrappersList(Equipment[] allEquipments, List<Equipment> previousSelection)
        {
            if (allEquipments == null) EquipmentWrappers = null;
            EquipmentWrappers = allEquipments.
                Select(equipment => new EquipmentWrapper(equipment)
                {
                    IsSelected = previousSelection != null && previousSelection.FirstOrDefault(e => e.Id == equipment.Id) != null
                }).ToArray();
        }

        public List<Equipment> GetSelectedEquipments()
        {
            var wrappers = EquipmentWrappers;
            if (wrappers == null) return null;
            var selectedList = new List<Equipment>();
            foreach (var equipmentWrapper in wrappers)
            {
                if (equipmentWrapper.IsSelected)
                {
                    selectedList.Add(equipmentWrapper.Equipment);
                }
            }
            return selectedList;
        }

        #endregion

    }
}

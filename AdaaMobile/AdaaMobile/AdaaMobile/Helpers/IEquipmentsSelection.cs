using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models.Response;

namespace AdaaMobile.Views.EServices
{
    public interface IEquipmentsSelection
    {
        event EventHandler<List<Equipment>> EquipmentsSelected;

    }
}

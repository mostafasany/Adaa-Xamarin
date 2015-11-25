using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.Helpers
{
    public class SvgAssembly
    {
        private static Assembly _pcl;
        public static Assembly Pcl
        {
            get
            {
                if (_pcl == null) _pcl = typeof (App).GetTypeInfo().Assembly;
                return _pcl;
            }
        }
    }
}

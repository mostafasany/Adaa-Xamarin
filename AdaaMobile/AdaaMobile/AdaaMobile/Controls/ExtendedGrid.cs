using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Controls
{
    public class ExtendedGrid : Grid
    {
        public ExtendedGrid()
        {

        }

        public static readonly BindableProperty CornerRadiusProperty =
			BindableProperty.Create("CornerRadius", typeof(double),  typeof(ExtendedGrid), default(double));

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
    }
}

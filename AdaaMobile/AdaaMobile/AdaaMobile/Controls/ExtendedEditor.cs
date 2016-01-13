using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Controls
{
    public class ExtendedEditor : Editor
    {
        #region MaxLength

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create<ExtendedEditor, int>(p => p.MaxLength, 1000000);
        /// <summary>
        /// Default MaxLength is one million
        /// </summary>
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        #endregion
    }
}

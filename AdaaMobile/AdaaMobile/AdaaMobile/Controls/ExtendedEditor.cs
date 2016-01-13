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

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create<ExtendedEditor, int>(p => p.MaxLength, default(int));
        /// <summary>
        /// A positive value will limit the max length allowed in this Editor.
        /// Non positive values will clear Limit on Length, ex 0,-1...
        /// Default is 0, which means no Limit.
        /// </summary>
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        #endregion
    }
}

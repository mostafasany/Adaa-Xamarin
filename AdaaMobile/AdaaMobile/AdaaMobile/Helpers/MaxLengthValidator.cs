using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Helpers
{
    public class MaxLengthValidator : Behavior<Editor>
    {
        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create("MaxLength", typeof(int), typeof(MaxLengthValidator), 0);

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        protected override void OnAttachedTo(Editor bindable)
        {
            bindable.TextChanged += bindable_TextChanged;
        }

        private void bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 0 && e.NewTextValue.Length > MaxLength)
            {
                var editor = (Editor) sender;
                editor.Text = e.NewTextValue.Substring(0, MaxLength);
              
            }
        }

        protected override void OnDetachingFrom(Editor bindable)
        {
            bindable.TextChanged -= bindable_TextChanged;

        }
    }
}

using System.ComponentModel;
using AdaaMobile.Controls;
using AdaaMobile.iOS.CustomRenderers.ExtendedEditor;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]
namespace AdaaMobile.iOS.CustomRenderers.ExtendedEditor
{
    public class ExtendedEditorRenderer : EditorRenderer
    {
        protected virtual Controls.ExtendedEditor FormsElement
        {
            get { return Element as Controls.ExtendedEditor; }
        }

        protected virtual ExtendedUITextView ExtendedControl
        {
            get { return Control as ExtendedUITextView; }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new ExtendedUITextView());
            }

            if (e.NewElement != null)
            {
                SetMaxLength(FormsElement.MaxLength);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Controls.ExtendedEditor.MaxLengthProperty.PropertyName)
            {
                SetMaxLength(FormsElement.MaxLength);
            }
        }

        private void SetMaxLength(int maxLength)
        {
            if (ExtendedControl != null)
            {
                //TODO:Test change of limits scenario
                string text = ExtendedControl.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    //Trim Text, if the current text is larger than the new limit
                    if (text.Length > maxLength)
                    {
                        ExtendedControl.Text = text.Substring(0, maxLength);
                    }
                }

                //Set new Limit
                ExtendedControl.MaxLength = maxLength;
            }
        }
    }
}

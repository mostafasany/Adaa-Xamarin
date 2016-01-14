using System;
using Foundation;
using UIKit;
using AdaaMobile.Controls;

namespace AdaaMobile.iOS.CustomRenderers.ExtendedEditor
{
    public class ExtendedUiTextView : UITextView
    {
        private readonly ExtendedUiTextViewDelegate _extendedDelegate;

		public ExtendedUiTextView(AdaaMobile.Controls.ExtendedEditor formsElement)
        {
			_extendedDelegate = new ExtendedUiTextViewDelegate(formsElement);
            Delegate = _extendedDelegate;
        }

        public nint MaxLength
        {
            get { return _extendedDelegate.MaxLength; }
            set { _extendedDelegate.MaxLength = value; }
        }
    }

    public class ExtendedUiTextViewDelegate : UITextViewDelegate
    {
		private AdaaMobile.Controls.ExtendedEditor _formsElement;

		public ExtendedUiTextViewDelegate(AdaaMobile.Controls.ExtendedEditor formsElement){
			this._formsElement = formsElement;
		}

        public nint MaxLength { get; set; }

        public override bool ShouldChangeText(UITextView textView, NSRange range, string replacementText)
        {
            string wholeText = textView.Text;
            //Call base method if there is not limit on Length
            if (MaxLength <= 0)
                return base.ShouldChangeText(textView, range, replacementText);
            //Check whether the new change will exceed max length or not
            nint textLength = string.IsNullOrEmpty(wholeText) ? 0 : wholeText.Length;
            nint replacementLength = string.IsNullOrEmpty(replacementText) ? 0 : replacementText.Length;
            nint selectedLength = range.Length;
            return (textLength + (replacementLength - selectedLength)) <= MaxLength;
        }

		public override void Changed (UITextView textView)
		{
			_formsElement.Text = textView.Text;
		}

    }
}

using System;
using UIKit;

namespace AdaaMobile.iOS.CustomRenderers.ExtendedEditor
{
    public class ExtendedUITextView : UITextView
    {
        public nint MaxLength { get; set; }

        public override bool ShouldChangeTextInRange(UITextRange inRange, string replacementText)
        {
            //Call base method if there is not limit on Length
            if (MaxLength <= 0)
                return base.ShouldChangeTextInRange(inRange, replacementText);

            //Check whether the new change will exceed max length or not
            nint textLength = string.IsNullOrEmpty(Text) ? 0 : Text.Length;
            nint replacementLength = string.IsNullOrEmpty(replacementText) ? 0 : Text.Length;
            nint selectedLength = SelectedRange.Length;
            return (textLength + (replacementLength - selectedLength)) <= MaxLength;
        }
    }
}

//Link https://github.com/BradChase2011/Xamarin.Forms.Plugins

using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AdaaMobile.Controls
{
    public class ColorPairs : ObservableCollection<ColorPair>
    {

    }

    public class ColorPair
    {
        public Color OldColor { get; set; }
        public Color NewColor { get; set; }

        public ColorPair( Color in_OldColor, Color in_NewColor ) : this( )
        {
            OldColor = in_OldColor;
            NewColor = in_NewColor;
        }

        public ColorPair( )
        {

        }
    }
}

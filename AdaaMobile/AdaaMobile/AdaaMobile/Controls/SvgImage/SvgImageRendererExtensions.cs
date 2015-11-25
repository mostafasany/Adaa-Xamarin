//Link https://github.com/BradChase2011/Xamarin.Forms.Plugins

using System.Collections.Generic;
using System.Linq;
using NGraphics;
using NGraphics.Models.Brushes;
using NGraphics.Models.Elements;

namespace AdaaMobile.Controls
{
    public static class SvgImageRendererExtensions
    {
        public static void ReplaceColors( this ISvgImageRenderer This, Graphic inGraphic, IEnumerable<ColorPair> inColors  )
        {
            if( (inColors == null) || (inGraphic == null) )
                return;

            if( inColors.Count() == 0 )
                return;

            if( inGraphic != null )
            {
                foreach( NGraphics.Interfaces.IDrawable it in inGraphic.Children )
                {
                    if( it is NGraphics.Models.Elements.Path )
                        ReplaceColor(it as Path, inColors);
                    else if( it is NGraphics.Models.Elements.Group )
                        ReplaceColor(it as Group, inColors);
                }
            }
        }

        private static void ReplaceColor( Group inGroup, IEnumerable<ColorPair> inColors )
        {
            if( inGroup.Brush != null )
                ReplaceColor(inGroup.Brush, inColors);

            if( inGroup.Children.Count() == 0 )
                return;

            foreach( NGraphics.Interfaces.IDrawable it in inGroup.Children )
            {
                if( it is Group )
                    ReplaceColor(it as Group, inColors);
                else if( it is Path )
                    ReplaceColor((it as Path), inColors);
            }
        }

        private static void ReplaceColor( Path inPath, IEnumerable<ColorPair> inColors )
        {
            if( (inPath == null) || (inColors == null) )
                return;

            ReplaceColor(inPath.Brush, inColors);
        }

        private static void ReplaceColor( BaseBrush inBrush, IEnumerable<ColorPair> inColors )
        {
            if( (inBrush == null) || (inColors == null) )
                return;

            if( inColors.Count( ) == 0 )
                return;

            if( inBrush is NGraphics.Models.Brushes.GradientBrush )
            {
                GradientBrush typedBrush = inBrush as GradientBrush;
                foreach( GradientStop stop in typedBrush.Stops )
                {
                    ColorPair pair = inColors.FirstOrDefault(x => (stop.Color.Red == x.OldColor.R) && (stop.Color.Green == x.OldColor.G) && (stop.Color.Blue == x.OldColor.B) && (stop.Color.Alpha == x.OldColor.A));
                    if( pair != null )
                        stop.Color = new NGraphics.Models.Color(pair.NewColor.R, pair.NewColor.G, pair.NewColor.B, pair.NewColor.A);
                }
            }
            else if( inBrush is NGraphics.Models.Brushes.SolidBrush )
            {
                SolidBrush typedBrush = inBrush as SolidBrush;
                ColorPair pair = inColors.FirstOrDefault(x => (typedBrush.Color.Red == x.OldColor.R) && (typedBrush.Color.Green == x.OldColor.G) && (typedBrush.Color.Blue == x.OldColor.B) && (typedBrush.Color.Alpha == x.OldColor.A));
                if( pair != null )
                    typedBrush.Color = new NGraphics.Models.Color(pair.NewColor.R, pair.NewColor.G, pair.NewColor.B, pair.NewColor.A);
            }
        }
    }
}

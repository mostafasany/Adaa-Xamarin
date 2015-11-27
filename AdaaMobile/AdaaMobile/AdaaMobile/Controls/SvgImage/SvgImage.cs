//Link https://github.com/BradChase2011/Xamarin.Forms.Plugins

using System;
using System.Reflection;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace AdaaMobile.Controls
{
    public class SvgImage : Image
    {
        private bool _isTemplateLoaded = false;
        /// <summary>
        /// This is used to prevent loop in measuring
        /// </summary>
        private double _lastMeasuredWidth;
        #region Properties
        /// <summary>
        /// The path to the svg file
        /// </summary>
        public static readonly BindableProperty SvgPathProperty = BindableProperty.Create("SvgPath", typeof(string), typeof(SvgImage), default(string));

        /// <summary>
        /// The path to the svg file
        /// </summary>
        public string SvgPath
        {
            get { return (string)GetValue(SvgPathProperty); }
            set { SetValue(SvgPathProperty, value); }
        }

        /// <summary>
        /// The assembly containing the svg file
        /// </summary>
        public static readonly BindableProperty SvgAssemblyProperty = BindableProperty.Create("SvgAssembly", typeof(Assembly), typeof(SvgImage), default(Assembly));

        /// <summary>
        /// The assembly containing the svg file
        /// </summary>
        public Assembly SvgAssembly
        {
            get { return (Assembly)GetValue(SvgAssemblyProperty); }
            set { SetValue(SvgAssemblyProperty, value); }
        }

        /// <summary>
        /// Colors to replace upon render.
        /// </summary>
        public static readonly BindableProperty ReplacementColorsProperty = BindableProperty.Create<SvgImage, ColorPairs>(w => w.ReplacementColors, null, BindingMode.Default, null, OnReplacementColorsPropertyChanged, null, null, null);
        /// <summary>
        /// Colors to replace upon render.
        /// </summary>
        public ColorPairs ReplacementColors
        {
            get { return (ColorPairs)GetValue(ReplacementColorsProperty); }
            set { SetValue(ReplacementColorsProperty, value); }
        }
        #endregion

        #region Percentage
        /// <summary>
        /// This will affect getSizeRequest and will return new Size(Percentage * widthContsraint,Percentage * widthContsraint)
        /// </summary>
        public static readonly BindableProperty PercentageProperty =
            BindableProperty.Create<SvgImage, double>(p => p.Percentage, default(double),
            propertyChanged: (bindable, value, newValue) => ((SvgImage)bindable).OnPercentageChanged());

        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        private void OnPercentageChanged()
        {
            //if (Percentage > 0)
            //{
            //    double width = Parent.ParentView.Width * 0.5;
            //    WidthRequest = width;
            //    HeightRequest = width;
            //}
        }

        #endregion

        #region Startup
        public SvgImage() : base()
        {
            if (ReplacementColors == null)
                ReplacementColors = new ColorPairs();

            ApplyTemplate();
        }

        private void ApplyTemplate()
        {
            _isTemplateLoaded = true;
            OnReplacementColorsChanged();
        }
        #endregion

        #region Property Changes
        private static void OnReplacementColorsPropertyChanged(BindableObject d, ColorPairs inOldValue, ColorPairs inNewValue)
        {
            SvgImage element = d as SvgImage;

            if (element == null)
                return;

            if (inOldValue != null)
                inOldValue.CollectionChanged -= element.ColorPairs_CollectionChanged;

            if (inNewValue != null)
            {
                inNewValue.CollectionChanged -= element.ColorPairs_CollectionChanged;
                inNewValue.CollectionChanged += element.ColorPairs_CollectionChanged;
            }

            if (inNewValue == inOldValue)
                return;

            element.OnReplacementColorsChanged();
        }

        private void ColorPairs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // inefficient for now, should go by whats added and what is not --bwc
            OnReplacementColorsChanged();
        }

        private void OnReplacementColorsChanged()
        {
            if (!_isTemplateLoaded)
                return;

            //ISvgImageRenderer renderer = RendererFactory.GetRenderer(this) as ISvgImageRenderer;
            //if( renderer != null )
            //    renderer.Render( );
        }
        #endregion

        public override SizeRequest GetSizeRequest(double widthConstraint, double heightConstraint)
        {
            //Update if Percentage is set and width constraint is larger than zero
            if (Percentage > 0 && !double.IsPositiveInfinity(widthConstraint) && widthConstraint > 0)
            {
                ////Only update if there is change in widthConstraint
                if (Math.Abs(_lastMeasuredWidth - widthConstraint) > 1e-6 && Math.Abs(_lastMeasuredWidth - widthConstraint) > 1e-6)
                {
                    double width = widthConstraint * Percentage;
                    _lastMeasuredWidth = width;
                }
                else
                {
                    ;
                }
                return new SizeRequest(new Size(_lastMeasuredWidth, _lastMeasuredWidth), new Size(_lastMeasuredWidth, _lastMeasuredWidth));
            }
            else
            {
                return base.GetSizeRequest(widthConstraint, heightConstraint);
            }
        }
    }
}
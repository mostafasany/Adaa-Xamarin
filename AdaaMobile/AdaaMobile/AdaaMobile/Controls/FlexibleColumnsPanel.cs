using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Controls
{
    //Some code snippets are based on https://github.com/conceptdev/xamarin-forms-samples/blob/145a43fb6153fc069eeb99d86f0e219ad6d8fcac/Evolve13/Evolve13/Controls/WrapLayout.cs
    public class FlexibleColumnsPanel : Layout<View>
    {
        #region Fields

        private double[] columnWidths;
        #endregion

        #region Spacing bindable property
        public static readonly BindableProperty SpacingProperty =
            BindableProperty.Create<FlexibleColumnsPanel, double>(p => p.Spacing, 10.0,
                propertyChanged: (bindable, old, newvalue) => ((FlexibleColumnsPanel)bindable).OnSizeChanged());
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }
        #endregion

        #region ItemTemplate
        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create<FlexibleColumnsPanel, DataTemplate>
            (p => p.ItemTemplate, default(DataTemplate), propertyChanged: OnItemTemplateChanged);

        private static void OnItemTemplateChanged(BindableObject bindable, DataTemplate oldvalue, DataTemplate newvalue)
        {
            var columnsPanel = (FlexibleColumnsPanel)bindable;
            columnsPanel.OnSizeChanged();
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        #endregion

        #region ItemSource

        public static readonly BindableProperty ItemSourceProperty =
            BindableProperty.Create<FlexibleColumnsPanel, IEnumerable>
                (p => p.ItemSource, default(IEnumerable), propertyChanged: OnItemSourceChanged);

        private static void OnItemSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var columnsPanel = (FlexibleColumnsPanel)bindable;
            columnsPanel.OnSizeChanged();
        }

        public IEnumerable ItemSource
        {
            get { return (IEnumerable)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }
        #endregion


        #region ColumnHeight
        public static readonly BindableProperty ColumnHeightProperty =
            BindableProperty.Create<FlexibleColumnsPanel, double>(p => p.ColumnHeight, default(double)
                , propertyChanged: (bindable, value, newValue) => ((FlexibleColumnsPanel)bindable).OnSizeChanged());

        public double ColumnHeight
        {
            get { return (double)GetValue(ColumnHeightProperty); }
            set { SetValue(ColumnHeightProperty, value); }
        }
        #endregion

        #region ColumnHeightDefinition
        public static readonly BindableProperty ColumnHeightDefinitionProperty
            = BindableProperty.Create<FlexibleColumnsPanel, RowDefinition>(p => p.ColumnHeightDefinition, default(RowDefinition));

        public RowDefinition ColumnHeightDefinition
        {
            get { return (RowDefinition)GetValue(ColumnHeightDefinitionProperty); }
            set { SetValue(ColumnHeightDefinitionProperty, value); }
        }
        #endregion

        #region ColumnDefenitions
        public static readonly BindableProperty ColumnDefinitionsProperty =
            BindableProperty.Create<FlexibleColumnsPanel, ColumnDefinitionCollection>
            (p => p.ColumnDefinitions,
             new ColumnDefinitionCollection() { new ColumnDefinition() { Width = GridLength.Auto } }
             , propertyChanged: (bindable, value, newValue) => ((FlexibleColumnsPanel)bindable).OnSizeChanged());

        public ColumnDefinitionCollection ColumnDefinitions
        {
            get { return (ColumnDefinitionCollection)GetValue(ColumnDefinitionsProperty); }
            set { SetValue(ColumnDefinitionsProperty, value); }
        }
        #endregion
        protected void OnSizeChanged()
        {
            ForceLayout();
        }

        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            if (WidthRequest > 0)
                widthConstraint = Math.Min(widthConstraint, WidthRequest);
            if (HeightRequest > 0)
                heightConstraint = Math.Min(heightConstraint, HeightRequest);

            double internalWidth = double.IsPositiveInfinity(widthConstraint) ? double.PositiveInfinity : Math.Max(0, widthConstraint);
            double internalHeight = double.IsPositiveInfinity(heightConstraint) ? double.PositiveInfinity : Math.Max(0, heightConstraint);
            return DoMeasure(internalWidth, internalHeight);

        }

        private SizeRequest DoMeasure(double widthConstraint, double heightConstraint)
        {
            columnWidths = new double[ColumnDefinitions.Count];
            int columnCount = columnWidths.Length;
            if (columnCount == 0) return new SizeRequest(new Size(0, 0), new Size(0, 0));
            int childrenCount = Children.Count;
            int rowCount = (int)Math.Ceiling(childrenCount * 1.0 / columnCount);

            double starsCount = 0;
            double starWidth = 0;
            double autoAndAbsWidth = 0;
            //First calculate columns that are Auto and Absolute
            //Also count the number of stars and remaining width.
            //Please note that if internalConstrain is +Inf, stars will be treated like Auto
            for (int column = 0; column < columnCount; column++)
            {
                var gridLength = ColumnDefinitions[column].Width;
                if (gridLength.IsAuto || (double.IsPositiveInfinity(widthConstraint) && gridLength.IsStar))
                {
                    for (int row = 0; row < rowCount && (column * row) < childrenCount; row++)
                    {
                        var child = Children[column * row];
                        var request = child.GetSizeRequest(widthConstraint, heightConstraint);
                        columnWidths[column] = Math.Max(request.Request.Width, columnWidths[column]);
                    }
                    autoAndAbsWidth += columnWidths[column];
                }
                else if (gridLength.IsAbsolute)
                {
                    columnWidths[column] = gridLength.Value;
                    autoAndAbsWidth += columnWidths[column];
                }
                else if (gridLength.IsStar)
                {
                    starsCount += gridLength.Value;
                }
            }

            if (starsCount > 0)
            {
                starWidth = (widthConstraint - autoAndAbsWidth) / starsCount;

                //Now calculate star based widths
                for (int column = 0; column < columnCount; column++)
                {
                    var gridLength = ColumnDefinitions[column].Width;
                    if (gridLength.IsStar)
                    {
                        columnWidths[column] = starWidth * gridLength.Value;
                    }
                }
            }

            //Calculate row height
            double width = widthConstraint;
            double height = rowCount * ColumnHeight;
            double minWidth = widthConstraint;
            double minHeight = height;
            //for (int row = 0; row < rowCount; row++)
            //{
            //    double rowHeight = 0;
            //    for (int columnIndex = 0; columnIndex < columnCount && (row * columnIndex) < childrenCount; columnIndex++)
            //    {
            //        var child = Children[row * columnIndex];
            //        var request = child.GetSizeRequest(width, height);
            //        double childHeight = request.Request.Height;
            //        rowHeight = Math.Max(rowHeight, childHeight);
            //    }
            //}

            //TODO:calculate row height based on item size or rowCount*ColumnHeight
            return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (columnWidths.Length == 0) return;
            int rowCount = Children.Count / columnWidths.Length;
            double rowHeight = 0;
            double yPos = y, xPos = x;
            //int index = 0;
            for (int row = 0; row < rowCount; row++)
            {
                var child = Children[row];
                var request = child.GetSizeRequest(width, height);
                int column = row % columnWidths.Length;
                double childWidth = columnWidths[column];
                double childHeight = ColumnHeight > 0 ? ColumnHeight : request.Request.Height;
                rowHeight = Math.Max(rowHeight, childHeight);

                xPos = x + column * childWidth;

                var region = new Rectangle(xPos, yPos, childWidth, childHeight);
                LayoutChildIntoBoundingRegion(child, region);
                xPos += region.Width + Spacing;
            }
        }
    }
}


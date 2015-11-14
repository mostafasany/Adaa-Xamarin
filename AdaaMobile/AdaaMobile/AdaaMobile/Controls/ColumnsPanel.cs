using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Controls
{
    //Some code snippets are based on https://github.com/conceptdev/xamarin-forms-samples/blob/145a43fb6153fc069eeb99d86f0e219ad6d8fcac/Evolve13/Evolve13/Controls/WrapLayout.cs
    public class ColumnsPanel : Layout<View>
    {
        #region Fields
        #endregion

        #region VerticalSpacing bindable property
        public static readonly BindableProperty VerticalSpacingProperty =
            BindableProperty.Create<ColumnsPanel, double>(p => p.VerticalSpacing, 10.0,
                propertyChanged: (bindable, old, newvalue) => ((ColumnsPanel)bindable).OnSizeChanged());
        public double VerticalSpacing
        {
            get { return (double)GetValue(VerticalSpacingProperty); }
            set { SetValue(VerticalSpacingProperty, value); }
        }
        #endregion

        #region HorizontalSpacing bindable property
        public static readonly BindableProperty HorizontalSpacingProperty =
            BindableProperty.Create<ColumnsPanel, double>(p => p.HorizontalSpacing, 10.0,
                propertyChanged: (bindable, old, newvalue) => ((ColumnsPanel)bindable).OnSizeChanged());
        public double HorizontalSpacing
        {
            get { return (double)GetValue(HorizontalSpacingProperty); }
            set { SetValue(HorizontalSpacingProperty, value); }
        }
        #endregion

        #region ItemTemplate
        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create<ColumnsPanel, DataTemplate>
            (p => p.ItemTemplate, default(DataTemplate), propertyChanged: OnItemTemplateChanged);

        private static void OnItemTemplateChanged(BindableObject bindable, DataTemplate oldvalue, DataTemplate newvalue)
        {
            var columnsPanel = (ColumnsPanel)bindable;
            columnsPanel.OnSizeChanged();
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        #endregion



        #region ColumnCount
        public static readonly BindableProperty ColumnCountProperty =
            BindableProperty.Create<ColumnsPanel, int>(p => p.ColumnCount, default(int)
                , propertyChanged: (bindable, value, newValue) => ((ColumnsPanel)bindable).OnSizeChanged());

        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }


        #region ColumnHeight
        public static readonly BindableProperty ColumnHeightProperty =
            BindableProperty.Create<ColumnsPanel, double>(p => p.ColumnHeight, default(double)
                , propertyChanged: (bindable, value, newValue) => ((ColumnsPanel)bindable).OnSizeChanged());

        public double ColumnHeight
        {
            get { return (double)GetValue(ColumnHeightProperty); }
            set { SetValue(ColumnHeightProperty, value); }
        }
        #endregion

        #region ColumnWidth
        public static readonly BindableProperty ColumnWidthProperty =
            BindableProperty.Create<ColumnsPanel, double>(p => p.ColumnWidth, default(double)
                , propertyChanged: (bindable, value, newValue) => ((ColumnsPanel)bindable).OnSizeChanged());

        public double ColumnWidth
        {
            get { return (double)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }
        #endregion

        #region ItemSource

        public static readonly BindableProperty ItemSourceProperty =
            BindableProperty.Create<ColumnsPanel, IEnumerable>
                (p => p.ItemSource, default(IEnumerable), propertyChanged: OnItemSourceChanged);

        private static void OnItemSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var columnsPanel = (ColumnsPanel)bindable;

            if (oldvalue is INotifyCollectionChanged)
            {
                var coll = (INotifyCollectionChanged)oldvalue;
                // Unsubscribe from CollectionChanged on the old collection
                coll.CollectionChanged += columnsPanel.OnCollectionChanged;
            }

            if (newvalue is INotifyCollectionChanged)
            {
                var coll = (INotifyCollectionChanged)newvalue;
                // Subscribe to CollectionChanged on the new collection
                coll.CollectionChanged += columnsPanel.OnCollectionChanged;
            }

            //Trigger reloading of item source
            var args = new NotifyCollectionChangedEventArgs
                (NotifyCollectionChangedAction.Reset);
            columnsPanel.OnCollectionChanged(columnsPanel, args);
            //columnsPanel.OnSizeChanged();
        }

        public IEnumerable ItemSource
        {
            get { return (IEnumerable)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }
        #endregion

        #endregion

        #region Events

        #endregion


        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            BatchBegin();
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                Children.Clear();
                AddItems(ItemSource as IList);
            }
            else
            {
                RemoveItems(args.OldItems);
                AddItems(args.NewItems);
            }
            BatchCommit();
        }

        private void RemoveItems(IList oldItems)
        {
            foreach (object item in oldItems)
            {
                var matchedChild = Children.FirstOrDefault((v) => v.BindingContext == item);
                if (matchedChild != null)
                    Children.Remove(matchedChild);
            }
        }

        private void AddItems(IList items)
        {
            foreach (object item in items)
            {
                var child = ItemTemplate.CreateContent() as View;
                if (child == null)
                    return;

                child.BindingContext = item;
                Children.Add(child);
            }
        }

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
            if (ColumnCount == 0 ||
                Math.Abs(ColumnWidth) < 1e-6 ||
                Math.Abs(ColumnHeight) < 1e-6)
                return new SizeRequest(new Size(0, 0), new Size(0, 0));
            int childrenCount = Children.Count;
            int rowCount = (int)Math.Ceiling(childrenCount * 1.0 / ColumnCount);

            //Calculate row height
            double width = ColumnCount * ColumnWidth + (ColumnCount - 1) * HorizontalSpacing;
            double height = rowCount * ColumnHeight + (rowCount ) * VerticalSpacing;
            double minWidth = width;
            double minHeight = height;

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
            double rowHeight = 0;
            double yPos = y, xPos = x;
            for (int index = 0; index < Children.Count; index++)
            {
                var child = Children[index];
                int row = index / ColumnCount;
                int column = index % ColumnCount;
                var request = child.GetSizeRequest(width, height);
                double childWidth = ColumnWidth;
                double childHeight = ColumnHeight > 0 ? ColumnHeight : request.Request.Height;

                //Update yPos and reset row height
                if (column == 0)
                {
                    yPos += rowHeight + VerticalSpacing;
                    rowHeight = 0;
                }
                rowHeight = Math.Max(rowHeight, childHeight);
                xPos = x + (column * (childWidth + HorizontalSpacing));
                var region = new Rectangle(xPos, yPos, childWidth, childHeight);
                LayoutChildIntoBoundingRegion(child, region);
            }
        }    
    }
}


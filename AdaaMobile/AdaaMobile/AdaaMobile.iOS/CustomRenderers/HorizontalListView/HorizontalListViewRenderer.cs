using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AdaaMobile.Controls;
using AdaaMobile.iOS.CustomRenderers.HorizontalListView;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PropertyChangingEventArgs = Xamarin.Forms.PropertyChangingEventArgs;

[assembly: ExportRenderer(typeof(HorizontalListView), typeof(HorizontalListViewRenderer))]
namespace AdaaMobile.iOS.CustomRenderers.HorizontalListView
{
    //Some snippets and code structure are adapted from XLabs Grid view  
    //See https://github.com/XLabs/Xamarin-Forms-Labs/tree/master/src/Forms/XLabs.Forms.iOS/Controls/GridView
    public class HorizontalListViewRenderer : ViewRenderer<Controls.HorizontalListView, UICollectionView>
    {
        private HorizontalCollectionView _collectionView;

        /// <summary>
        /// The data source
        /// </summary>
        private HorizontalListDataSource _dataSource;

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>The data source.</value>
        private HorizontalListDataSource DataSource
        {
            get
            {
                return _dataSource ??
                    (_dataSource =
                        new HorizontalListDataSource(GetCell, this.GetItemsCount, this.RaiseItemTappedEvent));
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Controls.HorizontalListView> e)
        {

            base.OnElementChanged(e);

            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                Unbind(e.OldElement);
            }

            if (e.NewElement != null)
            {
                _collectionView = new HorizontalCollectionView();
                _collectionView.AllowsMultipleSelection = false;
                _collectionView.SelectionEnable = false;//Not in the plan currently

                _collectionView.BackgroundColor = Element.BackgroundColor.ToUIColor();
				_collectionView.ItemSize = new CoreGraphics.CGSize((float)100, (float)200);
//                _collectionView.ItemSize = new CoreGraphics.CGSize((float)Element.ItemWidth, (float)Element.ItemHeight);
				_collectionView.RowSpacing = 0;//Todo
				_collectionView.ColumnSpacing = Element.ColumnSpacing;
				Element.HeightRequest = 200;
                //Horizontal 
                UICollectionViewFlowLayout flowLayout = (UICollectionViewFlowLayout)_collectionView.CollectionViewLayout;
				flowLayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
				flowLayout.MinimumInteritemSpacing = 200000000f;

                //Set Source and Delegate
                _collectionView.Source = DataSource;
                _collectionView.Delegate = new HorizontalListDelegate();

                SetNativeControl(_collectionView);

                Bind(e.NewElement);
            }
        }

        /// <summary>
        /// Unbinds the specified old element.
        /// </summary>
        /// <param name="oldElement">The old element.</param>
        private void Unbind(Controls.HorizontalListView oldElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanging -= ElementPropertyChanging;
                oldElement.PropertyChanged -= ElementPropertyChanged;
                if (oldElement.ItemsSource is INotifyCollectionChanged)
                {
                    (oldElement.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
                }
            }
        }

        /// <summary>
        /// Binds the specified new element.
        /// </summary>
        /// <param name="newElement">The new element.</param>
        private void Bind(Controls.HorizontalListView newElement)
        {
            if (newElement != null)
            {
                newElement.PropertyChanging += ElementPropertyChanging;
                newElement.PropertyChanged += ElementPropertyChanged;
                if (newElement.ItemsSource is INotifyCollectionChanged)
                {
                    (newElement.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
                }
            }
        }

        private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                if (this.Element.ItemsSource is INotifyCollectionChanged)
                {
                    (this.Element.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
                }
            }
        }

        private void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                if (this.Element.ItemsSource is INotifyCollectionChanged)
                {
                    (this.Element.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
                }
                //Reset Data source
                this.Control.ReloadData();
            }
        }

        private void DataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO:Update changes instead of reloading

            if (this.Control != null) this.Control.ReloadData();

        }

        /// <summary>
        /// Retieves the item at 'position' from Element.ItemSource.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private object GetItem(int position)
        {
            object item;
            if (Element.ItemsSource is IList)
            {
                item = (Element.ItemsSource as IList)[position];
            }
            else
            {
                item = this.Element.ItemsSource.Cast<object>().ElementAt(position);
            }

            return item;
        }


        private void RaiseItemTappedEvent(UICollectionView collectionView, NSIndexPath indexPath)
        {
			var currentCell = (HorizontalListCell)collectionView.CellForItem (indexPath);

			var args = new HorizontaListItemTappedEventArgs(currentCell.ViewCell.View, currentCell.ViewCell.BindingContext);
            if (Element != null)
            {
                Element.RaiseItemTapped(args);
            }
        }

        /// <summary>
        /// Gets items source count. It's used primarily in Data source.
        /// </summary>
        /// <returns></returns>
        private nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            var collection = this.Element.ItemsSource as IList;
            if (collection != null) return collection.Count;

            return Element.ItemsSource.Cast<object>().Count();
        }


        private NSString cellId;

        //Modifications to this method copied from https://github.com/twintechs/TwinTechsFormsLib/blob/master/TwinTechsForms/TwinTechsForms.iOS/XLabs/Forms/Controls/GridView/GridViewRenderer.cs
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="indexPath">Index path.</param>
        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            cellId = cellId ?? new NSString(HorizontalListCell.Key);
			var item = GetItem (indexPath.Row);
            var collectionCell = collectionView.DequeueReusableCell(cellId, indexPath) as HorizontalListCell;

            collectionCell.RecycleCell(item, Element.ItemTemplate, Element);
            return collectionCell;
        }

        /// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && _dataSource != null)
            {
                Unbind(Element);
                _dataSource.Dispose();
                _dataSource = null;
            }
        }
    }


}

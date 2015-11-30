using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using AdaaMobile.Controls;
using AdaaMobile.Droid.CustomRenderers;
using AdaaMobile.Models;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PropertyChangingEventArgs = Xamarin.Forms.PropertyChangingEventArgs;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(HorizontalListView), typeof(HorizontalListViewRenderer))]
namespace AdaaMobile.Droid.CustomRenderers
{
    //Some snippets related to recycler view of the Renderer is from xamrin android sample 
    //See: https://developer.xamarin.com/guides/android/user_interface/recyclerview/
    //Some snippets related to data source structure inspired from XLabs GridView
    //See: https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/src/Forms/XLabs.Forms.Droid/Controls/GridView/GridViewRenderer.cs
    public class HorizontalListViewRenderer : ViewRenderer<HorizontalListView, RecyclerView>
    {
        private RecyclerView _recyclerView;
        private HorizontalListLayoutManager _layoutManager;
        private bool _isSpecificHeight;//A value of false will update the List height to wrap childs.
        /// <summary>
        /// The data source
        /// </summary>
        private HorizontalListViewAdapter _dataSource;

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>The data source.</value>
        private HorizontalListViewAdapter DataSource
        {
            get
            {
                return _dataSource ??
                    (_dataSource =
                        new HorizontalListViewAdapter(this.OnBindViewHolder, this.OnCreateViewHolder, this.GetItemsCount));
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<HorizontalListView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                Unbind(e.OldElement);
            }

            if (e.NewElement != null)
            {
                Bind(e.NewElement);
                if (Element.HeightRequest > 0) _isSpecificHeight = true;
                _recyclerView = new RecyclerView(Xamarin.Forms.Forms.Context);
                _layoutManager = new HorizontalListLayoutManager(Xamarin.Forms.Forms.Context, LinearLayoutManager.Horizontal, false);
                _recyclerView.SetLayoutManager(_layoutManager);
                _recyclerView.HasFixedSize = false;
                _recyclerView.SetAdapter(DataSource);
                _recyclerView.Background = new ColorDrawable(Color.Transparent.ToAndroid());
                _recyclerView.LayoutChange += _recyclerView_LayoutChange;
                base.SetNativeControl(_recyclerView);
            }
        }

        private void _recyclerView_LayoutChange(object sender, LayoutChangeEventArgs e)
        {
            //TODO:Test to make sure it doesn't make performance issues
            if (_layoutManager != null && !_isSpecificHeight)
            {
                //This the is the only way I find so far to update the size in Xamarin forms
                //when list height is auto and not hardwired.
                int height = _layoutManager.GetMeasuredHeight();
                if (height > 0)
                {
                    Element.HeightRequest = DpToPixel(height);
                }
            }
        }

        /// <summary>
        /// Unbinds the specified old element.
        /// </summary>
        /// <param name="oldElement">The old element.</param>
        private void Unbind(HorizontalListView oldElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanging += ElementPropertyChanging;
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
        private void Bind(HorizontalListView newElement)
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
                _dataSource.NotifyDataSetChanged();
            }
        }

        private void DataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO:Test different scenarios
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    _dataSource.NotifyItemInserted(e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    _dataSource.NotifyItemRemoved(e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    _dataSource.NotifyItemChanged(e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    _dataSource.NotifyItemMoved(e.OldStartingIndex, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _dataSource.NotifyDataSetChanged();
                    break;

            }
        }

        private void RaiseItemTappedEvent(HorizontaListItemTappedEventArgs args)
        {
            if (Element != null)
            {
                Element.RaiseItemTapped(args);
            }
        }

        /// <summary>
        /// This method is passed to HorizontalListViewAdapter to create view holder.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        private RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var viewCellBinded = (Element.ItemTemplate.CreateContent() as ViewCell);
            System.Diagnostics.Debug.Assert(viewCellBinded != null, "viewCellBinded != null");

            var renderer = XamarinRendererHelper.Convert(viewCellBinded.View, Element);
            System.Diagnostics.Debug.Assert(renderer != null, "renderer != null");

            return new HorizontalListViewHolder(renderer.ViewGroup, renderer, RaiseItemTappedEvent);
        }

        /// <summary>
        /// This method is passed to HorizontalListViewAdapter to bind view holder.
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var bindedHolder = (HorizontalListViewHolder)holder;
            //retrieve item
            object item = GetItem(position);

            //Set binding context
            bindedHolder.Renderer.Element.BindingContext = item;

            //Update width and height based on binding data
            var renderer = bindedHolder.Renderer;
            var element = bindedHolder.Renderer.Element;

            //Update layout
            var requestSize = element.GetSizeRequest(double.PositiveInfinity, double.PositiveInfinity);
            Rectangle rect = new Rectangle(0, 0, requestSize.Request.Width, requestSize.Request.Height);
            element.Layout(rect);
            var layoutParams = new ViewGroup.LayoutParams((int)PixelToDp(rect.Width), (int)PixelToDp(rect.Height));
            renderer.ViewGroup.LayoutParameters = layoutParams;
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

        /// <summary>
        /// Gets items source count.
        /// </summary>
        /// <returns></returns>
        public int GetItemsCount()
        {
            var collection = this.Element.ItemsSource as IList;
            if (collection != null) return collection.Count;

            return Element.ItemsSource.Cast<object>().Count();
        }

        /// <summary>
        /// http://stackoverflow.com/questions/24465513/how-to-get-detect-screen-size-in-xamarin-forms
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns>Dp to be used inside android renderer.</returns>
        private double PixelToDp(double pixel)
        {
            var scale = Resources.DisplayMetrics.Density;
            return (double)((pixel * scale) + 0.5f);
        }

        /// <summary>
        /// Returns pixel to be used inside xamarin forms
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        private double DpToPixel(double dp)
        {
            var scale = Resources.DisplayMetrics.Density;
            return (double)((dp - 0.5f) / scale);
        }
    }


    /// <summary>
    /// Adapter for Recyclerview, It uses delegate methods to pass functionalities.
    /// </summary>
    public class HorizontalListViewAdapter : RecyclerView.Adapter
    {
        //Delegates
        public delegate void OnBindViewHolderDelegate(RecyclerView.ViewHolder holder, int position);

        public delegate RecyclerView.ViewHolder OnCreateViewHolderDelegate(ViewGroup parent, int viewType);

        public delegate int GetItemsCount();

        //Fields
        private readonly OnBindViewHolderDelegate _bindDelegate;
        private readonly OnCreateViewHolderDelegate _createDelegate;
        private readonly GetItemsCount _itemsCountDelegate;

        public HorizontalListViewAdapter(OnBindViewHolderDelegate bindDelegate, OnCreateViewHolderDelegate createDelegate, GetItemsCount itemsCountDelegate) : base()
        {
            _bindDelegate = bindDelegate;
            _createDelegate = createDelegate;
            _itemsCountDelegate = itemsCountDelegate;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _bindDelegate(holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return _createDelegate(parent, viewType);
        }


        public override int ItemCount
        {
            get { return _itemsCountDelegate(); }
        }
    }

    /// <summary>
    /// Custom view holder for views of Recylcerview
    /// </summary>
    public class HorizontalListViewHolder : RecyclerView.ViewHolder
    {
        public IVisualElementRenderer Renderer { get; set; }

        public Action<HorizontaListItemTappedEventArgs> ItemTapped { get; set; }

        public HorizontalListViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public HorizontalListViewHolder(View itemView) : base(itemView)
        {
        }

        public HorizontalListViewHolder(ViewGroup itemView, IVisualElementRenderer rendererBinded, Action<HorizontaListItemTappedEventArgs> itemTapped) : base(itemView)
        {
            Renderer = rendererBinded;
            ItemTapped = itemTapped;
            //wire click event, 
            EventHandler handler = null;
            handler = (sender, args) => { itemTapped(new HorizontaListItemTappedEventArgs((Xamarin.Forms.View)Renderer.Element, Renderer.Element.BindingContext)); };
            ItemView.Click -= handler; //remove handler to prevent multiple supscription, (It's a little bit defensive)
            itemView.Click += handler;
        }
    }

    #region Platform Render Workaround code

    public class XamarinRendererHelper
    {
        // Solution for render issue added from this links
        // Thanks for thaihung203 for the brilliant solution
        // See https://forums.xamarin.com/discussion/comment/148210/#Comment_148210
        // and https://github.com/thaihung203/xfpopup/blob/master/xfpopup/xfpopup.Droid/DroidXFPopupSrvc.cs
        private static Type _platformType = Type.GetType("Xamarin.Forms.Platform.Android.Platform, Xamarin.Forms.Platform.Android", true);

        private static BindableProperty _rendererProperty;

        public static BindableProperty RendererProperty
        {
            get { return _rendererProperty ?? (_rendererProperty = (BindableProperty)_platformType.GetField("RendererProperty", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public).GetValue(null)); }
        }

        private static PropertyInfo _isplatformenabledprop;

        public static PropertyInfo IsPlatformEnabledProperty
        {
            get { return _isplatformenabledprop ?? (_isplatformenabledprop = typeof(VisualElement).GetProperty("IsPlatformEnabled", BindingFlags.NonPublic | BindingFlags.Instance)); }
        }

        private static PropertyInfo _platform;

        public static PropertyInfo PlatformProperty
        {
            get { return _platform ?? (_platform = typeof(VisualElement).GetProperty("Platform", BindingFlags.NonPublic | BindingFlags.Instance)); }
        }

        public static IVisualElementRenderer Convert(Xamarin.Forms.View source, Xamarin.Forms.View valid)
        {
            IVisualElementRenderer render = (IVisualElementRenderer)source.GetValue(RendererProperty);
            if (render == null)
            {
                render = Platform.CreateRenderer(source);
                source.SetValue(RendererProperty, render);
                var p = PlatformProperty.GetValue(valid);
                PlatformProperty.SetValue(source, p);
                IsPlatformEnabledProperty.SetValue(source, true);
            }

            return render;
        }
    }

    #endregion
}

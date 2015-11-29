using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AdaaMobile.Controls;
using AdaaMobile.Droid.CustomRenderers;
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
using View = Android.Views.View;

[assembly:ExportRenderer(typeof(HorizontalListView), typeof(HorizontalListViewRenderer))]
namespace AdaaMobile.Droid.CustomRenderers
{
    public class HorizontalListViewRenderer : ViewRenderer<HorizontalListView, RecyclerView>
    {
        private RecyclerView _recyclerView;
        private RecyclerView.LayoutManager _layoutManager;

        protected override void OnElementChanged(ElementChangedEventArgs<HorizontalListView> e)
        {
            base.OnElementChanged(e);

            _recyclerView = new RecyclerView(Xamarin.Forms.Forms.Context);
            _layoutManager = new LinearLayoutManager(Xamarin.Forms.Forms.Context, LinearLayoutManager.Horizontal, false);
            _recyclerView.SetLayoutManager(_layoutManager);
            _recyclerView.HasFixedSize = false;
            _recyclerView.SetAdapter(DataSource);
            
            base.SetNativeControl(_recyclerView);
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        /// <summary>
        /// Unbinds the specified old element.
        /// </summary>
        /// <param name="oldElement">The old element.</param>
        private void Unbind(HorizontalListView oldElement)
        {
            if (oldElement != null)
            {

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

            }
        }

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

        private RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //var viewCellBinded = (Element.ItemTemplate.CreateContent() as ViewCell);
            //System.Diagnostics.Debug.Assert(viewCellBinded != null, "viewCellBinded != null");
            //var renderer = Platform.CreateRenderer(viewCellBinded.View);
            //renderer.ViewGroup.SetBackgroundColor(global::Android.Graphics.Color.Blue);
            //return new HorizontalListViewHolder(renderer.ViewGroup, renderer);
            var tempView = new TextView(Xamarin.Forms.Forms.Context);
            tempView.Text = "Test";
            tempView.Background= new ColorDrawable(Color.Aqua.ToAndroid());
            tempView.SetWidth(100);
            tempView.SetHeight(100);
            return new HorizontalListViewHolder(tempView);
        }

        public void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            //var bindedHolder = (HorizontalListViewHolder)holder;
            //var item = this.Element.ItemsSource.Cast<object>().ElementAt(position);
            //bindedHolder.Renderer.Element.BindingContext = item;
        }

        public int GetItemsCount()
        {
            var collection = this.Element.ItemsSource as ICollection;
            if (collection != null) return collection.Count;

            return Element.ItemsSource.Cast<object>().Count();
        }
    }



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

    public class HorizontalListViewHolder : RecyclerView.ViewHolder
    {
        public IVisualElementRenderer Renderer { get; set; }

        public HorizontalListViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public HorizontalListViewHolder(View itemView) : base(itemView)
        {
        }

        public HorizontalListViewHolder(ViewGroup itemView, IVisualElementRenderer rendererBinded) : base(itemView)
        {
            Renderer = rendererBinded;
        }
    }

}
